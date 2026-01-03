using System;
using System.Security.Cryptography;
using System.Text;

public static class AES
{
    private const int IVSize = 16;

    public static string Encrypt(string text, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GetHashKey(key);
            aes.GenerateIV();
            byte[] iv = aes.IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);

            byte[] result = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }
    }

    public static string Decrypt(string encryptedText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GetHashKey(key);

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            byte[] iv = new byte[IVSize];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            int ciphertextLength = encryptedBytes.Length - iv.Length;
            byte[] ciphertext = new byte[ciphertextLength];
            Buffer.BlockCopy(encryptedBytes, iv.Length, ciphertext, 0, ciphertextLength);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
    
    public static string GetHash(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }

    public static byte[] GetHashKey(string input)
    {
        if (string.IsNullOrEmpty(input)) return new byte[32];

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            return sha256.ComputeHash(inputBytes);
        }
    }
}
