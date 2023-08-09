using DigitalShowcaseAPIServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DigitalShowcaseAPIServer.Data.Models.VersaDebug
{
    public class LotDataTransferObject : ICategoryLotDataTransferObject
    {
        [Required, MaxLength(36)]
        public string? GUID { get; set; }

        [Required]
        public int? GemLevel { get; set; }
    }
}
