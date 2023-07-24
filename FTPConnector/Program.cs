// See https://aka.ms/new-console-template for more information
using FTPConnector;
using Renci.SshNet;
using System.IO;
using System.Net;

Console.WriteLine("Starting FTP Connector");

try
{
    var username = "stavrianosy";
    var password = "Pass@word2023";
    var ftpURL = "ftp://localhost";
    var ftpFileURL = "ftp://localhost/ftpTest.txt";
    var ftpFileUpload = "C:\\dev\\Files\\test.txt";
    var ftpFileDownload = "C:\\dev\\FilesDownloaded\\encrypted.pgp";
    var publikKeyPath = @"C:\dev\Keys\public.asc";
    var privateKeyPath = @"C:\dev\Keys\private.asc";
    var encryptedFilePath = @"C:\dev\Files\encrypted.pgp";
    var decryptedFilePath = @"C:\dev\FilesDownloaded\decryptedFile.txt";

    var credentials = new NetworkCredential(username, password);

    var pgp = new PgpHelper();
    await pgp.GenerateKeyAsync(publikKeyPath, privateKeyPath, username, password);
    Console.WriteLine($"PGP Keys Created - Public key: {publikKeyPath}, Private key: {privateKeyPath}");

    await pgp.EncryptFileAsync(publikKeyPath, ftpFileUpload, encryptedFilePath);
    Console.WriteLine($"Encrypted file Created - Original file: {ftpFileUpload}, Encrypted file: {encryptedFilePath}");

    var ftp = new FtpHelper(ftpURL, username, password);
    await ftp.UploadFileAsync(encryptedFilePath);
    Console.WriteLine($"Encrypted file uploaded to ftp - Encrypted file: {encryptedFilePath}");
    
    await ftp.DownloadFileAsync(ftpFileDownload);
    Console.WriteLine($"Encrypted file downloaded from ftp - Downloaded file: {encryptedFilePath}");

    await pgp.DecryptFileAsync(privateKeyPath, ftpFileDownload, decryptedFilePath, password);
    Console.WriteLine($"Decrypt file - Decrypted/destination file: {decryptedFilePath}");

}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

