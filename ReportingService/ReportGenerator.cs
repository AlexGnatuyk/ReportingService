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
            _logger.Info($"[ReportGenerator] ReportGeneratow was created with path= '{_path}'");
        }

        public void GenerateReport()
        {
            var requestTime = DateTime.Now;
            var requestWasSuccess = false;

            while (!requestWasSuccess)
            {
                try
                {
                    var trades = new TradingService().GetTrades(requestTime);

                    var forCheck = trades.GroupBy(a => a.Date, a => a.Periods,
                        (key, g) => new { date = key, periodsSummary = g.SelectMany(x => x).GroupBy(x => x.Period).Select(y => 
                            new PeriodSummary
                            {
                                LocalTime = TimeSpan.FromHours((y.Key + 5) % 24).ToString("hh':'mm"),
                                Volume = y.Sum(v => v.Volume)
                            }) });

                    using (var writer = new StreamWriter(Path.Combine(_path + "\\PowerPosition_" + requestTime.ToString("yyyyMMdd_HHmmss") + ".csv")))
                    using (var csv = new CsvWriter(writer))
                    {
                        foreach (var x1 in forCheck)
                        {
                            csv.WriteRecords(x1.periodsSummary);
                        }
                    }
                    requestWasSuccess = true;
                }
                catch (Exception e)
                {
                    _logger.Error($"[ReportGenerator] Error with getting periodsSummary at {requestTime:yyyy/MM/dd_HHmmss} " + e.Message);
                }
            }
        }
    }
}
