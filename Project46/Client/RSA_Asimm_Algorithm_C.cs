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

        //enkripcija secret keya prilikom slanja istog monitoringu.
        public string EncryptData(string pubKey, string secretKeyToEncrypt)
        {
            var sr = new System.IO.StringReader(pubKey);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            var publicKey = (RSAParameters)xs.Deserialize(sr);

            var csp = new RSACryptoServiceProvider();
            //pubKey.Modulus[0] = 1;
            csp.ImportParameters(publicKey);

            string secretKey = secretKeyToEncrypt;

            var bytesSecretKey = System.Text.Encoding.Unicode.GetBytes(secretKey);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCypherSecretKey = csp.Encrypt(bytesSecretKey, false);

            //we might want a string representation of our cypher text... base64 will do
            var encSecretKey = Convert.ToBase64String(bytesCypherSecretKey);      //string kriptovanog tajnog kljuca

            return encSecretKey;
        }
    }
}
