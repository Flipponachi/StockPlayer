using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XOProject.Repository.Domain;
using XOProject.Repository.Exchange;
using XOProject.Services.Domain;

namespace XOProject.Services.Exchange
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IShareRepository _shareRepository;

        public AnalyticsService(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task<AnalyticsPrice> GetDailyAsync(string symbol, DateTime day)
        {
            
            // TODO: Add implementation for the daily summary
             //Check if symbol exists first
            var symbolExists = await _shareRepository.ShareSymbolExists(symbol);

            if (!symbolExists)
            {
                throw new Exception("Share Symbol doesn't exist");
            }

            //Load all prices for the share at that time
            var allShares = await _shareRepository.AllShares(symbol, day);

            var apsa = new AnalyticsPrice();

            //First and Last index contains Open and Closing price
            if (allShares.Count != 0)
            {
                apsa.Open = allShares[0].Rate;
                apsa.Close = allShares[allShares.Count - 1].Rate;
            }

            //Calculate High and Low price
            List<decimal> valuesPrice = new List<decimal>();

            foreach (var rate in allShares)
            {
                valuesPrice.Add(rate.Rate);
            }

            apsa.High = valuesPrice.Max();
            apsa.Low = valuesPrice.Min();


            return apsa;
        }

        public async Task<AnalyticsPrice> GetWeeklyAsync(string symbol, int year, int week)
        {
            // TODO: Add implementation for the weekly summary
            throw new NotImplementedException();
        }

        public async Task<AnalyticsPrice> GetMonthlyAsync(string symbol, int year, int month)
        {
            // TODO: Add implementation for the monthly summary
            throw new NotImplementedException();
        }
    }
}