using System;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    public static void Main()
    {
        string originalData = "Hello, world!";
        byte[] encryptedData;
        byte[] decryptedData;
        string DataSignature;
        bool isVerified;
        // Создание экземпляра класса RSA для шифрования и дешифрования данных
        using (RSA rsa = RSA.Create())
        {
            // Генерация пары ключей
            RSAParameters publicKey = rsa.ExportParameters(false);
            RSAParameters privateKey = rsa.ExportParameters(true);

            // Шифрование данных
            encryptedData = EncryptData(Encoding.UTF8.GetBytes(originalData), publicKey);

            // Дешифрование данных
            decryptedData = DecryptData(encryptedData, privateKey);
            // Подписание данных
            byte[] signature = SignData(Encoding.UTF8.GetBytes(originalData), privateKey);

            // Верификация данных
            isVerified = VerifyData(Encoding.UTF8.GetBytes(originalData), signature, publicKey);
            DataSignature = Convert.ToBase64String(signature);
            
           
        }

        Console.WriteLine("Original Data: " + originalData);
        Console.WriteLine();
        Console.WriteLine("Encrypted Data: " + Convert.ToBase64String(encryptedData));
        Console.WriteLine();
        Console.WriteLine("Decrypted Data: " + Encoding.UTF8.GetString(decryptedData));
        Console.WriteLine();
        Console.WriteLine("Data Signature: " + DataSignature);
        Console.WriteLine();
        Console.WriteLine("Data Verified: " + isVerified);

    }

    public static byte[] EncryptData(byte[] data, RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        }
    }

    public static byte[] DecryptData(byte[] encryptedData, RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(privateKey);
            return rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
        }
    }

    public static byte[] SignData(byte[] data, RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }

    public static bool VerifyData(byte[] data, byte[] signature, RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}