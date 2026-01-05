using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class AES
{
    private const int IVSize = 16;
    
    public static void Encrypt(Stream outStream, string text, byte[] key)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        outStream.Write(aes.IV, 0, aes.IV.Length);

        using var encryptor = aes.CreateEncryptor();
        using var cryptoStream = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write);
    
        cryptoStream.Write(textBytes, 0, textBytes.Length);
        cryptoStream.FlushFinalBlock();
    }
    
    public static string Decrypt(Stream inStream, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[IVSize];
        inStream.Read(iv, 0, IVSize);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var cryptoStream = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read);
        
        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    public static byte[] GetHashedKey(string input)
    {
        using var sha256 = SHA256.Create();
        
        byte[] inputBytes = Encoding.UTF8.GetBytes(input); 
        return sha256.ComputeHash(inputBytes);
        
    }
}
