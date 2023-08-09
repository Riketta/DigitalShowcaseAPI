using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalShowcaseAPIServer.Data.Interfaces;

namespace DigitalShowcaseAPIServer.Data.Models.Diablo4
{
    public class LotData : ICategoryLotData
    {
        [Required]
        public int Id { get; set; }

        #region Category Specific Data
        [Required]
        public int Level { get; set; }

        [Required]
        public Class.ClassId? ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class? Class { get; set; }

        [Required]
        public ItemType.ItemId? ItemTypeId { get; set; }

        [ForeignKey(nameof(ItemTypeId))]
        public ItemType? ItemType { get; set; }
        #endregion

        public static ICategoryLotData? FromTransferObject(ICategoryLotDataTransferObject transferObject)
        {
            if (transferObject is null)
                return null;

            LotDataTransferObject obj = (transferObject as LotDataTransferObject)!;

            var lotData = new LotData()
            {
                Level = obj.Level,
                ClassId = obj.Class,
                ItemTypeId = obj.ItemType,
            };

            return lotData;
        }

        public ICategoryLotDataTransferObject ToTransferObject()
        {
            var transferObject = new LotDataTransferObject()
            {
                Level = Level,
                Class = ClassId,
                ItemType = ItemTypeId,
            };

            return transferObject;
        }
    }
}
