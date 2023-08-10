using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class LotService : ILotService
    {
        private readonly DigitalShowcaseContext _db;

        public LotService(DigitalShowcaseContext db)
        {
            _db = db;
        }

        public async Task<int> GetTotalLotsCountAsync(bool includeSoldLots = false)
        {
            return await _db.Lots.CountAsync(lot => !lot.IsSold || (lot.IsSold && includeSoldLots));
        }

        public async Task<int> GetLotsCountAsync(Category.CategoryId categoryId, bool includeSoldLots = false)
        {
            return await _db.Lots.CountAsync(lot => lot.CategoryId == categoryId && (!lot.IsSold || (lot.IsSold && includeSoldLots)));
        }

        /// <summary>
        /// Get all lots that passed filtering query
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <param name="includeSoldLots"></param>
        /// <returns></returns>
        public async Task<List<Lot>> GetLotsAsync(int pageSize, int pageIndex, Category.CategoryId categoryId = Category.CategoryId.None, bool includeSoldLots = false)
        {
            return await _db.Lots // TODO: implement querying and pagination
                .Include(lot => lot.LotData_VersaDebug)
                .Include(lot => lot.LotData_Diablo4)
                .ToListAsync();

            //if (categoryId == Category.CategoryId.None)
            //    return await _db.Lots
            //        .Where(lot => !lot.IsSold || (lot.IsSold && includeSoldLots))
            //        .OrderBy(lot => lot.Priority)
            //        .Skip((pageIndex - 1) * pageSize)
            //        .Take(pageSize)
            //        .ToListAsync();

            //return await _db.Lots
            //    .Where(lot => lot.CategoryId == categoryId && (!lot.IsSold || (lot.IsSold && includeSoldLots)))
            //    .OrderBy(lot => lot.Priority)
            //    .Skip((pageIndex - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToListAsync();
        }

        public async Task<Lot?> GetLotAsync(int id)
        {
            Lot? lot = await _db.Lots
                .Include(lot => lot.LotData_VersaDebug)
                .Include(lot => lot.LotData_Diablo4)
                .SingleOrDefaultAsync(lot => lot.Id == id);

            if (lot is null)
                return null;

            switch (lot.CategoryId)
            {
                case Category.CategoryId.VersaDebug:
                    if (lot.LotData_VersaDebug is null)
                        throw new MissingFieldException(nameof(lot.LotData_VersaDebug));
                    break;

                case Category.CategoryId.Diablo4:
                    if (lot.LotData_Diablo4 is null)
                        throw new MissingFieldException(nameof(lot.LotData_Diablo4));
                    break;
            }

            return lot;
        }

        public async Task<Lot?> AddLotAsync(Lot? lot, int userId)
        {
            if (lot is null)
                return null;

            // Filling required internal fields
            lot.Id = 0; // prevents user to try to add existing Id and expose exception
            lot.AddedByUserId = userId;

            lot = _db.Lots.Add(lot).Entity;

            switch (lot.CategoryId)
            {
                case Category.CategoryId.VersaDebug:
                    if (lot.LotData_VersaDebug is null) return null;
                    await _db.VersaDebug_LotsData.AddAsync(lot.LotData_VersaDebug);
                    break;

                case Category.CategoryId.Diablo4:
                    if (lot.LotData_Diablo4 is null) return null;
                    await _db.Diablo4_LotsData.AddAsync(lot.LotData_Diablo4);
                    break;
            }

            await _db.SaveChangesAsync();
            return lot;
        }

        public async Task<Lot?> UpdateLotAsync(Lot? lot) // TODO: rewrite usinng transfer objects
        {
            if (lot is null)
                return null;

            Lot? existingLot = await _db.Lots
                                .Include(lot => lot.LotData_VersaDebug)
                                .Include(lot => lot.LotData_Diablo4)
                                .SingleOrDefaultAsync(l => l.Id == lot.Id);

            if (existingLot is null)
                return null;

            existingLot.Name = lot.Name;
            existingLot.Description = lot.Description;
            existingLot.IsSold = lot.IsSold;
            existingLot.Price = lot.Price;
            existingLot.Amount = lot.Amount;
            existingLot.Priority = lot.Priority;
            existingLot.DateSold = lot.DateSold;

            switch (existingLot.CategoryId)
            {
                case Category.CategoryId.VersaDebug:
                    if (existingLot.LotData_VersaDebug is null || lot.LotData_VersaDebug is null)
                        return null;

                    existingLot.LotData_VersaDebug.GUID = lot.LotData_VersaDebug.GUID;
                    existingLot.LotData_VersaDebug.GemLevel = lot.LotData_VersaDebug.GemLevel;
                    break;

                case Category.CategoryId.Diablo4:
                    if (existingLot.LotData_Diablo4 is null || lot.LotData_Diablo4 is null)
                        return null;

                    existingLot.LotData_Diablo4.Level = lot.LotData_Diablo4.Level;
                    existingLot.LotData_Diablo4.Class = lot.LotData_Diablo4.Class; // TODO: use Id?
                    existingLot.LotData_Diablo4.ItemType = lot.LotData_Diablo4.ItemType;
                    break;
            }

            await _db.SaveChangesAsync();
            return existingLot;
        }

        public async Task<bool?> DeleteLotAsync(int id)
        {
            Lot? lot = await _db.Lots
                .Include(lot => lot.LotData_VersaDebug)
                .Include(lot => lot.LotData_Diablo4)
                .SingleOrDefaultAsync(lot => lot.Id == id);
            
            if (lot is null)
                return false;
            
            _db.Lots.Remove(lot);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
