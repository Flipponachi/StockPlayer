using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XOProject.Repository.Domain;

namespace XOProject.Repository.Exchange
{
    public interface IShareRepository : IGenericRepository<HourlyShareRate>
    {
        Task<bool> ShareSymbolExists(string shareSymbol);
        Task<List<HourlyShareRate>> TodayAllShares(string shareSymbol, DateTime timeOfDay);
        Dictionary<DateTime, List<HourlyShareRate>> WeekAllShares(string shareSymbol, DateTime[] weekDays);
    }
}