using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingService.Abstractions;
using ReportingService.Adapter;
using ReportingService.Models;
using TradingPlatform;

namespace ReportingService
{
    public class TradeWorker : ITradeWorker
    {
        private const int TimeShift = 5;

        public IEnumerable<PowerPeriodSummary> GetPowerPeriodsSummary(DateTime requestTime)
        {
            ITradingService tradeAdapter = new TradeAdapter();
            var trades = tradeAdapter.GetTrades(requestTime);

            return AggregatePeriods(trades);
        }

        private string TransofmPeriodToTime(int period)
        {
            return TimeSpan.FromHours(ModifyPeriod(period)).ToString("hh':'mm");
        }

        private int ModifyPeriod(int period)
        {
            return (period + TimeShift) % 24;
        }

        private IEnumerable<PowerPeriodSummary> AggregatePeriods(IEnumerable<Trade> trades)
        {
            return trades.Select(x => x.Periods).SelectMany(x => x).GroupBy(x => x.Period).Select(y =>
                new PowerPeriodSummary
                {
                    LocalTime = TransofmPeriodToTime(y.Key),
                    Volume = y.Sum(v => v.Volume)
                });
        }
    }
}
