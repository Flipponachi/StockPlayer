using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using XOProject.Api.Model.Analytics;
using XOProject.Services.Domain;
using XOProject.Services.Exchange;

using Microsoft.AspNetCore.Mvc;

namespace XOProject.Api.Controller
{
    [Route("api")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

       
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("daily/{symbol}/{year}/{month}/{day}")]
        public async Task<IActionResult> Daily([FromRoute] string symbol, [FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            // TODO: Add implementation for the daily summary

            //validate route values
            if (symbol.Length > 3)
            {
                return BadRequest("Symbol is more than 3 characters");
            }

            if (year == 0 || month == 0 || day == 0)
            {
                return BadRequest("Incorrect Route Values");
            }

            var yearAsString = year.ToString();
            if (yearAsString.Length > 4)
            {
                return BadRequest("The year value is less");
            }

            if (!Enumerable.Range(1, 12).Contains(month))
            {
                return BadRequest("The month value doesn't exist");
            }

            if (!Enumerable.Range(1, 31).Contains(day))
            {
                return BadRequest("The day value doesn't exist");
            }


            //Parse from route values Datetime object
            var queryTime = new DateTime(year, month, day);

            AnalyticsPrice todaysAnalyticsPrice = new AnalyticsPrice();
            //Set validation for route values
            try
            {
                todaysAnalyticsPrice = await _analyticsService.GetDailyAsync(symbol, queryTime);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

           
            var result = new DailyModel()
            {
                Symbol = symbol,
                Day = queryTime,
                Price = Map(todaysAnalyticsPrice)
            };

            return Ok(result);
        }

        [HttpGet("weekly/{symbol}/{year}/{week}")]
        public async Task<IActionResult> Weekly([FromRoute] string symbol, [FromRoute] int year, [FromRoute] int week)
        {
            // TODO: Add implementation for the weekly summary
            var result = new WeeklyModel()
            {
                Symbol = symbol,
                Year = year,
                Week = week,
                Price = Map(new AnalyticsPrice())
            };

            return Ok(result);
        }

        [HttpGet("monthly/{symbol}/{year}/{month}")]
        public async Task<IActionResult> Monthly([FromRoute] string symbol, [FromRoute] int year, [FromRoute] int month)
        {
            // TODO: Add implementation for the monthly summary
            var result = new MonthlyModel()
            {
                Symbol = symbol,
                Year = year,
                Month = month,
                Price = Map(new AnalyticsPrice())
            };

            return Ok(result);
        }

        private PriceModel Map(AnalyticsPrice price)
        {
            return new PriceModel()
            {
                Open = price.Open,
                Close = price.Close,
                High = price.High,
                Low = price.Low
            };
        }
    }
}