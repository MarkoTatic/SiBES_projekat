using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class RSA_Asimm_Algorithm_C
    {
        private RSAParameters privateKey;

        //generisemo privatni i javni rsa kljuc
        public string GenerateKeys()
        {
            var csp = new RSACryptoServiceProvider(2048);

            privateKey = csp.ExportParameters(true);   //generise private key(true),a public(false)
            var pubKey = csp.ExportParameters(false);
            string pubKeyString;
            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, pubKey);
            pubKeyString = sw.ToString();

            return pubKeyString;
        }

        //dekripcija tajnog kljuca uz pomoc privatnog rsa kljuca
        public string DecryptData(string encryptedData)
        {
            string decryptedData;
            var bytesCypherData = Convert.FromBase64String(encryptedData);

            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privateKey);

            var bytesPlainTextData = csp.Decrypt(bytesCypherData, false);
            decryptedData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            return decryptedData;
        }
    }
}
