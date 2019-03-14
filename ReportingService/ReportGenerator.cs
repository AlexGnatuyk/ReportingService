using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using NLog;
using TradingPlatform;

namespace ReportingService
{
    class ReportGenerator
    {
        private readonly string _path;
        private readonly Logger _logger;
        public ReportGenerator(string path, Logger logger)
        {
            _path = path;
            _logger = logger;
            _logger.Info($"[ReportGenerator] ReportGenerator was created with path = '{_path}'");
        }

        public void GenerateReport()
        {
            var requestTime = DateTime.Now;
            var requestSuccess = false;

            while (!requestSuccess)
            {
                try
                {
                    var powerPeriods = GetPowerPeriodsSummary(requestTime);
                    WriteToCsv(requestTime, powerPeriods);
                    requestSuccess = true;
                }
                catch (Exception e)
                {
                    _logger.Error($"[ReportGenerator] Error with getting periodsSummary at {requestTime:yyyy/MM/dd_HHmmss} " + e.Message);
                }
            }
        }

        private IEnumerable<PowerPeriodSummary> GetPowerPeriodsSummary(DateTime requestTime)
        {
            var trades = new TradingService().GetTrades(requestTime);

           return trades.Select(x => x.Periods).SelectMany(x => x).GroupBy(x => x.Period).Select(y =>
                new PowerPeriodSummary
                {
                    LocalTime = TimeSpan.FromHours((y.Key + 5) % 24).ToString("hh':'mm"),
                    Volume = y.Sum(v => v.Volume)
                });
        }

        private void WriteToCsv(DateTime requestTime, IEnumerable<PowerPeriodSummary> periodsSummary)
        {
            using (var writer = new StreamWriter(Path.Combine(_path + "\\PowerPosition_" + requestTime.ToString("yyyyMMdd_HHmmss") + ".csv")))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(periodsSummary);
                }
            }
        }
    }
}
