using System.Security.Cryptography;
using System.Text;

namespace Hydra.Common.Globle
{
    public class EncryptionService
    {
        public static string Encipher(string Password)
        {
            string key = "abcdefghijklmnopqrstuvwxyz1234567890";
            byte[] bytesBuff = Encoding.Unicode.GetBytes(Password);
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new(key,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using MemoryStream mStream = new();
                using (CryptoStream cStream = new(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                Password = Convert.ToBase64String(mStream.ToArray());
            }
            return Password;
        }


        public static string Decipher(string Password)
        {
            string key = "abcdefghijklmnopqrstuvwxyz1234567890";
            Password = Password.Replace(" ", "+");
            byte[] bytesBuff = Convert.FromBase64String(Password);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new(key,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using MemoryStream mStream = new();
                using (CryptoStream cStream = new(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                Password = Encoding.Unicode.GetString(mStream.ToArray());
            }
            return Password;
        }
    }
}
