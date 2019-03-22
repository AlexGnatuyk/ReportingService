using System;
using System.Collections.Generic;
using System.Linq;
using ReportingService.Abstractions;
using ReportingService.Models;
using TradingPlatform;

namespace ReportingService
{
    public class PeriodWorker : IPeriodWorker
    {
        private const int TimeShift = 5;

        public List<PowerPeriodSummary> AggregatePeriods(IEnumerable<TradingPeriod> trades)
        {
            return trades.GroupBy(x => x.Period).Select(y =>
                new PowerPeriodSummary
                {
                    LocalTime = TransofmPeriodToTime(y.Key),
                    Volume = y.Sum(v => v.Volume)
                }).ToList();
        }

        private string TransofmPeriodToTime(int period)
        {
            return TimeSpan.FromHours(ModifyPeriod(period)).ToString("hh':'mm");
        }

        private int ModifyPeriod(int period)
        {
            return period + TimeShift;
        }
    }
}