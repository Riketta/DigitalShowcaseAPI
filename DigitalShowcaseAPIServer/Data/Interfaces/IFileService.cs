using DigitalShowcaseAPIServer.DTOs;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface IFileService
    {
        public string GenerateRandomFileName();
        public string GetStaticContentPath();
        public Models.File.FileType GetAllowedContentType(string contentType);
        public Task<string?> UploadFileAsync(FileTransferObject fileTransferObject);
        public Task DeleteFileByNameAsync(string filename);
        public Task UploadFilesAsyncAsync(List<IFormFile> formFiles);
        public Task<Models.File?> DownloadFileByNameAsync(string filename);
    }
}
