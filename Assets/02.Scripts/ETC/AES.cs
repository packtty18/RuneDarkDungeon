using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class AES
{
    private const int IVSize = 16;

    public static string Encrypt(string text, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
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
    
    public static void EncryptToStream(Stream outStream, byte[] data, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        outStream.Write(aes.IV, 0, aes.IV.Length);

        using var encryptor = aes.CreateEncryptor();
        using var cryptoStream = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write);
    
        cryptoStream.Write(data, 0, data.Length);
    }

    public static string Decrypt(string encryptedText, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;

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
    
    public static byte[] DecryptFromStream(Stream inStream, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[IVSize];
        if (inStream.Read(iv, 0, IVSize) < IVSize) return null;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var cryptoStream = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read);
        
        using var memoryStream = new MemoryStream();
        cryptoStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public static byte[] GetHashKey(string input)
    {
        if (string.IsNullOrEmpty(input)) return new byte[32];

        using var sha256 = SHA256.Create();
        
        byte[] inputBytes = Encoding.UTF8.GetBytes(input); 
        return sha256.ComputeHash(inputBytes);
        
    }
}
