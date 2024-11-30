using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("File Encryption/Decryption Tool");
        Console.WriteLine("1. Encrypt a file");
        Console.WriteLine("2. Decrypt a file");
        Console.Write("Choose an option (1 or 2): ");
        
        var choice = Console.ReadLine();
        
        Console.Write("Enter the file path: ");
        var filePath = Console.ReadLine();

        Console.Write("Enter the password: ");
        var password = Console.ReadLine();

        if (choice == "1")
        {
            EncryptFile(filePath, password);
        }
        else if (choice == "2")
        {
            DecryptFile(filePath, password);
        }
        else
        {
            Console.WriteLine("Invalid choice!");
        }
    }

    static void EncryptFile(string filePath, string password)
    {
        try
        {
            byte[] salt = GenerateRandomSalt();
            using var aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(password, salt, 100000);
            aes.Key = key.GetBytes(32); // AES-256
            aes.IV = key.GetBytes(16); // AES block size
            
            using var fileStream = new FileStream(filePath + ".enc", FileMode.Create);
            fileStream.Write(salt, 0, salt.Length); // Save salt for decryption
            
            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using var inputFile = new FileStream(filePath, FileMode.Open);

            inputFile.CopyTo(cryptoStream);
            Console.WriteLine("File encrypted successfully: " + filePath + ".enc");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during encryption: " + ex.Message);
        }
    }

    static void DecryptFile(string filePath, string password)
    {
        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Open);
            byte[] salt = new byte[16];
            fileStream.Read(salt, 0, salt.Length); // Read the salt
            
            using var aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(password, salt, 100000);
            aes.Key = key.GetBytes(32); // AES-256
            aes.IV = key.GetBytes(16); // AES block size
            
            using var cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var outputFile = new FileStream(filePath.Replace(".enc", ""), FileMode.Create);

            cryptoStream.CopyTo(outputFile);
            Console.WriteLine("File decrypted successfully: " + filePath.Replace(".enc", ""));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during decryption: " + ex.Message);
        }
    }

    static byte[] GenerateRandomSalt()
    {
        byte[] salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
}
