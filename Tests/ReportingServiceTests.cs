using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using ReportingService;
using ReportingService.Abstractions;
using ReportingService.Models;

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
    }
}
