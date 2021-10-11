using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSafe.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AspNetCoreSafeApi
{
    public class SafeResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RsaOptions _rsaOptions;

        public SafeResponseMiddleware(RequestDelegate next,RsaOptions rsaOptions)
        {
            _next = next;
            _rsaOptions = rsaOptions;
        }

        public async Task Invoke(HttpContext context)
        {

            //AuthSecurity-Authorization
            if ( context.Request.Headers.TryGetValue("AuthSecurity-Authorization", out var authorization) && !string.IsNullOrWhiteSpace(authorization))
            {
                //获取Response.Body内容
                var originalBodyStream = context.Response.Body;
                await using (var newResponse = new MemoryStream())
                {
                    //替换response流
                    context.Response.Body = newResponse;
                    await _next(context);
                    string responseString = null;
                    var identityIsAuthenticated = context.User?.Identity?.IsAuthenticated;
                    if (identityIsAuthenticated.HasValue && identityIsAuthenticated.Value)
                    {
                        var authorizationSplit = authorization.ToString().Split('.');
                        var requestId = authorizationSplit[0];
                        var appid = authorizationSplit[1];

                        using (var reader = new StreamReader(newResponse))
                        {
                            newResponse.Position = 0;
                            responseString = (await reader.ReadToEndAsync())??string.Empty;
                            var responseStr = JsonConvert.SerializeObject(responseString);
                            var app = AppCallerStorage.ApiCallers.FirstOrDefault(o => o.Id == appid);
                            var encryptBody = RsaFunc.Encrypt(app.PublickKey, responseStr);
                            var signature = RsaFunc.CreateSignature(_rsaOptions.PrivateKey, $"{requestId}{appid}{encryptBody}");
                            context.Response.Headers.Add("AuthSecurity-Signature", signature);
                            responseString = encryptBody;
                        }

                        await using (var writer = new StreamWriter(originalBodyStream))
                        {
                            await writer.WriteAsync(responseString);
                            await writer.FlushAsync();
                        }
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
