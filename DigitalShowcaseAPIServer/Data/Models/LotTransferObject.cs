﻿using System.ComponentModel.DataAnnotations;
using static DigitalShowcaseAPIServer.Data.Models.Category;

namespace DigitalShowcaseAPIServer.Data.Models
{
    public class LotTransferObject
    {
        public int Id { get; set; }

        [Required]
        public CategoryId CategoryId { get; set; }

        [Required, MaxLength(256)]
        public string? ImageURL { get; set; }

        [MaxLength(128)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
        public bool IsSold { get; set; } = false;
        public decimal Price { get; set; } = 0;
        public uint Amount { get; set; } = 0;
        public int Priority { get; set; } = 0;

        public VersaDebug.LotDataTransferObject? VersaDebug_LotsData { get; set; }
        public Diablo4.LotDataTransferObject? Diablo4_LotsData { get; set; }
    }
}
