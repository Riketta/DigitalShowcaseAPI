using DigitalShowcaseAPIServer.Data.Contexts;
using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalShowcaseAPIServer.Data.APIs
{
    public class DigitalShowcaseService : IDigitalShowcaseService
    {
        readonly DigitalShowcaseContext _db;

        //public DigitalShowcaseApiEF(IOptions<DigitalShowcaseApiEFOptions> options)
        public DigitalShowcaseService(DigitalShowcaseContext db)
        {
            _db = db;
        }
    }
}
