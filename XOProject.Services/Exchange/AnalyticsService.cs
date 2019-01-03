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
            var allShares = await _shareRepository.TodayAllShares(symbol, day);

            var analyticsPrice = new AnalyticsPrice();

            //First and Last index contains Open and Closing price
            if (allShares.Count != 0)
            {
                analyticsPrice.Open = allShares[0].Rate;
                analyticsPrice.Close = allShares[allShares.Count - 1].Rate;

                //Calculate High and Low price
                List<decimal> valuesPrice = new List<decimal>();

                foreach (var rate in allShares)
                {
                    valuesPrice.Add(rate.Rate);
                }

                analyticsPrice.High = valuesPrice.Max();
                analyticsPrice.Low = valuesPrice.Min();
            }



            return analyticsPrice;
        }

        public static DateTime[] WeekDays(int Year, int WeekNumber)
        {
            DateTime startDate = new DateTime(Year, 1, 1).AddDays(7 * WeekNumber);
            startDate = startDate.AddDays(-((int)startDate.DayOfWeek));
            return Enumerable.Range(0, 7).Select(num => startDate.AddDays(num)).ToArray();
        }

        public async Task<AnalyticsPrice> GetWeeklyAsync(string symbol, int year, int week)
        {
            // TODO: Add implementation for the weekly summary
            var symbolExists = await _shareRepository.ShareSymbolExists(symbol);

            if (!symbolExists)
            {
                throw new Exception("Share Symbol doesn't exist");
            }

            var dayValues = WeekDays(year, week);
            var weekValues = _shareRepository.WeekAllShares(symbol, dayValues);

            List<decimal> valuesPrice = new List<decimal>();

           //Get week's share price
            foreach (var dateValue in weekValues.Values)
            {
                foreach (var hourlyShareRate in dateValue)
                {
                   valuesPrice.Add(hourlyShareRate.Rate);
                }
                
            }

            var analyticsPrice = new AnalyticsPrice();
            if (valuesPrice.Count != 0)
            {
                analyticsPrice.Open = valuesPrice[0];
                analyticsPrice.High = valuesPrice.Max();
                analyticsPrice.Low = valuesPrice.Min();
                analyticsPrice.Close = valuesPrice[valuesPrice.Count - 1];
            }
            return analyticsPrice;
        }

        public async Task<AnalyticsPrice> GetMonthlyAsync(string symbol, int year, int month)
        {
            // TODO: Add implementation for the monthly summary
            throw new NotImplementedException();
        }
    }
}