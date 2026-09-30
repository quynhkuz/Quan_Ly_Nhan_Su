using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            if (file is null || file.Length == 0)
                return null;

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            Directory.CreateDirectory(folderPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            string filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile (string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);

            if (File.Exists(filePath)) 
                 File.Delete(filePath);
            
        }
    }
}
