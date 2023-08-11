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
        /// Category identifier to look for more category specific data in by current lot <see cref="Id"/>
        /// </summary>
        [Required]
        public CategoryId CategoryId { get; set; }

        [Required, MaxLength(256)]
        public string? ImageURL { get; set; }

        [MaxLength(128)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Is item sold (for historical purposes, item should be hidden from user view)
        /// </summary>
        public bool IsSold { get; set; } = false;

        [Required]
        public decimal Price { get; set; } = decimal.MinusOne;

        /// <summary>
        /// Amount of items of this lot available
        /// </summary>
        public uint Amount { get; set; } = 0;

        /// <summary>
        /// Value to sort lots by to display, higher value - higher priority
        /// </summary>
        public int Priority { get; set; } = 0;

        [Required]
        public int? AddedByUserId { get; set; }

        [Required, ForeignKey(nameof(AddedByUserId))]
        public User? AddedByUser { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime DateSold { get; set; } = DateTime.MinValue;

        #region Category lot specific data linked to current lot (only one should be data, others must be null)
        public int? VersaDebug_LotDataId { get; set; }
        [ForeignKey(nameof(VersaDebug_LotDataId))]
        public VersaDebug.LotData? LotData_VersaDebug { get; set; }

        public int? Diablo4_LotDataId { get; set; }
        [ForeignKey(nameof(Diablo4_LotDataId))]
        public Diablo4.LotData? LotData_Diablo4 { get; set; }
        #endregion

        /// <summary>
        /// Update the current lot based on the data provided by the other lot, keeping the original <see cref="Id"/>. 
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public Lot Update(Lot lot)
        {
            CategoryId = lot.CategoryId;
            ImageURL = lot.ImageURL;

            Name = lot.Name;
            Description = lot.Description;
            IsSold = lot.IsSold;
            Price = lot.Price;
            Amount = lot.Amount;
            Priority = lot.Priority;

            return this;
        }

        public Lot? Update(LotTransferObject lotTransferObject)
        {
            Lot? lot = FromTransferObject(lotTransferObject);
            if (lot is null)
                return null;

            return Update(lot);
        }

        public static Lot? FromTransferObject(LotTransferObject transferObject)
        {
            var lot = new Lot
            {
                Id = transferObject.Id,
                CategoryId = transferObject.CategoryId,
                ImageURL = transferObject.ImageURL,

                Name = transferObject.Name,
                Description = transferObject.Description,
                IsSold = transferObject.IsSold,
                Price = transferObject.Price,
                Amount = transferObject.Amount,
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
                    return null; // TODO: throw new exception?
            }

            return lot;
        }

        public LotTransferObject ToTransferObject()
        {
            var transferObject = new LotTransferObject
            {
                Id = Id,
                CategoryId = CategoryId,
                ImageURL = ImageURL,

                Name = Name,
                Description = Description,
                IsSold = IsSold,
                Price = Price,
                Amount = Amount,
                Priority = Priority,
                
                Diablo4_LotsData = LotData_Diablo4?.ToTransferObject() as Diablo4.LotDataTransferObject,
                VersaDebug_LotsData = LotData_VersaDebug?.ToTransferObject() as VersaDebug.LotDataTransferObject,
            };

            return transferObject;
        }
    }
}
