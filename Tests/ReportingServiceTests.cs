using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using ReportingService;
using ReportingService.Abstractions;
using ReportingService.Models;
using TradingPlatform;

namespace Tests
{
    [TestClass]
    public class ReportingServiceTests
    {
        Mock<IWriter> writerMock;
        Mock<ILogger> loggerStub;

        [TestInitialize]
        public void Initialization()
        {
            writerMock = new Mock<IWriter>();
            loggerStub = new Mock<ILogger>();
        }

        [TestMethod]
        public void WriteToCsvMethodCallCheck_AllIsOk()
        {
            var tradeWorkerMock = new Mock<ITradeWorker>();

            var reportGeneratorService = new ReportGenerator(writerMock.Object, tradeWorkerMock.Object, loggerStub.Object);
            reportGeneratorService.GenerateReport();

            writerMock.Verify(m => m.WriteToCsv(It.IsAny<DateTime>(), It.IsAny<IEnumerable<PowerPeriodSummary>>()), Times.Once);
        }

        [TestMethod]
        public void GetPowerPeriodsMethodCallCheck_AlIsOk()
        {
            var tradeWorkerMock = new Mock<ITradeWorker>();

            var reportGeneratorService = new ReportGenerator(writerMock.Object, tradeWorkerMock.Object, loggerStub.Object);
            reportGeneratorService.GenerateReport();

            tradeWorkerMock.Verify(m => m.GetPowerPeriodsSummary(It.IsAny<DateTime>()), Times.Once);

        }

        [TestMethod]
        public void Aggregating_AllIsOk()
        {
            IEnumerable<TradingPeriod> trades = new List<TradingPeriod>();
            var testData = new List<TradingPeriod>
            {
                new TradingPeriod { Volume = 100, Period = 1}, new TradingPeriod { Volume = 100, Period = 2}, 
                new TradingPeriod { Volume = 50, Period = 1}, new TradingPeriod { Volume = 50, Period = 2}
            };

            var expectedData = new List<PowerPeriodSummary>
            {
                new PowerPeriodSummary {LocalTime = "06:00", Volume = 150},
                new PowerPeriodSummary {LocalTime = "07:00", Volume = 150}
            };
            var periodWorker = new PeriodWorker();

            var result = periodWorker.AggregatePeriods(testData);
            
            CollectionAssert.AreEqual(expectedData, result);
        }
    }
}
