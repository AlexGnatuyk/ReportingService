using System;
using System.Collections.Generic;
using TradingPlatform;

namespace ReportingService.Abstractions
{
    public interface ITradingService
    {
        IEnumerable<Trade> GetTrades(DateTime date);
    }
}
