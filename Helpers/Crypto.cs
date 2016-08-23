using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Crypto
    {
        private static HashAlgorithm Hash;
        public static string GetSHA256(string toHash)
        {
            Hash = new SHA256Managed();
            return DoHash(toHash);
        }
        public static string GetSHA1(string toHash)
        {
            Hash = new SHA1Managed();
            return DoHash(toHash);
        }
        public static string DoHash(string toHash )
        {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(toHash);
            byte[] hash = Hash.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}
