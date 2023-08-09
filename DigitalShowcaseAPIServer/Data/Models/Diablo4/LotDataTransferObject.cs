using DigitalShowcaseAPIServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DigitalShowcaseAPIServer.Data.Models.Diablo4
{
    public class LotDataTransferObject : ICategoryLotDataTransferObject
    {
        [MaxLength(128)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public Class.ClassId? Class { get; set; }

        [Required]
        public ItemType.ItemId? ItemType { get; set; }
    }
}
