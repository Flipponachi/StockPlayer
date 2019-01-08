using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using XOProject.Api.Controller;
using XOProject.Repository.Exchange;
using XOProject.Services.Domain;
using XOProject.Services.Exchange;

namespace XOProject.Api.Tests
{
    [TestFixture]
    class AnalyticsControllerTests
    {
        private AnalyticsController _analyticsController;
        private Mock<IAnalyticsService> _analyticsServiceMock;
       
        [SetUp]
        public void Setup()
        {
           
            _analyticsServiceMock = new Mock<IAnalyticsService>();

            _analyticsController = new AnalyticsController(_analyticsServiceMock.Object);
        }

        [Test]
        public async Task Daily_IncorrectYearValue_ShouldReturnBadrequest()
        {
           
            var newRequest = await _analyticsController.Daily("ASA", 1, 1, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("The year value is less"));
            
        }

        [Test]
        public async Task Daily_IncorrectMonthValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Daily("ASA", 2000, 13, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("The month value doesn't exist"));

        }

        [Test]
        public async Task Daily_IncorrectDayValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Daily("ASA", 2000, 1, 34);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("The day value doesn't exist"));

        }

        [Test]
        public async Task Daily_IncorrectDateValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Daily("ASA", 0, 0, 0);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("Incorrect Route Values for date"));

        }

        [Test]
        [TestCase("AS", "Symbol characters exceeded or below")]
        [TestCase("ASDA", "Symbol characters exceeded or below")]
        public async Task Daily_IncorrectSymbolLength_ShouldReturnBadrequest(string symbol, string responseMessage)
        {

            var newRequest = await _analyticsController.Daily(symbol, 2012, 1, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(responseMessage));

        }

        [Test]
        public async Task Daily_SupplyCorrectValue_ShouldReturnOkWitDailyModel()
        {
            _analyticsServiceMock.Setup(e => e.GetDailyAsync("ADS", new DateTime(2000,1,1))).Returns(
                Task.FromResult<AnalyticsPrice>(new AnalyticsPrice
                {
                    Close = 21.3m,
                    High = 34.1m,
                    Low = 12.4m,
                    Open = 11.10m
                }));

            var newRequest = await _analyticsController.Daily("ADS", 2000, 1, 1);

            var result = newRequest as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));          
        }

        [Test]
        public async Task Daily_SupplyWrongSymbol_ShouldReturnBadRequest()
        {
            _analyticsServiceMock.Setup(e => e.GetDailyAsync("ADS", new DateTime(2000, 1, 1)))
                .Throws(new Exception("Share Symbol doesn't exist"));

            var newRequest = await _analyticsController.Daily("ADS", 2000, 1, 1);

            var result = newRequest as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value, Is.EqualTo("Share Symbol doesn't exist"));
        }

        [Test]
        public async Task Weekly_IncorrectYearValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Weekly("ASA", 1, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("The year value is less"));

        }

        [Test]
        public async Task Weekly_IncorrectWeekValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Weekly("ASA", 2000, 53);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("Incorrect Week Value"));

        }


        [Test]
        [TestCase("AS", "Symbol characters exceeded or below")]
        [TestCase("ASDA", "Symbol characters exceeded or below")]
        public async Task Weekly_IncorrectSymbolLength_ShouldReturnBadrequest(string symbol, string responseMessage)
        {

            var newRequest = await _analyticsController.Weekly(symbol, 2012, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(responseMessage));

        }

        [Test]
        public async Task Weekly_SupplyCorrectValue_ShouldReturnOkWitDailyModel()
        {
            _analyticsServiceMock.Setup(e => e.GetWeeklyAsync("ADS", 2001, 10)).Returns(
                Task.FromResult<AnalyticsPrice>(new AnalyticsPrice
                {
                    Close = 21.3m,
                    High = 34.1m,
                    Low = 12.4m,
                    Open = 11.10m
                }));

            var newRequest = await _analyticsController.Weekly("ADS", 2001, 10);

            var result = newRequest as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Weekly_SupplyWrongSymbol_ShouldReturnBadRequest()
        {
            _analyticsServiceMock.Setup(e => e.GetWeeklyAsync("ADS", 2000, 1))
                .Throws(new Exception("Share Symbol doesn't exist"));

            var newRequest = await _analyticsController.Weekly("ADS", 2000, 1);

            var result = newRequest as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value, Is.EqualTo("Share Symbol doesn't exist"));
        }

        [Test]
        public async Task Monthly_IncorrectYearValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Monthly("ASA", 20, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("The year value is less"));

        }

        [Test]
        public async Task Monthly_IncorrectWeekValue_ShouldReturnBadrequest()
        {

            var newRequest = await _analyticsController.Monthly("ASA", 2000, 16);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("Incorrect month Value"));

        }

        [Test]
        [TestCase("AS", "Symbol characters exceeded or below")]
        [TestCase("ASDA", "Symbol characters exceeded or below")]
        public async Task Monthly_IncorrectSymbolLength_ShouldReturnBadrequest(string symbol, string responseMessage)
        {

            var newRequest = await _analyticsController.Monthly(symbol, 2012, 1);

            var result = newRequest as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(responseMessage));

        }

        [Test]
        public async Task Monthly_SupplyCorrectValue_ShouldReturnOkWitDailyModel()
        {
            _analyticsServiceMock.Setup(e => e.GetMonthlyAsync("ADS", 2001, 10)).Returns(
                Task.FromResult<AnalyticsPrice>(new AnalyticsPrice
                {
                    Close = 21.3m,
                    High = 34.1m,
                    Low = 12.4m,
                    Open = 11.10m
                }));

            var newRequest = await _analyticsController.Monthly("ADS", 2001, 10);

            var result = newRequest as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Monthly_SupplyWrongSymbol_ShouldReturnBadRequest()
        {
            _analyticsServiceMock.Setup(e => e.GetMonthlyAsync("ADS", 2000, 1))
                .Throws(new Exception("Share Symbol doesn't exist"));

            var newRequest = await _analyticsController.Monthly("ADS", 2000, 1);

            var result = newRequest as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value, Is.EqualTo("Share Symbol doesn't exist"));
        }

        [TearDown]
        public void CleanUp()
        {
            _analyticsServiceMock.Reset();
        }
    }
}
