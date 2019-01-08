using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using XOProject.Api.Controller;
using XOProject.Api.Model;
using XOProject.Repository.Domain;
using XOProject.Services.Exchange;

namespace XOProject.Api.Tests
{
    [TestFixture]
    class PortfolioControllerTests
    {
        private Mock<IPortfolioService> _portfolioServiceMock;
        private PortfolioController _portfolioController;
        private List<Trade> _listOfTrades;

        [SetUp]
        public void Setup()
        {
           _portfolioServiceMock = new Mock<IPortfolioService>();
            _portfolioController = new PortfolioController(_portfolioServiceMock.Object);
            _listOfTrades = new List<Trade>
            {
                new Trade
                {
                    Action = "Sell",
                    ContractPrice = 3.53m,
                    Id = 1,
                    PortfolioId = 2,
                    NoOfShares = 300,
                    Symbol = "REL"
                },
                new Trade
                {
                    Action = "Sell",
                    ContractPrice = 2.53m,
                    Id = 2,
                    PortfolioId = 1,
                    NoOfShares = 300,
                    Symbol = "REL"
                },
                new Trade
                {
                    Action = "Sell",
                    ContractPrice = 3.23m,
                    Id = 3,
                    PortfolioId = 4,
                    NoOfShares = 600,
                    Symbol = "CBL"
                }
            };
        }

        [Test]
        public void GetPortfolio_NullPortfolio_ShouldReturnBadRequest()
        {
            _portfolioServiceMock.Setup(e => e.GetByIdAsync(1)).Returns(Task.FromResult<Portfolio>(null));
            var act = _portfolioController.GetPortfolio(1);

            var result = act.Result as NotFoundResult;

            Assert.IsNotNull(result);

        }

        [Test]
        public void GetPortfolio_CorrectId_ShouldReturnPortfolioObject()
        {
           
            _portfolioServiceMock.Setup(e => e.GetByIdAsync(1)).Returns(Task.FromResult<Portfolio>(new Portfolio{Id = 1, Name = "Jon", Trades = _listOfTrades}));
            var act = _portfolioController.GetPortfolio(1);

            var result = act.Result as OkObjectResult;

            Assert.IsNotNull(result);

        }

        [Test]
        public async Task Post_SupplyCorrectModelValue_ShouldReturnSuccess()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = "John"
            };
           

            var act = await _portfolioController.Post(portfolioModel);

            var result = act as CreatedResult;

            Assert.IsNotNull(result);
            Assert.That(result.Location, Is.EqualTo($"Portfolio/{portfolioModel.Id}"));
            Assert.That(result.StatusCode, Is.EqualTo(201));

        }


        [Test]
        public async Task Post_SupplyIncorrectModelValue_ShouldReturnBadRequest()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = "John"
            };

            _portfolioController.ModelState.AddModelError("error", "error");

            var act = await _portfolioController.Post(portfolioModel);

            var result = act as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
           

        }

        [TearDown]
        public void CleanUp()
        {
            _portfolioServiceMock.Reset();
        }
    }
}
