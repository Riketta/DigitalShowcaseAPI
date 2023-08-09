using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DigitalShowcaseAPIServer.Data.Models.Category;

namespace DigitalShowcaseAPIServer.Data.Models
{
    /// <summary>
    /// Contains major data required for every single lot and also references for extra lot data in category specific tables
    /// </summary>
    public class Lot
    {
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Is item sold (for historical purposes, item should be hidden from user view)
        /// </summary>
        [Required]
        public bool IsSold { get; set; }

        [Required]
        public decimal Price { get; set; } = decimal.MinusOne;

        /// <summary>
        /// Amount of items of this lot available
        /// </summary>
        [Required]
        public uint Amount { get; set; } = 0;

        /// <summary>
        /// Value to sort lots by to display, higher value - higher priority
        /// </summary>
        [Required]
        public int Priority { get; set; } = 0;

        [Required]
        public int? AddedByUserId { get; set; }

        [Required, ForeignKey(nameof(AddedByUserId))]
        public User? AddedByUser { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public DateTime DateSold { get; set; } = DateTime.MinValue;

        [Required, MaxLength(256)]
        public string? ImageURL { get; set; }

        /// <summary>
        /// Category identifier to look for more category specific data in by current lot <see cref="Id"/>
        /// </summary>
        [Required]
        public CategoryId CategoryId { get; set; }

        #region Category lot specific data linked to current lot (only one should be data, others must be null)
        public int? VersaDebug_LotDataId { get; set; }
        [ForeignKey(nameof(VersaDebug_LotDataId))]
        public VersaDebug.LotData? LotData_VersaDebug { get; set; }

        public int? Diablo4_LotDataId { get; set; }
        [ForeignKey(nameof(Diablo4_LotDataId))]
        public Diablo4.LotData? LotData_Diablo4 { get; set; }
        #endregion

        public static Lot? FromTransferObject(LotTransferObject transferObject)
        {
            var lot = new Lot
            {
                CategoryId = transferObject.CategoryId,
                ImageURL = transferObject.ImageURL,

                IsSold = transferObject.IsSold,
                Price = transferObject.Price,
                Priority = transferObject.Priority,
            };

            switch (transferObject.CategoryId)
            {
                case CategoryId.VersaDebug:
                    lot.LotData_VersaDebug = VersaDebug.LotData.FromTransferObject(transferObject.VersaDebug_LotsData!) as VersaDebug.LotData;
                    break;

                case CategoryId.Diablo4:
                    lot.LotData_Diablo4 = Diablo4.LotData.FromTransferObject(transferObject.Diablo4_LotsData!) as Diablo4.LotData;
                    break;

                default:
                    return null;
            }

            return lot;
        }

        public LotTransferObject ToTransferObject()
        {
            var transferObject = new LotTransferObject
            {
                CategoryId = CategoryId,
                ImageURL = ImageURL,
                IsSold = IsSold,
                Price = Price,
                Priority = Priority,
                
                Diablo4_LotsData = LotData_Diablo4?.ToTransferObject() as Diablo4.LotDataTransferObject,
                VersaDebug_LotsData = LotData_VersaDebug?.ToTransferObject() as VersaDebug.LotDataTransferObject,
            };

            return transferObject;
        }
    }
}
