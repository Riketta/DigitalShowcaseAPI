using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalShowcaseAPIServer
{
    public static class IQueryableExtensionMethods
    {
        public static IQueryable<TEntity> IncludeLotData<TEntity>(this IQueryable<TEntity> source) where TEntity : Lot
        {
            return source
                .Include(lot => lot.LotData_VersaDebug)
                .Include(lot => lot.LotData_Diablo4);
        }
    }
}
