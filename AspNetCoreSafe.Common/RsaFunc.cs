using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCoreSafe.Common
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/10/10 20:12:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class RsaFunc
    {
        private RsaFunc()
        {
        }
        /// <summary>
        /// RSA的加密函数  string
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string Encrypt(string publicKey, string plaintext)
        {
            return Encrypt(publicKey, Encoding.UTF8.GetBytes(plaintext));
        }
        /// <summary>
        /// RSA的加密函数  string
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="plainbytes">明文字节数组</param>
        /// <returns></returns>
        public static string Encrypt(string publicKey, byte[] plainbytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                var bufferSize = (rsa.KeySize / 8 - 11);
                byte[] buffer = new byte[bufferSize]; //待加密块
                using (MemoryStream msInput = new MemoryStream(plainbytes))
                {
                    using (MemoryStream msOutput = new MemoryStream())
                    {
                        int readLen;
                        while ((readLen = msInput.Read(buffer, 0, bufferSize)) > 0)
                        {
                            byte[] dataToEnc = new byte[readLen];
                            Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                            byte[] encData = rsa.Encrypt(dataToEnc, false);
                            msOutput.Write(encData, 0, encData.Length);
                        }
                        byte[] result = msOutput.ToArray();
                        rsa.Clear();
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        /// <summary>
        /// RSA的解密函数  stirng
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="ciphertext">密文字符串</param>
        /// <returns></returns>
        public static string Decrypt(string privateKey, string ciphertext)
        {
            return Decrypt(privateKey, Convert.FromBase64String(ciphertext));
        }
        /// <summary>
        /// RSA的解密函数  byte
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="cipherbytes">密文字节数组</param>
        /// <returns></returns>
        public static string Decrypt(string privateKey, byte[] cipherbytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                int keySize = rsa.KeySize / 8;
                if (cipherbytes.Length <= keySize)
                    return Encoding.UTF8.GetString(rsa.Decrypt(cipherbytes, false));
                byte[] buffer = new byte[keySize];
                using (MemoryStream msInput = new MemoryStream(cipherbytes))
                {
                    using (MemoryStream msOutput = new MemoryStream())
                    {
                        int readLen;
                        while ((readLen = msInput.Read(buffer, 0, keySize)) > 0)
                        {
                            byte[] dataToDec = new byte[readLen];
                            Array.Copy(buffer, 0, dataToDec, 0, readLen);
                            byte[] decData = rsa.Decrypt(dataToDec, false);
                            msOutput.Write(decData, 0, decData.Length);
                        }
                        byte[] result = msOutput.ToArray();
                        rsa.Clear();
                        return Encoding.UTF8.GetString(result);
                    }
                }
            }
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string CreateSignature(string privateKey, string encrypt)
        {
            using (RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(encrypt);
                byte[] signatureBytes = rsa.SignData(dataBytes, "SHA256");
                return Convert.ToBase64String(signatureBytes);
            }
        }
        /// <summary>
        /// 校验签名
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="encrypt"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool ValidateSignature(string publicKey, string encrypt, string signature)
        {
            using (RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                RSAPKCS1SignatureDeformatter rsaDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rsa);
                //指定解密的时候HASH算法为SHA256
                rsaDeformatter.SetHashAlgorithm("SHA256");

                byte[] deformatterData = Convert.FromBase64String(signature);
                using (HashAlgorithm sha256 = HashAlgorithm.Create("SHA256"))
                {
                    byte[] hashData = sha256.ComputeHash(Encoding.UTF8.GetBytes(encrypt));
                    return rsaDeformatter.VerifySignature(hashData, deformatterData);
                }
            }
        }
    }
}
