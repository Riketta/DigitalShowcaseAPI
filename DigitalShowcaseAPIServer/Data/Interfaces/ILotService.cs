using DigitalShowcaseAPIServer.Data.Models;

namespace DigitalShowcaseAPIServer.Data.Interfaces
{
    public interface ILotService
    {
        /// <summary>
        /// Total lots count across all categories
        /// </summary>
        /// <returns></returns>
        public Task<int> GetTotalLotsCountAsync(bool includeSoldLots = false);

        /// <summary>
        /// Get lots count in specific category <see cref="Category.CategoryId"/>
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="includeSoldLots"></param>
        /// <returns></returns>
        public Task<int> GetLotsCountAsync(Category.CategoryId categoryId, bool includeSoldLots = false);

        /// <summary>
        /// Get lots from specific category <see cref="Category.CategoryId"/>
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <param name="includeSoldLots"></param>
        /// <returns></returns>
        public Task<List<Lot>?> GetLotsAsync(int pageSize, int pageIndex, Category.CategoryId categoryId = Category.CategoryId.None, bool includeSoldLots = false);
        public Task<Lot?> AddLotAsync(Lot? lot, int userId);
        public Task<Lot?> GetLotAsync(int id);
        public Task<Lot?> UpdateLotAsync(Lot? lot);
        public Task<bool?> DeleteLotAsync(int id);
    }
}
