# FileEncryptor

A console application for encrypting and decrypting files using AES-GCM with password-based key derivation.

## Features

- AES-256 GCM authenticated encryption
- Password-based key derivation using PBKDF2 (100,000 iterations)
- Salt, nonce, and tag stored with ciphertext
- Async file operations for responsiveness

## Usage

```bash
# Encrypt
FileEncryptor.exe encrypt <file-path>

# Decrypt
FileEncryptor.exe decrypt <file-path>
```

Alternatively, run without arguments and follow the interactive prompts.
