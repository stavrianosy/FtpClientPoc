using PgpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FTPConnector
{
    public class PgpHelper
    {
        public async Task GenerateKeyAsync(string publicKeyPath, string privateKeyPath, string username = "", string password = "") {
            using (PGP pgp = new PGP())
            {
                // Generate keys
                await pgp.GenerateKeyAsync(publicKeyPath, privateKeyPath, username, password);
            }
        }

        public async Task EncryptFileAsync(string publicKeyPath, string inputFilePath, string encryptedFilePath) {
            // Load keys
            FileInfo publicKey = new FileInfo(publicKeyPath);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input/output files
            FileInfo inputFile = new FileInfo(inputFilePath);
            FileInfo encryptedFile = new FileInfo(encryptedFilePath);

            // Encrypt
            PGP pgp = new PGP(encryptionKeys);
            await pgp.EncryptFileAsync(inputFile, encryptedFile);
        }

        public async Task DecryptFileAsync(string privateKeyPath, string inputFilePath, string decryptedFilePath, string password = "") {
            // Load keys
            FileInfo privateKey = new FileInfo(privateKeyPath);
            EncryptionKeys encryptionKeys = new EncryptionKeys(privateKey, password);

            // Reference input/output files
            FileInfo inputFile = new FileInfo(inputFilePath);
            FileInfo decryptedFile = new FileInfo(decryptedFilePath);

            // Decrypt
            PGP pgp = new PGP(encryptionKeys);
            await pgp.DecryptFileAsync(inputFile, decryptedFile);
        }
    }
}
