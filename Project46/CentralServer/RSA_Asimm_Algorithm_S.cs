using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class RSA_Asimm_Algorithm_S
    {
        private RSAParameters publicKey;

        public string Encryption(string pubKey)
        {
            var sr = new System.IO.StringReader(pubKey);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            publicKey = (RSAParameters)xs.Deserialize(sr);

            var csp = new RSACryptoServiceProvider();
            //pubKey.Modulus[0] = 1;
            csp.ImportParameters(publicKey);

            string secretKey = SecretKey.GenerateKey();

            var bytesSecretKey = System.Text.Encoding.Unicode.GetBytes(secretKey);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCypherSecretKey = csp.Encrypt(bytesSecretKey, false);

            //we might want a string representation of our cypher text... base64 will do
            var encSecretKey = Convert.ToBase64String(bytesCypherSecretKey);      //string kriptovanog tajnog kljuca

            return encSecretKey;
        }
    }
}
