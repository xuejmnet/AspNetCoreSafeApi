using System.Threading.Tasks;
using AspNetCoreSafe.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AspNetCoreSafeApi.AuthSecurityBinder.RsaBinder
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 19 November 2020 15:02:48
* @Email: 326308290@qq.com
*/
    public class EncryptBodyModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var httpContext = bindingContext.HttpContext;
            //if (bindingContext.ModelType != typeof(string))
            //    return;
            string authorization = httpContext.Request.Headers["AuthSecurity-Authorization"];
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                //有参数接收就反序列化并且进行校验
                if (bindingContext.ModelType != null)
                {
                    //获取请求体
                    var encryptBody = await httpContext.Request.RequestBodyAsync();
                    if (string.IsNullOrWhiteSpace(encryptBody))
                        return;
                    //解密
                    var rsaOptions = httpContext.RequestServices.GetService<RsaOptions>();
                    var body = RsaFunc.Decrypt(rsaOptions.PrivateKey, encryptBody);
                    var request = JsonConvert.DeserializeObject(body, bindingContext.ModelType);
                    if (request == null)
                    {
                        return;
                    }
                    bindingContext.Result = ModelBindingResult.Success(request);

                }
            }
        }
    }
}