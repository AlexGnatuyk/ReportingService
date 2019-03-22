﻿using System;
using System.Collections.Generic;
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
        public void AggregatingPeriods_Success()
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

        [TestMethod]
        public void WriteToCsvMethodCallCheck_Success()
        {
            var tradeWorkerMock = new Mock<ITradeWorker>();

            var reportGeneratorService = new ReportGenerator(writerMock.Object, tradeWorkerMock.Object, loggerStub.Object);
            reportGeneratorService.GenerateReport();

            writerMock.Verify(m => m.WriteToCsv(It.IsAny<DateTime>(), It.IsAny<IEnumerable<PowerPeriodSummary>>()), Times.Once);
        }

        [TestMethod]
        public void GetPowerPeriodsMethodCallCheck_Success()
        {
            var tradeWorkerMock = new Mock<ITradeWorker>();

            var reportGeneratorService = new ReportGenerator(writerMock.Object, tradeWorkerMock.Object, loggerStub.Object);
            reportGeneratorService.GenerateReport();

            tradeWorkerMock.Verify(m => m.GetPowerPeriodsSummary(It.IsAny<DateTime>()), Times.Once);
        }

        [TestMethod]
        public void GetTradesMethodCallCheck_Success()
        {
            var adapterMock = new Mock<ITradingService>();
            var periodWorkerMock = new Mock<IPeriodWorker>();

            var tradeWorker = new TradeWorker(adapterMock.Object, periodWorkerMock.Object);
            tradeWorker.GetPowerPeriodsSummary(It.IsAny<DateTime>());

            adapterMock.Verify(m=> m.GetTrades(It.IsAny<DateTime>()), Times.Once);
        }
    }
}
