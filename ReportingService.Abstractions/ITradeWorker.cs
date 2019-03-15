using System;
using System.Collections.Generic;
using ReportingService.Models;

namespace ReportingService.Abstractions
{
    public interface ITradeWorker
    {
        IEnumerable<PowerPeriodSummary> GetPowerPeriodsSummary(DateTime requestTime);
    }
}