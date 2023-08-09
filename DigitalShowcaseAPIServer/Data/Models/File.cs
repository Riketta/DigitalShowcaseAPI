using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalShowcaseAPIServer.Data.Models
{
    public class File
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FileType
        {
            Unknown = 0,

            [Description("image/png")]
            PNG = 1,

            [Description("image/jpeg")]
            JPEG = 2,
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int? UploadedByUserId { get; set; }

        [Required, ForeignKey(nameof(UploadedByUserId))]
        public User? UploadedByUser { get; set; }

        /// <summary>
        /// Randomly generated name to safely store file on disk
        /// </summary>
        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public byte[]? Data { get; set; }

        [Required]
        public FileType Type { get; set; } = FileType.Unknown;
    }
}
