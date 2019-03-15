using System;
using NLog;
using ReportingService.Abstractions;

namespace ReportingService
{
    public class ReportGenerator
    {
        private readonly ILogger _logger;
        private readonly IWriter _writer;
        private readonly ITradeWorker _tradeWorker;
        public ReportGenerator(IWriter writer, ITradeWorker tradeWorker, ILogger logger)
        {
            _writer = writer;            
            _logger = logger;
            _tradeWorker = tradeWorker;
            _writer.LogEvent += (str) => _logger.Info(str);
        }

        public void GenerateReport()
        {
            var requestTime = DateTime.Now;
            var requestSuccess = false;

            while (!requestSuccess)
            {
                try
                {
                    var powerPeriods = _tradeWorker.GetPowerPeriodsSummary(requestTime);
                    _writer.WriteToCsv(requestTime, powerPeriods);
                    requestSuccess = true;
                }
                catch (Exception e)
                {
                    _logger.Error($"[ReportGenerator] Error with getting periodsSummary at {requestTime:yyyy/MM/dd_HHmm} " + e.Message);
                }
            }
        }

       
    }
}
