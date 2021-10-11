using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCoreSafe.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCoreSafeApi.SecurityAuthorization.RsaChecker
{
    public class AuthSecurityRsaAuthenticationHandler: AuthenticationHandler<AuthSecurityRsaOptions>
    {

        private readonly ConcurrentDictionary<string, object> _repeatRequestMap =
            new ConcurrentDictionary<string, object>();

        public AuthSecurityRsaAuthenticationHandler(IOptionsMonitor<AuthSecurityRsaOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                string authorization = Request.Headers["AuthSecurity-Authorization"];
                // If no authorization header found, nothing to process further
                if (string.IsNullOrWhiteSpace(authorization))
                    return AuthenticateResult.NoResult();

                var authorizationSplit = authorization.Split('.');
                if (authorizationSplit.Length != 4)
                    return await AuthenticateResultFailAsync("签名参数不正确");
                var reg = new Regex(@"[0-9a-zA-Z]{1,40}");


                var requestId = authorizationSplit[0];
                if (string.IsNullOrWhiteSpace(requestId) || !reg.IsMatch(requestId))
                    return await AuthenticateResultFailAsync("请求Id不正确");


                var appid = authorizationSplit[1];
                if (string.IsNullOrWhiteSpace(appid) || !reg.IsMatch(appid))
                    return await AuthenticateResultFailAsync("应用Id不正确");


                var timeStamp = authorizationSplit[2];
                if (string.IsNullOrWhiteSpace(timeStamp) || !long.TryParse(timeStamp, out var timestamp))
                    return await AuthenticateResultFailAsync("请求时间不正确");
                //请求时间大于30分钟的就抛弃
                if (Math.Abs(UtcTime.CurrentTimeMillis() - timestamp) > 30 * 60 * 1000)
                    return await AuthenticateResultFailAsync("请求已过期");


                var sign = authorizationSplit[3];
                if (string.IsNullOrWhiteSpace(sign))
                    return await AuthenticateResultFailAsync("签名参数不正确");
                //数据库获取
                //Request.HttpContext.RequestServices.GetService<DbContext>()
                var app = AppCallerStorage.ApiCallers.FirstOrDefault(o=>o.Id==appid);
                if (app == null)
                    return AuthenticateResult.Fail("未找到对应的应用信息");
                //获取请求体
                var body = await Request.RequestBodyAsync();

                //验证签名
                if (!RsaFunc.ValidateSignature(app.PublickKey, $"{requestId}{appid}{timeStamp}{body}", sign))
                    return await AuthenticateResultFailAsync("签名失败");
                var repeatKey = $"AuthSecurityRequestDistinct:{appid}:{requestId}";
                //自行替换成缓存或者redis本项目不带删除key功能没有过期时间原则上需要设置1小时过期,前后30分钟服务器时间差
                if (_repeatRequestMap.ContainsKey(repeatKey) || !_repeatRequestMap.TryAdd(repeatKey,null))
                {
                    return await AuthenticateResultFailAsync("请勿重复提交");
                }


                //给Identity赋值
                var identity = new ClaimsIdentity(AuthSecurityRsaDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim("appid", appid));
                identity.AddClaim(new Claim("appname", app.Name));
                identity.AddClaim(new Claim("role", "app"));
                //......

                var principal = new ClaimsPrincipal(identity);
                return HandleRequestResult.Success(new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RSA签名失败");
                return await AuthenticateResultFailAsync("认证失败");
            }
        }

        private async Task<AuthenticateResult> AuthenticateResultFailAsync(string message)
        {
            Response.StatusCode = 401;
            await Response.WriteAsync(message);
            return AuthenticateResult.Fail(message);
        }
    }
}
