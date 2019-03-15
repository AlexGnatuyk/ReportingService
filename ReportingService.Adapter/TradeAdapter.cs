using System;
using System.Collections.Generic;
using ReportingService.Abstractions;
using TradingPlatform;

namespace ReportingService.Adapter
{
   public class TradeAdapter : ITradingService
    {
        public IEnumerable<Trade> GetTrades(DateTime date)
        {
            return new TradingService().GetTrades(date);
        }
    }
}
