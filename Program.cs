using System;
using System.IO;
using System.Threading.Tasks;
using FileEncryptor.Services;
using FileEncryptor.Utils;

namespace FileEncryptor
{
    internal class Program
    {
        private static readonly IEncryptionService encryptionService = new AesEncryptionService();

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  FileEncryptor.exe encrypt <file-path>");
            Console.WriteLine("  FileEncryptor.exe decrypt <file-path>");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Welcome to FileEncryptor!");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Encrypt a file");
            Console.WriteLine("2. Decrypt a file");
            Console.Write("Your choice: ");
        }

        private static async Task<int> RunWithArgs(string[] args)
        {
            var cmd = args[0].ToLower();
            var filePath = args[1];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return 1;
            }
            var password = PasswordHelper.ReadPassword();
            if (!PasswordHelper.IsPasswordStrong(password))
            {
                Console.WriteLine("Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");
                return 1;
            }
            try
            {
                if (cmd == "encrypt")
                {
                    await encryptionService.EncryptFileAsync(filePath, password);
                    Console.WriteLine("File encrypted successfully.");
                }
                else if (cmd == "decrypt")
                {
                    await encryptionService.DecryptFileAsync(filePath, password);
                    Console.WriteLine("File decrypted successfully.");
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                    PrintUsage();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }
            return 0;
        }

        private static async Task<int> RunInteractive()
        {
            PrintMenu();
            var choice = Console.ReadLine();
            Console.Write("Enter the path: ");
            var path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exist.");
                return 1;
            }
            Console.Write("Enter a password: ");
            var pwd = PasswordHelper.ReadPassword();
            if (!PasswordHelper.IsPasswordStrong(pwd))
            {
                Console.WriteLine("Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");
                return 1;
            }
            switch (choice)
            {
                case "1":
                    try
                    {
                        await encryptionService.EncryptFileAsync(path, pwd);
                        Console.WriteLine("File encrypted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error encrypting file: {ex.Message}");
                        return 1;
                    }
                    break;
                case "2":
                    try
                    {
                        await encryptionService.DecryptFileAsync(path, pwd);
                        Console.WriteLine("File decrypted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error decrypting file: {ex.Message}");
                        return 1;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return 1;
            }
            return 0;
        }

        private static async Task<int> Main(string[] args)
        {
            if (args.Length == 2)
            {
                return await RunWithArgs(args);
            }
            else
            {
                return await RunInteractive();
            }
        }
    }
}
