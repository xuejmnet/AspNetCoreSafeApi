using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSafeApi
{
    public class AppCallerStorage
    {
        private AppCallerStorage()
        {
            
        }

        public static List<ApiCaller> ApiCallers = new List<ApiCaller>()
        {
            new ApiCaller()
            {
                Id = "1",
                Name = "我是1",
                PublickKey = "<RSAKeyValue><Modulus>vKQbdjZ4b4foZOq4RkUfQ4COkJ6htxW+S1fTZvTC8HbaaicTYWi9WICzeTd5PuAaDBwztk0HsS3r6ds1HSy//Wb7JsBE8ynrALaVPApNn54yQsTqEPmuGiTMYEBFIdyNwKzgdxFz6MWO7An2yWsenC0IEpSdntL918eVixt4aKOl39mtftSK2vVBhL+tljLzTkk6KZvjFGmGxf4dZFeSlU7H8BQ+zQKfkyViDeKgewFqrRsc2JGCCKChr1paVampBE/lpb6hzUPLUUQbpFHKBlIwtmut7m+Ly0fylbO0lSO06Q3CoEPLYfc81dTnPvMKRO6SEgFPpQBepqhLzedRTw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
            }
        };
    }
    public class ApiCaller
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PublickKey { get; set; }
    }
}
