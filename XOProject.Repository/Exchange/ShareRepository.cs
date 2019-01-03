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

        public async Task<List<HourlyShareRate>> TodayAllShares(string shareSymbol, DateTime timeOfDay)
        {
            return await DbContext.Shares
                .Where(e => e.Symbol == shareSymbol && 
                            e.TimeStamp.Day == timeOfDay.Day &&
                            e.TimeStamp.Month == timeOfDay.Month &&
                            e.TimeStamp.Year == timeOfDay.Year)
                .OrderBy(e => e.TimeStamp)
                .ToListAsync();
        }

        public Dictionary<DateTime, List<HourlyShareRate>> WeekAllShares(string shareSymbol, DateTime[] weekDays)
        {
            Dictionary<DateTime, List<HourlyShareRate>> list = new Dictionary<DateTime, List<HourlyShareRate>>();

            foreach (var dateTime in weekDays)
            {
                var dayValue = DbContext.Shares
                    .Where(e => e.Symbol == shareSymbol &&
                                e.TimeStamp.Day == dateTime.Day &&
                                e.TimeStamp.Month == dateTime.Month &&
                                e.TimeStamp.Year == dateTime.Year)
                    .OrderBy(e => e.TimeStamp)
                    .ToList();
                list.Add(dateTime, dayValue);
            }

            return list;
        }
    }
}