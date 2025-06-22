// filepath: c:\Users\bob\Desktop\FeilEncryptor\src\Services\IEncryptionService.cs
using System.Threading.Tasks;

namespace FileEncryptor.Services
{
    public interface IEncryptionService
    {
        Task EncryptFileAsync(string filePath, string password);
        Task DecryptFileAsync(string filePath, string password);
    }
}