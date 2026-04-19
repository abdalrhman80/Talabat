using Microsoft.AspNetCore.Http;
using Talabat.Domain.Services;

namespace Talabat.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string rootPath, string folderPath)
        {
            var uploadFolderPath = Path.Combine(rootPath, folderPath);

            Directory.CreateDirectory(uploadFolderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine(folderPath, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string rootPath, string filePath)
        {
            var fullPath = Path.Combine(rootPath, filePath.TrimStart('\\', '/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

    }
}
