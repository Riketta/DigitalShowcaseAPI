using DigitalShowcaseAPIServer.Data.Contexts;
using static DigitalShowcaseAPIServer.Data.Models.Category;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    /// <summary>
    /// Category lot specific query data should be declared in actual interface implementation
    /// </summary>
    public abstract class ILotQueryParameters
    {
        private const int MaxPageSize = 100;
        private int pageSize { get; set; } = 25;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;

        public bool IncludeSoldLots { get; set; } = true;

        /// <summary>
        /// Lot Id in global lot table across all categories <see cref="DigitalShowcaseContext.Lots"/>
        /// </summary>
        public int LotId { get; set; }

        ///// <summary>
        ///// Lot data Id in specific category <see cref="CategoryId"/>
        ///// </summary>
        //public int LotDataId { get; set; }

        /// <summary>
        /// Specific category where lot's data stored
        /// </summary>
        public CategoryId CategoryId { get; set; }
    }
}
