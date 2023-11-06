using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.DAL.Utilities
{
    public static class Security
    {
        public static string EncryptionKey = "IMS_Security_Key";
        public static string Encrypt(this long longToEncrypt)
        {
            return Encrypt(longToEncrypt.ToString());
        }
        public static string Encrypt(this int intToEncrypt)
        {
            return Encrypt(intToEncrypt.ToString());
        }
        public static string Encrypt(this string textToEncrypt)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(EncryptionKey));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(textToEncrypt);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results).Replace('+', '_').Replace('=', '@').Replace('/', '$');
        }
        public static string Decrypt(this string cipherString)
        {
            try
            {
                if (cipherString != null) { 
                cipherString = cipherString.Replace(' ', '+').Replace('_', '+').Replace('@', '=').Replace('$', '/');
				}
                else
                {
					return cipherString;
				}
				byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(EncryptionKey));
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] DataToDecrypt = Convert.FromBase64String(cipherString);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                catch (Exception ex)
                {
                    return cipherString;
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                return UTF8.GetString(Results);
            }
            catch (Exception e)
            {
                return cipherString;
            }

        }
        public static string GetCheckSum(string data, string CheckSumKey)
        {
            byte[] key = Encoding.ASCII.GetBytes(CheckSumKey); //
            HMACSHA256 myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            return result;
        }
        public static byte[] GenerateHash256(string s)
        {
            byte[] hashValue = null;

            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
               hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

                //// Convert the byte array to string format
                //foreach (byte b in hashValue)
                //{
                //    hash += $"{b:X2}";
                //}
            }

            return hashValue;
        }

    }
}
