# **FileEncryptor**

**FileEncryptor** is a C# console application that allows users to securely encrypt and decrypt files using AES-256 encryption. This tool helps protect sensitive data by ensuring that only authorized users can access the encrypted content.

---

## **Features**
- Encrypt files using AES-256 encryption.
- Decrypt files encrypted with AES-256.
- User-friendly console interface.
- Password-protected encryption for added security.
- Simple and intuitive command-line usage.

---

## **How It Works**
1. Run the program and select either encryption or decryption.
2. Enter the path to the file to be encrypted or decrypted.
3. Enter a secure password (masked during input).
4. Encrypted files are saved with a `.enc` extension.
5. Decrypted files are saved with their original file name.

---

## **Usage Instructions**

### **Encrypting a File**
1. Choose the `1. Encrypt a file` option.
2. Enter the file path of the file you wish to encrypt.
3. Enter a password (masked for privacy) to protect the file.
4. The program will create an encrypted file with the `.enc` extension.

### **Decrypting a File**
1. Choose the `2. Decrypt a file` option.
2. Enter the path of the encrypted file.
3. Enter the correct password to decrypt the file.
4. The program will restore the original file, overwriting the `.enc` file.

---

## **System Requirements**
- **Operating System**: Windows, macOS, or Linux
- **.NET Runtime**: .NET 6.0 or higher
- **Permissions**: Read and write permissions for the target file(s)

---

## **Installation Steps**
1. Clone or download this repository:
    ```bash
    git clone https://github.com/YourUsername/FileEncryptor.git
    ```
2. Build the project using your preferred IDE (e.g., Visual Studio) or the .NET CLI:
    ```bash
    dotnet build
    ```
3. Run the application:
    ```bash
    dotnet run
    ```

---

## **Security Tips**
- **Password Security**: Use a strong password for encryption. Without the correct password, encrypted files cannot be decrypted.
- **Backup Your Files**: Always create backups of original files before encryption to prevent accidental data loss.

---

## **Contributing**
Feel free to fork this repository, submit pull requests, or report issues.

---

## **License**
This project is licensed under the MIT License. See the `LICENSE` file for more details.
