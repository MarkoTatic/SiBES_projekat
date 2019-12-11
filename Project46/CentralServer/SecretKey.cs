using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CentralServer
{
    public class SecretKey
    {
        public static string GenerateKey()
        {
            SymmetricAlgorithm algorithm = null;
            algorithm = AesCryptoServiceProvider.Create();
            return algorithm == null ? String.Empty : ASCIIEncoding.ASCII.GetString(algorithm.Key);
        }
    }
}
