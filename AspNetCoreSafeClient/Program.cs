using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using AspNetCoreSafe.Common;
using Newtonsoft.Json;

namespace AspNetCoreSafeClient
{
    class Program
    {
        private const string SysPublicKey = "<RSAKeyValue><Modulus>x29/gr6KX2UwgDrXE44bcg9/F1A4PcmINeBlIEbjR3Yv/hj/mNj1hlPedMrDpGK37A1xZ6W46eoXvDSY2c3mbJwlbpevjCz/GEBshUkDHXRN/kzemr3rOGI+sL47RwBSaCKG1L1oZ0zD5hAuApx0W5noKjGDPAVElW/OvjQ0vHiA1UDYazaqMXw8jBx4uXZR1tjJQ8+f6hvOE+pc7Qq3wHmNkfCw+u8Llg+p+MtIfQ5kK6+R0/E4dJ8IjuftDKBCRkD2yDKo25tLIx0q/SN8DFNvxUIPMoaHYUP7GAHbRTSYMvTVUm+MIAFLc4YetsxRhWw8TD9AMIM7jo87Vq4ENQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string AppPrivateKey = "<RSAKeyValue><Modulus>vKQbdjZ4b4foZOq4RkUfQ4COkJ6htxW+S1fTZvTC8HbaaicTYWi9WICzeTd5PuAaDBwztk0HsS3r6ds1HSy//Wb7JsBE8ynrALaVPApNn54yQsTqEPmuGiTMYEBFIdyNwKzgdxFz6MWO7An2yWsenC0IEpSdntL918eVixt4aKOl39mtftSK2vVBhL+tljLzTkk6KZvjFGmGxf4dZFeSlU7H8BQ+zQKfkyViDeKgewFqrRsc2JGCCKChr1paVampBE/lpb6hzUPLUUQbpFHKBlIwtmut7m+Ly0fylbO0lSO06Q3CoEPLYfc81dTnPvMKRO6SEgFPpQBepqhLzedRTw==</Modulus><Exponent>AQAB</Exponent><P>7zl5GNpKU89rcew0fnQUp/uMdPsb3+MAYkQgzXYxqV6FSLKuxIoU1E7UMXswGESHaVO10KO2LqubgjIJvmosFvFSeB2IvUnUax6/K6fMR+mddzAiOFPhH3DHGVHyJ9NVOLSSZxnXGEqZMLPxWxO0cfmW5yJPJxIUKnZpgy+IaKk=</P><Q>yd6RC+nJYq+0fKET12eaYQZNb/8uxywDIL5RFGS09coJfCsN+U4iM/RHFxghmVeAAdFxvF0uERdXfxnD8MyHHvQAEnjyXqKRJIKRVHHQ8VZ5Rq9nr3xGhZeIYk780J6fRS2VyJmgXYplPxbkCgW1P4rN/tBalS9hhny2gAvuTTc=</Q><DP>hOLjLvgLc9TztXvliR0IYGvukQjwagTaMLvxkNCIM7JKzaBcTtb5TRpg6v+oLsLaiZqzk6ttRy2Sm9cZ7Ilj5na1Pf4B+Ewr0DlrLl/urT/LderqB2oo0uM95gXMQ200mORNszH6dwbxY8mBV/txMCLaPZikaWq0gwX2BKaB2sk=</DP><DQ>HS6hhTlctXl0+/dFKQR/GruQgjo/hudj5F3e1rXgOw/j4yFOOdYDt8L+a+Y/JS2zAZBHgtVtjWb0bRlKbAsFFYJsaD83ulqB5OdDHxP9AoZfrco5kPLENxe6zYthnL7xg0ydtIwQ1LTnAgHLIW/FzdPBB68TCTH6RTjOISCYaG0=</DQ><InverseQ>1C8H6ll3tpSdjm2TrK7nYDVgMT6ckT2Sm0y/mN4kHW8j+4YkEv9pDbyUVypjBGMGD5FO/MuK/Pg+1MZaKkb2ExHnmCIzjmTRqKb+jz9WKFEMvUpOJ9zvlAsU0k6ElIlLoFO1aP8ULFBNDXqMbdENNIsGDCrkwGWHJcvxangy6Ew=</InverseQ><D>OlGt38UFRM3KjfB22dqiyLak3Jb+PeDt/NMBG1JONhM4gRrlhfbgmsznL3Fz/XlA9D9/yTtVRnSA+8J2UDe2fzvoJ1nHtzldWtIXnwE8cD1zImtIRck7BwAbYyJbfRV3iXqoxobRw8PX5KdL8Yc5ZmURmtTxSdnG+n/Mfr4WYpq03i0OA1Z8EIwJ41G1AIep1smQl1lQ6gZY4YqoGVyfFR7jlDcTOhr7A5aru9W006CMXFZzQOVimLIN5NjnSmI8wvbmwPhKGnP6FfKFQsOCApb8cOxJ2LC36czw0clEtMfzBiHRY33kcPm2QlEtxjcJBjVJzkK/4UVNYDSg4w0HMQ==</D></RSAKeyValue>";
        private const string AppId = "1";
        static void Main(string[] args)
        {
            //KeyConvert();

            using (var httpClient = new HttpClient())
            {
                Test1(httpClient);
                Test2(httpClient);
                Test3(httpClient);
            }
            Console.WriteLine("Hello World!");
        }
        static void Test1(HttpClient httpClient)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/test/test1");
            var timestamp = UtcTime.CurrentTimeMillis();
            var requestId = Guid.NewGuid().ToString("n");
            var requestBody = string.Empty;
            var encryptBody = RsaFunc.Encrypt(SysPublicKey, requestBody);
            var signBody = requestId + AppId + timestamp + encryptBody;
            var signature = RsaFunc.CreateSignature(AppPrivateKey, signBody);
            var rsaAuthorization = requestId + "." + AppId + "." + timestamp + "." + signature;

            requestMessage.Headers.Add("AuthSecurity-Authorization", rsaAuthorization);
            requestMessage.Content = new StringContent(encryptBody, Encoding.UTF8, "application/json");
            var response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("成功");
            }
            else
            {
                Console.WriteLine("错了");
            }
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var rsaSignature = response.Headers.Contains("AuthSecurity-Signature")
                ? response.Headers.FirstOrDefault(o => o.Key == "AuthSecurity-Signature").Value.FirstOrDefault() : null;
            if (rsaSignature == null)
            {

                Console.WriteLine($"响应结果:{responseBody}");
                return;
            }

            var responseSignBody = requestId + AppId + responseBody;
            if (!RsaFunc.ValidateSignature(SysPublicKey, responseSignBody, rsaSignature))
            {

                Console.WriteLine("非法响应");
                return;
            }

            var responseJson = RsaFunc.Decrypt(AppPrivateKey, responseBody);
            Console.WriteLine($"响应结果:{responseJson}");
        }

        static void Test2(HttpClient httpClient)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/test/test2");
            var timestamp = UtcTime.CurrentTimeMillis();
            var requestId = Guid.NewGuid().ToString("n");
            var requestBody=JsonConvert.SerializeObject(new { Id="123" });
            //var requestBody = JsonConvert.SerializeObject(new { Id1 = "123" });//测试模型校验
            var encryptBody = RsaFunc.Encrypt(SysPublicKey, requestBody);
            var signBody = requestId + AppId + timestamp + encryptBody;
            var signature = RsaFunc.CreateSignature(AppPrivateKey, signBody);
            var rsaAuthorization = requestId + "."+ AppId + "." + timestamp + "." + signature;

            requestMessage.Headers.Add("AuthSecurity-Authorization", rsaAuthorization);
            requestMessage.Content = new StringContent(encryptBody, Encoding.UTF8, "application/json");
            var response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
            else
            {
                Console.WriteLine("错了");
            }
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var rsaSignature = response.Headers.Contains("AuthSecurity-Signature")
                ? response.Headers.FirstOrDefault(o => o.Key == "AuthSecurity-Signature").Value.FirstOrDefault() : null;
            if (rsaSignature == null)
            {

                Console.WriteLine($"响应结果:{responseBody}");
                return;
            }

            var responseSignBody = requestId + AppId + responseBody;
            if (!RsaFunc.ValidateSignature(SysPublicKey, responseSignBody, rsaSignature))
            {

                Console.WriteLine("非法响应");
                return;
            }

            var responseJson = RsaFunc.Decrypt(AppPrivateKey, responseBody);
            Console.WriteLine($"响应结果:{responseJson}");
        }

        static void Test3(HttpClient httpClient)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/test/test3");
            var timestamp = UtcTime.CurrentTimeMillis();
            var requestId = Guid.NewGuid().ToString("n");
            var requestBody=JsonConvert.SerializeObject(new { Id="123" });
            var encryptBody = RsaFunc.Encrypt(SysPublicKey, requestBody);
            var signBody = requestId + AppId + timestamp + encryptBody;
            var signature = RsaFunc.CreateSignature(AppPrivateKey, signBody);
            var rsaAuthorization = requestId + "."+ AppId + "." + timestamp + "." + signature;

            requestMessage.Headers.Add("AuthSecurity-Authorization", rsaAuthorization);
            requestMessage.Content = new StringContent(encryptBody, Encoding.UTF8, "application/json");
            var response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
            else
            {
                Console.WriteLine("错了");
            }
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var rsaSignature = response.Headers.Contains("AuthSecurity-Signature")
                ? response.Headers.FirstOrDefault(o => o.Key == "AuthSecurity-Signature").Value.FirstOrDefault() : null;
            if (rsaSignature == null)
            {

                Console.WriteLine($"响应结果:{responseBody}");
                return;
            }

            var responseSignBody = requestId + AppId + responseBody;
            if (!RsaFunc.ValidateSignature(SysPublicKey, responseSignBody, rsaSignature))
            {

                Console.WriteLine("非法响应");
                return;
            }

            var responseJson = RsaFunc.Decrypt(AppPrivateKey, responseBody);
            Console.WriteLine($"响应结果:{responseJson}");
        }

        static void KeyConvert()
        {
            #region 系统公钥私钥
            var rsaPublicKeyJava2DotNet = RsaKeyConvert.RsaPublicKeyJava2DotNet(@"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx29/gr6KX2UwgDrXE44b
cg9/F1A4PcmINeBlIEbjR3Yv/hj/mNj1hlPedMrDpGK37A1xZ6W46eoXvDSY2c3m
bJwlbpevjCz/GEBshUkDHXRN/kzemr3rOGI+sL47RwBSaCKG1L1oZ0zD5hAuApx0
W5noKjGDPAVElW/OvjQ0vHiA1UDYazaqMXw8jBx4uXZR1tjJQ8+f6hvOE+pc7Qq3
wHmNkfCw+u8Llg+p+MtIfQ5kK6+R0/E4dJ8IjuftDKBCRkD2yDKo25tLIx0q/SN8
DFNvxUIPMoaHYUP7GAHbRTSYMvTVUm+MIAFLc4YetsxRhWw8TD9AMIM7jo87Vq4E
NQIDAQAB
-----END PUBLIC KEY-----
");
            var rsaPrivateKeyJava2DotNet = RsaKeyConvert.RsaPrivateKeyJava2DotNet(@"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDHb3+CvopfZTCA
OtcTjhtyD38XUDg9yYg14GUgRuNHdi/+GP+Y2PWGU950ysOkYrfsDXFnpbjp6he8
NJjZzeZsnCVul6+MLP8YQGyFSQMddE3+TN6aves4Yj6wvjtHAFJoIobUvWhnTMPm
EC4CnHRbmegqMYM8BUSVb86+NDS8eIDVQNhrNqoxfDyMHHi5dlHW2MlDz5/qG84T
6lztCrfAeY2R8LD67wuWD6n4y0h9DmQrr5HT8Th0nwiO5+0MoEJGQPbIMqjbm0sj
HSr9I3wMU2/FQg8yhodhQ/sYAdtFNJgy9NVSb4wgAUtzhh62zFGFbDxMP0AwgzuO
jztWrgQ1AgMBAAECggEAeLqVbRddepwt/SIhetQG50hcPkewlC7AxySc8yoWgfcY
3HBTojqMfxGcVAU+4+9iuYH1ybG3u9qbHbKVaRxMxijMPCOVAH1ou0ycpR7qxNTY
Nf5grzoS/6Hxo1q1VajrOeDU0EoMvqPo3weiYrBFfAIyWgB5GkdyGiiB9gBygDg8
cDZpTxVzAzZj9PDOrAYTQFXJDZUsU5e46kzBZ7T3VKLdWSNvpUHKQN8z+vBdNvYE
H+lJBZ00mAjzLHgU3BGqIWr9xJ1atPrAsgvnHWy4B0E+vwE/lo6esToGQPpfrYgT
+3Fc9hhbHJEwcYySCKC/BYgNz/8QT085w2gk5LIJrQKBgQDvyB4lxfaT+2ycjBhb
ITJ1iqRCt1U770Vdpt5OONWhbOwz6jxHqfOPA2yRgK5imfZKtQQZcjjjPLugLiD1
Du3vNjTWHvQSAd0TsslS39u222aY9IRwBsu64UveChunqyW8+tytk3CoRwi8iNd3
b+3eFKvMeegnV368PBSoTN0fMwKBgQDU7MalXncYiU4tR+oF7wLT8514erf555HF
rFGF8PvCvxeqP/wOxFDno5eLzgQhn+Mifc3Mn39KCmSVgYEBgFreeU3KIdvIXEDf
u/nIOBlb8mi9B1FwA+8pSptVLiivvYy7uEn3x6JEEkuUTM18w8uuoTYdCFMiEp/Q
ifQmEl9u9wKBgAeu7ehf84rCX7SUeKNS6P8DNkg8/6ov/JUHp8/x2kaV6uuu8ceK
FpfakDwJV40pSc8TBdKa6l+9rnVvhPbVR6KGpiLGUyPQk3jyHaleKvgtB5iXHTdJ
wHV4iqnOwRwaS9dGYpPdSZray9jeuajGGWUoXGjs7xdzokf91NufuuYpAoGAP7b2
nmeNJdd4gE5DtjDWXMxKc9HtGaf00/0DjBwXiigakcSBzX1rZFVstNYaIGGy2xBg
M/oKYgKIJMDS9LsO9gHAiFrnZKZOd2TNkt9On1gDhYUaFnXm7Ck6IBwm6qfC5C16
XN3tLYd3/FGbL9l7Kq9s/PUPw2NlVUbiuz3GNvMCgYEA7rqDUGKYiX7Mc6jxVQQ3
bgSeCewN2lQxsJW57mYptGUcFja1z2kVD6omiOmtvpn4AfBZT01kMXt7uHxzjsOG
pfhddQ8Ruf/Wxoawf7A6VBBBjrtVf+a9g0IDK2bmnC3iJOhIA1v2sRPU6dfkUje2
0f8k97ues4oE4is9f5zYXhk=
-----END PRIVATE KEY-----
");
            #endregion


            #region 客户端公钥私钥
            var rsaPublicKeyJava2DotNet1 = RsaKeyConvert.RsaPublicKeyJava2DotNet(@"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvKQbdjZ4b4foZOq4RkUf
Q4COkJ6htxW+S1fTZvTC8HbaaicTYWi9WICzeTd5PuAaDBwztk0HsS3r6ds1HSy/
/Wb7JsBE8ynrALaVPApNn54yQsTqEPmuGiTMYEBFIdyNwKzgdxFz6MWO7An2yWse
nC0IEpSdntL918eVixt4aKOl39mtftSK2vVBhL+tljLzTkk6KZvjFGmGxf4dZFeS
lU7H8BQ+zQKfkyViDeKgewFqrRsc2JGCCKChr1paVampBE/lpb6hzUPLUUQbpFHK
BlIwtmut7m+Ly0fylbO0lSO06Q3CoEPLYfc81dTnPvMKRO6SEgFPpQBepqhLzedR
TwIDAQAB
-----END PUBLIC KEY-----
");
            var rsaPrivateKeyJava2DotNet1 = RsaKeyConvert.RsaPrivateKeyJava2DotNet(@"-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC8pBt2Nnhvh+hk
6rhGRR9DgI6QnqG3Fb5LV9Nm9MLwdtpqJxNhaL1YgLN5N3k+4BoMHDO2TQexLevp
2zUdLL/9ZvsmwETzKesAtpU8Ck2fnjJCxOoQ+a4aJMxgQEUh3I3ArOB3EXPoxY7s
CfbJax6cLQgSlJ2e0v3Xx5WLG3hoo6Xf2a1+1Ira9UGEv62WMvNOSTopm+MUaYbF
/h1kV5KVTsfwFD7NAp+TJWIN4qB7AWqtGxzYkYIIoKGvWlpVqakET+WlvqHNQ8tR
RBukUcoGUjC2a63ub4vLR/KVs7SVI7TpDcKgQ8th9zzV1Oc+8wpE7pISAU+lAF6m
qEvN51FPAgMBAAECggEAOlGt38UFRM3KjfB22dqiyLak3Jb+PeDt/NMBG1JONhM4
gRrlhfbgmsznL3Fz/XlA9D9/yTtVRnSA+8J2UDe2fzvoJ1nHtzldWtIXnwE8cD1z
ImtIRck7BwAbYyJbfRV3iXqoxobRw8PX5KdL8Yc5ZmURmtTxSdnG+n/Mfr4WYpq0
3i0OA1Z8EIwJ41G1AIep1smQl1lQ6gZY4YqoGVyfFR7jlDcTOhr7A5aru9W006CM
XFZzQOVimLIN5NjnSmI8wvbmwPhKGnP6FfKFQsOCApb8cOxJ2LC36czw0clEtMfz
BiHRY33kcPm2QlEtxjcJBjVJzkK/4UVNYDSg4w0HMQKBgQDvOXkY2kpTz2tx7DR+
dBSn+4x0+xvf4wBiRCDNdjGpXoVIsq7EihTUTtQxezAYRIdpU7XQo7Yuq5uCMgm+
aiwW8VJ4HYi9SdRrHr8rp8xH6Z13MCI4U+EfcMcZUfIn01U4tJJnGdcYSpkws/Fb
E7Rx+ZbnIk8nEhQqdmmDL4hoqQKBgQDJ3pEL6clir7R8oRPXZ5phBk1v/y7HLAMg
vlEUZLT1ygl8Kw35TiIz9EcXGCGZV4AB0XG8XS4RF1d/GcPwzIce9AASePJeopEk
gpFUcdDxVnlGr2evfEaFl4hiTvzQnp9FLZXImaBdimU/FuQKBbU/is3+0FqVL2GG
fLaAC+5NNwKBgQCE4uMu+Atz1PO1e+WJHQhga+6RCPBqBNowu/GQ0IgzskrNoFxO
1vlNGmDq/6guwtqJmrOTq21HLZKb1xnsiWPmdrU9/gH4TCvQOWsuX+6tP8t16uoH
aijS4z3mBcxDbTSY5E2zMfp3BvFjyYFX+3EwIto9mKRparSDBfYEpoHayQKBgB0u
oYU5XLV5dPv3RSkEfxq7kII6P4bnY+Rd3ta14DsP4+MhTjnWA7fC/mvmPyUtswGQ
R4LVbY1m9G0ZSmwLBRWCbGg/N7pageTnQx8T/QKGX63KOZDyxDcXus2LYZy+8YNM
nbSMENS05wIByyFvxc3TwQevEwkx+kU4ziEgmGhtAoGBANQvB+pZd7aUnY5tk6yu
52A1YDE+nJE9kptMv5jeJB1vI/uGJBL/aQ28lFcqYwRjBg+RTvzLivz4PtTGWipG
9hMR55giM45k0aim/o8/VihRDL1KTifc75QLFNJOhJSJS6BTtWj/FCxQTQ16jG3R
DTSLBgwq5MBlhyXL8Wp4MuhM
-----END PRIVATE KEY-----

");
            #endregion
        }
    }
}
