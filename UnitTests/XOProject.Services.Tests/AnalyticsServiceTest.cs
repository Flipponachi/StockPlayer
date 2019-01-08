using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using XOProject.Repository.Domain;
using XOProject.Repository.Exchange;
using XOProject.Services.Domain;
using XOProject.Services.Exchange;

namespace XOProject.Services.Tests
{
    [TestFixture]
    class AnalyticsServiceTest
    {
        private Mock<IShareRepository> _shareRepoMoq;
        private AnalyticsService _analyticsService;

        [SetUp]
        public void Setup()
        {
            _shareRepoMoq = new Mock<IShareRepository>();
            _analyticsService = new AnalyticsService(_shareRepoMoq.Object);
        }

        [Test]
        public void GetDailyAsync_SymbolNotExist_ThrowsAnException()
        {
            _shareRepoMoq.Setup(e => e.ShareSymbolExists("Some")).Throws(new Exception("Share Symbol doesn't exist"));

            var result = _analyticsService.GetDailyAsync("A", DateTime.Now);

            Assert.IsNotNull(result);
            Assert.That(result.Exception.InnerExceptions[0].Message, Is.EqualTo("Share Symbol doesn't exist"));
        }


        [Test]
        public void GetWeeklyAsync_SymbolNotExist_ThrowsAnException()
        {
            _shareRepoMoq.Setup(e => e.ShareSymbolExists("Some")).Throws(new Exception("Share Symbol doesn't exist"));

            var result = _analyticsService.GetWeeklyAsync("A", 1, 3);

            Assert.IsNotNull(result);
            Assert.That(result.Exception.InnerExceptions[0].Message, Is.EqualTo("Share Symbol doesn't exist"));
        }

        [Test]
        public void GetMonthlyAsync_SymbolNotExist_ThrowsAnException()
        {
            _shareRepoMoq.Setup(e => e.ShareSymbolExists("Some")).Throws(new Exception("Share Symbol doesn't exist"));

            var result = _analyticsService.GetMonthlyAsync("A", 3, 5);

            Assert.IsNotNull(result);
            Assert.That(result.Exception.InnerExceptions[0].Message, Is.EqualTo("Share Symbol doesn't exist"));
        }
    }
}
