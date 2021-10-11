using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreSafeApi
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 18 November 2020 17:18:55
* @Email: 326308290@qq.com
*/
    public static class RequestExtension
    {
        
        public static async Task<string> RequestBodyAsync(this HttpRequest request)
        {
            var buffsize = (int)(request.ContentLength ?? 10240);
            if (buffsize > 0)
            {
                string datas;
                request.EnableBuffering();
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, true, buffsize, true))
                {
                    datas = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
                return datas;
            }
            return string.Empty;
        }
    }
}