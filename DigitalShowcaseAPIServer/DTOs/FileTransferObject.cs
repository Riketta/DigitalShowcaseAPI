using static DigitalShowcaseAPIServer.Data.Models.File;

namespace DigitalShowcaseAPIServer.DTOs
{
    public class FileTransferObject
    {
        public IFormFile File { get; set; } = null!;
        //public FileType FileType { get; set; } = FileType.Unknown;
    }
}
