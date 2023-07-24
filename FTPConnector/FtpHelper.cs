using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPConnector
{
    public class FtpHelper
    {
        private string _host;
        public NetworkCredential credential;
        public FtpHelper()
        {
        }

        public FtpHelper(string host, string username, string password)
        {
            _host  = host;
            credential = new NetworkCredential(username, password);
        }

        public AsyncFtpClient CreateFtpClient() { 
            return new AsyncFtpClient(_host, credential);
        }
        public async Task UploadFileAsync(string fileToUpload)
        {
            using (AsyncFtpClient ftp = CreateFtpClient())
            {
                if (ftp != null && !ftp.IsConnected) { await ftp.Connect(); }
                using (FileStream fs = File.OpenRead(fileToUpload))
                {
                    await ftp.UploadStream(fs, Path.GetFileName(fileToUpload));
                }
            }
        }
        public async Task DownloadFileAsync(string fileToDownload)
        {
            using (AsyncFtpClient ftp = CreateFtpClient())
            {
                if (ftp != null && !ftp.IsConnected) { await ftp.Connect(); }
                await ftp.DownloadFile(fileToDownload, Path.GetFileName(fileToDownload), FtpLocalExists.Overwrite);
            }
        }
    }
}
