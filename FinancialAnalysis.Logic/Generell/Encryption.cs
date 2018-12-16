﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FinancialAnalysis.Logic
{
    public static class Encryption
    {
        private const int SaltLength = 8;

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = {8, 13, 4, 53, 16, 71, 25, 94};

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = {8, 13, 4, 53, 16, 71, 25, 94};

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            var result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(input);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            var result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        public static string EncryptStringWithSalt(string text, string password)
        {
            var baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            var baPwdHash = SHA256.Create().ComputeHash(baPwd);

            var baText = Encoding.UTF8.GetBytes(text);

            var baSalt = GetRandomBytes();
            var baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (var i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (var i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

            var result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string DecryptStringWithSalt(string text, string password)
        {
            var baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            var baPwdHash = SHA256.Create().ComputeHash(baPwd);

            var baText = Convert.FromBase64String(text);

            var baDecrypted = AES_Decrypt(baText, baPwdHash);

            // Remove salt
            var baResult = new byte[baDecrypted.Length - SaltLength];
            for (var i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + SaltLength];

            var result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        public static byte[] GetRandomBytes()
        {
            var ba = new byte[SaltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }

        public static string ComputeHash(string input, HashAlgorithm algorithm, byte[] salt)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Combine salt and input bytes
            Byte[] saltedInput = new Byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);

            Byte[] hashedBytes = algorithm.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }
    }
}