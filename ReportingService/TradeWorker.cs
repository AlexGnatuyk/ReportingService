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
        private readonly ITradingService _tradeAdapter;
        private readonly IPeriodWorker _periodWorker;

        public TradeWorker(ITradingService tradeAdapter, IPeriodWorker periodWorker)
        {
            _tradeAdapter = tradeAdapter;
            _periodWorker = periodWorker;
        }

        public IEnumerable<PowerPeriodSummary> GetPowerPeriodsSummary(DateTime requestTime)
        {
            var trades = _tradeAdapter.GetTrades(requestTime).SelectMany(tr => tr.Periods);

            return _periodWorker.AggregatePeriods(trades);
        }
        
    }
}
