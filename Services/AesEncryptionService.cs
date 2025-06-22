using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FileEncryptor.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int NonceSize = 12;
        private const int TagSize = 16;
        private const int Iterations = 100_000;

        private byte[] DeriveKey(string password, byte[] salt)
        {
            using var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return kdf.GetBytes(KeySize);
        }

        public async Task EncryptFileAsync(string filePath, string password)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            var key = DeriveKey(password, salt);
            var nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            var plaintext = await File.ReadAllBytesAsync(filePath);
            var ciphertext = new byte[plaintext.Length];
            var tag = new byte[TagSize];

            using var aesgcm = new AesGcm(key);
            aesgcm.Encrypt(nonce, plaintext, ciphertext, tag);

            using var outFile = new FileStream(filePath + ".enc", FileMode.Create);
            await outFile.WriteAsync(salt);
            await outFile.WriteAsync(nonce);
            await outFile.WriteAsync(tag);
            await outFile.WriteAsync(ciphertext);
        }

        public async Task DecryptFileAsync(string filePath, string password)
        {
            using var inFile = new FileStream(filePath, FileMode.Open);
            var salt = new byte[SaltSize];
            await inFile.ReadAsync(salt);
            var nonce = new byte[NonceSize];
            await inFile.ReadAsync(nonce);
            var tag = new byte[TagSize];
            await inFile.ReadAsync(tag);

            var ciphertext = new byte[inFile.Length - SaltSize - NonceSize - TagSize];
            await inFile.ReadAsync(ciphertext);

            var key = DeriveKey(password, salt);
            var plaintext = new byte[ciphertext.Length];

            using var aesgcm = new AesGcm(key);
            aesgcm.Decrypt(nonce, ciphertext, tag, plaintext);

            var outputPath = filePath.EndsWith(".enc") ? filePath[..^4] : filePath + ".dec";
            await File.WriteAllBytesAsync(outputPath, plaintext);
        }
    }
}