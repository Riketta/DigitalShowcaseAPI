using System.ComponentModel.DataAnnotations;
using DigitalShowcaseAPIServer.Data.Interfaces;

namespace DigitalShowcaseAPIServer.Data.Models.VersaDebug
{
    public class LotData : ICategoryLotData
    {
        [Required]
        public int Id { get; set; }

        #region Category Specific Data
        [Required, MaxLength(36)]
        public string? GUID { get; set; }

        /// <summary>
        /// Debug value, expected to be in range [0; 100]
        /// </summary>
        [Required]
        public int? GemLevel { get; set; }
        #endregion

        public static ICategoryLotData? FromTransferObject(ICategoryLotDataTransferObject transferObject)
        {
            if (transferObject is null)
                return null;

            LotDataTransferObject obj = (transferObject as LotDataTransferObject)!;

            var lotData = new LotData()
            {
                GUID = obj.GUID,
                GemLevel = obj.GemLevel,
            };

            return lotData;
        }

        public ICategoryLotDataTransferObject ToTransferObject()
        {
            var transferObject = new LotDataTransferObject()
            {
                GUID = GUID!,
                GemLevel = GemLevel ?? 0,
            };

            return transferObject;
        }
    }
}
