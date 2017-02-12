using System;
using System.IO;
using System.Security.Cryptography;

namespace Common.Methods
{
    public static class Encryption
    {
        public static string Encrypt(string key, string value) => Security.Encrypt(value, key, 128);
        
        public static string Decrypt(string key, string value) => Security.Decrypt(value, key, 128);
    }

    static class Security
    {
        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {

            // Create a MemoryStream that is going to accept the encrypted bytes 
            var ms = new MemoryStream();

            var alg = Rijndael.Create();
            alg.Padding = PaddingMode.ISO10126;
            alg.Key = Key;

            alg.IV = IV;
            var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            var encryptedData = ms.ToArray();
            return encryptedData;
        }
        /// <summary>
        /// Returns an encrypted string using Rijndael (128,192,256 Bits).
        /// </summary>
        public static string Encrypt(string Data, string Password, int Bits)
        {


            var clearBytes = System.Text.Encoding.Unicode.GetBytes(Data);


            var pdb = new PasswordDeriveBytes(Password,


                new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });


            if (Bits == 128)
            {
                var encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 192)
            {
                var encryptedData = Encrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 256)
            {
                var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else
            {
                return string.Concat(Bits);
            }
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {

            var ms = new MemoryStream();
            var alg = Rijndael.Create();
            alg.Padding = PaddingMode.ISO10126;
            alg.Key = Key;
            alg.IV = IV;
            var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            var decryptedData = ms.ToArray();
            return decryptedData;
        }


        /// <summary>
        /// Returns a decrypted string.
        /// </summary>
        // Decrypt a string into a string using a password 
        public static string Decrypt(string Data, string Password, int Bits)
        {

            var cipherBytes = Convert.FromBase64String(Data);

            var pdb = new PasswordDeriveBytes(Password,

                new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

            if (Bits == 128)
            {
                var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else if (Bits == 192)
            {
                var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else if (Bits == 256)
            {
                var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else
            {
                return string.Concat(Bits);
            }

        }
    }
}
