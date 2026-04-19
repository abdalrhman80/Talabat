using Microsoft.AspNetCore.Http;

namespace Talabat.Domain.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string rootPath, string folderPath);
        void DeleteFile(string rootPath, string filePath);
    }
}
