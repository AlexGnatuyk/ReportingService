using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using ReportingService.Abstractions;
using ReportingService.Models;
using IWriter = ReportingService.Abstractions.IWriter;

namespace ReportingService
{
    public class Writer : IWriter
    {
        private readonly string _path;
        public event Log LogEvent;

        public Writer(string path)
        {
            _path = path;
        }

        public void WriteToCsv(DateTime requestTime, IEnumerable<PowerPeriodSummary> periodsSummary)
        {
            using (var writer =
                new StreamWriter(Path.Combine(_path + "\\PowerPosition_" + requestTime.ToString("yyyyMMdd_HHmm") +
                                              ".csv")))
            {
                LogEvent?.Invoke($"[ReportGenerator] File PowerPosition_{requestTime:yyyyMMdd_HHmm}.csv was created successfully");
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(periodsSummary);
                    LogEvent?.Invoke($"[ReportGenerator] Data in file PowerPosition_{requestTime:yyyyMMdd_HHmm}.csv was wrote successfully");
                }
            }
        }
    }
}
