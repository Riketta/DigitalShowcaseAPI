namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class FileServiceOptions
    {
        public string ContentRoot { get; set; } = string.Empty;
        public string StaticFolder { get; set; } = string.Empty;
        public string UploadsFolder { get; set; } = string.Empty;
    }
}
