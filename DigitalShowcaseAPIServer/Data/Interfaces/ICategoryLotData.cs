using DigitalShowcaseAPIServer.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    /// <summary>
    /// Category lot specific data should be declared in actual interface implementation
    /// </summary>
    public interface ICategoryLotData
    {
        /// <summary>
        /// Lot identifier in specific category table, referenced by main Lot table
        /// </summary>
        [Required]
        public int Id { get; set; }

        public static abstract ICategoryLotData? FromTransferObject(ICategoryLotDataTransferObject transferObject);
        public ICategoryLotDataTransferObject ToTransferObject();
    }
}
