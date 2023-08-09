using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using DigitalShowcaseAPIServer.DTOs;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using File = System.IO.File;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class FileService : IFileService
    {
        private readonly string ContentRoot;
        private readonly string StaticFolder;
        private readonly string UploadsFolder;
        private readonly string FileStorageFullPath;
        private readonly string FileStorageRelativePath;

        private readonly DigitalShowcaseContext _db;
        private readonly IHttpContextAccessor _accessor;

        public FileService(IOptions<FileServiceOptions> options, DigitalShowcaseContext db, IHttpContextAccessor accessor)
        {
            _db = db;
            _accessor = accessor;

            ContentRoot = options.Value.ContentRoot;
            StaticFolder = options.Value.StaticFolder;
            UploadsFolder = options.Value.UploadsFolder ?? "/UploadsStorage";
            FileStorageFullPath = Path.Combine(ContentRoot, StaticFolder, UploadsFolder);
            FileStorageRelativePath = Path.Combine(StaticFolder, UploadsFolder).Replace('\\', '/');

            if (!Directory.Exists(FileStorageFullPath))
                Directory.CreateDirectory(FileStorageFullPath);
        }

        public string GenerateRandomFileName()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string GetStaticContentPath()
        {
            return FileStorageFullPath;
        }

        /// <summary>
        /// Get white-listed content type if passed content type allowed to upload
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns><see cref="Models.File.FileType"/> if valid content type, <see cref="Models.File.FileType.Unknown"/> otherwise</returns>
        public Models.File.FileType GetAllowedContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return Models.File.FileType.JPEG;
                case "image/png":
                    return Models.File.FileType.PNG;
            }

            return Models.File.FileType.Unknown;
        }

        public async Task DeleteFileByNameAsync(string filename)
        {
            Models.File? file = await _db.Files.Where(x => x.Name == filename).SingleOrDefaultAsync();

            if (file is null)
                return;

            var path = Path.Combine(FileStorageFullPath, file.Name);
            if (File.Exists(path))
                File.Delete(path);

            _db.Files.Remove(file);
            await _db.SaveChangesAsync();
        }

        public async Task<string?> UploadFileAsync(FileTransferObject fileTransferObject) // TODO: validate file hash to prevent duplicates
        {
            //string mimeContentType = GetMimeTypeForFileExtension(fileTransferObject.File.FileName);
            Models.File.FileType fileType = GetAllowedContentType(fileTransferObject.File.ContentType);
            if (fileType == Models.File.FileType.Unknown)
                return null;

            Models.File file = new Models.File()
            {
                UploadedByUserId = Convert.ToInt32(_accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Name = GenerateRandomFileName() + Path.GetExtension(fileTransferObject.File.FileName),
                Type = fileType,
            };
            
            using (var stream = new MemoryStream())
            {
                fileTransferObject.File.CopyTo(stream);
                file.Data = stream.ToArray();
            }

            if (file.Data.Length == 0)
                return null;

            await _db.Files.AddAsync(file);
            await _db.SaveChangesAsync();
            await SaveStreamAsFile(file);

            return string.Format($"/{FileStorageRelativePath}/{file.Name}"); // .Trim('/')
        }

        public Task UploadFilesAsyncAsync(List<IFormFile> formFiles)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get file by name stored in database, file saved to dedicated public static files folder
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<Models.File?> DownloadFileByNameAsync(string filename)
        {
            Models.File? file = await _db.Files.Where(x => x.Name == filename).SingleOrDefaultAsync();

            if (file is null || file.Data is null)
                return null;

            await SaveStreamAsFile(file);
            return file;
        }

        public async Task SaveStreamAsFile(Models.File file)
        {
            var path = Path.Combine(FileStorageFullPath, file.Name);
            if (File.Exists(path))
                return;

            using (var stream = new MemoryStream(file.Data!))
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                await stream.CopyToAsync(fileStream);
        }

        public string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string? contentType))
                contentType = DefaultContentType;

            return contentType;
        }
    }
}
