using GymManagementBL.Service.Interface.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtenstions = { ".jpg", ".jpeg", ".png" };

        private readonly long FileMaxSize = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _webHost;

        // IWebHostEnvironment to get the wwwroot path on different devices or environments
        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }


        public string? Upload(string FolderName, IFormFile file)
        {
            try
            {
                // THE 8 KNOWN STEPS OF FILE UPLOAD


                if (FolderName is null || file is null || file.Length == 0)
                {
                    return null;
                }
                if (file.Length > FileMaxSize)
                {
                    return null;
                }
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedExtenstions.Contains(extension.ToLower())) return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", FolderName); // FolderName is like trainer or member inside images folder

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension; // to avoid duplicate file names
                var filePath = Path.Combine(folderPath, fileName); //wwwroot/images/Mambers/234r243f34r34efd34efd3ed.png
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                return fileName;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to upload file to folder {FolderName} : {ex.Message}");
                return null;
            }

        }

        public bool Delete(string FolderName, string fileName)
        {
            try
            {
                if(string.IsNullOrEmpty(FolderName) || string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
                var fullPath = Path.Combine(_webHost.WebRootPath, "images", FolderName, fileName);

                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to delete file with name {fileName} : {ex.Message}");
                return false;
            }
        }


    }
}
