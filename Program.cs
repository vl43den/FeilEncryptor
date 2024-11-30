using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class FileEncryptor
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== File Encryption/Decryption Tool ===");
        Console.WriteLine("1. Encrypt a file");
        Console.WriteLine("2. Decrypt a file");
        Console.WriteLine("=======================================");
        Console.Write("Choose an option (1 or 2): ");
        
        string choice = Console.ReadLine()?.Trim();
        if (choice != "1" && choice != "2")
        {
            Console.WriteLine("Invalid choice! Please select 1 or 2.");
            return;
        }

        Console.Write("Enter the file path: ");
        string filePath = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Error: File does not exist or path is invalid.");
            return;
        }

        Console.Write("Enter a password: ");
        string password = ReadPassword();

        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Error: Password cannot be empty.");
            return;
        }

        if (choice == "1")
        {
            EncryptFile(filePath, password);
        }
        else
        {
            DecryptFile(filePath, password);
        }
    }

    static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true); // Read key without displaying it

            if (key.Key == ConsoleKey.Backspace)
            {
                // Remove last character if backspace is pressed
                if (password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Move cursor back, overwrite with space, and move back again
                }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                // End reading input when Enter is pressed
                Console.WriteLine();
                break;
            }
            else
            {
                // Append the character to the password string
                password.Append(key.KeyChar);
                Console.Write("*"); // Display asterisks instead of the actual character
            }
        } while (true);

        return password.ToString();
    }

    static void EncryptFile(string filePath, string password)
    {
        try
        {
            byte[] salt = GenerateRandomSalt();

            using Aes aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(password, salt, 100000);
            aes.Key = key.GetBytes(32); // AES-256 key
            aes.IV = key.GetBytes(16);  // AES block size

            string outputFilePath = filePath + ".enc";
            using var fileStream = new FileStream(outputFilePath, FileMode.Create);
            fileStream.Write(salt, 0, salt.Length); // Save the salt for decryption

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using var inputFileStream = new FileStream(filePath, FileMode.Open);

            inputFileStream.CopyTo(cryptoStream);
            Console.WriteLine($"File encrypted successfully: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during encryption: {ex.Message}");
        }
    }

    static void DecryptFile(string filePath, string password)
    {
        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Open);

            byte[] salt = new byte[16];
            if (fileStream.Read(salt, 0, salt.Length) != salt.Length)
            {
                throw new InvalidDataException("Failed to read the salt from the file.");
            }

            using Aes aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(password, salt, 100000);
            aes.Key = key.GetBytes(32); // AES-256 key
            aes.IV = key.GetBytes(16);  // AES block size

            string outputFilePath = filePath.Replace(".enc", "");
            using var cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var outputFileStream = new FileStream(outputFilePath, FileMode.Create);

            cryptoStream.CopyTo(outputFileStream);
            Console.WriteLine($"File decrypted successfully: {outputFilePath}");
        }
        catch (CryptographicException)
        {
            Console.WriteLine("Error: Incorrect password or file is corrupted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during decryption: {ex.Message}");
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
