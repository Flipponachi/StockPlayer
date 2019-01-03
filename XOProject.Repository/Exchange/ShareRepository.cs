using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XOProject.Repository.Domain;

namespace XOProject.Repository.Exchange
{
    public class ShareRepository : GenericRepository<HourlyShareRate>, IShareRepository
    {
        public ShareRepository(ExchangeContext dbContext)
        {
            DbContext = dbContext;
        }


        public async Task<bool> ShareSymbolExists(string shareSymbol)
        {
           
            return await DbContext.Shares.AnyAsync(e => e.Symbol == shareSymbol);
        }

        public async Task<List<HourlyShareRate>> AllShares(string shareSymbol, DateTime timeOfDay)
        {
            return await DbContext.Shares
                .Where(e => e.Symbol == shareSymbol && 
                            e.TimeStamp.Day == timeOfDay.Day &&
                            e.TimeStamp.Month == timeOfDay.Month &&
                            e.TimeStamp.Year == timeOfDay.Year)
                .OrderBy(e => e.TimeStamp)
                .ToListAsync();
        }
    }
}