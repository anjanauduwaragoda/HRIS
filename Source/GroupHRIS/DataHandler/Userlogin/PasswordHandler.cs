using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataHandler.Userlogin
{
    public class PasswordHandler : TemplateDataHandler 
    {
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";


        public string Encrypt(string plainText)
        {

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string encryptedText)
        {
            //encryptedText = encryptedText.Replace(" ", "+");
            //encryptedText = encryptedText.Replace(" ", "+");
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText.Replace(" ", "+"));
            //byte[] cipherTextBytes = Convert.FromBase64String(encryptedText.Trim());
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public string EncodeBase64(string data)
        {
            string s = data.Trim().Replace(" ", "+");
            if (s.Length % 4 > 0)
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }

        public string GenerateNewPassword()
        {
            string sNewpassword = "";
            Random rnd = new Random();
            for (int i = 0; i < CommonVariables.PASSWORD_LENGTH; i++)
            {
                int iRandom = rnd.Next(0, CommonVariables.PASSWORD_CHARS.Length - 1);
                sNewpassword += CommonVariables.PASSWORD_CHARS.Substring(iRandom, 1);
            }

            return sNewpassword;
        }

    }
}
