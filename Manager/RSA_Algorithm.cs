using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class RSA_Algorithm
    {
        public static string EncryptRsa(string input, X509Certificate2 cert)
        {
            string output = string.Empty;
            //X509Certificate2 cert = getCertificate(certificateName);
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
                byte[] bytesData = Encoding.UTF8.GetBytes(input);
                byte[] bytesEncrypted = csp.Encrypt(bytesData, false);
                output = Convert.ToBase64String(bytesEncrypted);

            return output;
        }

        public static string DecryptRsa(string encrypted, X509Certificate2 cert)
        {
            string text = string.Empty;
            //X509Certificate2 cert = getCertificate(certificateName);
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;
            
                byte[] bytesEncrypted = Convert.FromBase64String(encrypted);
                byte[] bytesDecrypted = csp.Decrypt(bytesEncrypted, false);
                text = Encoding.UTF8.GetString(bytesDecrypted);
            
            return text;
        }
    }
}
