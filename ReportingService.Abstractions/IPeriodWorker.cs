using System.Collections.Generic;
using ReportingService.Models;
using TradingPlatform;

namespace ReportingService.Abstractions
{
    public interface IPeriodWorker
    {
        List<PowerPeriodSummary> AggregatePeriods(IEnumerable<TradingPeriod> trades);
    }
}