using System;
using System.Collections.Generic;
using ReportingService.Models;

namespace ReportingService.Abstractions
{
    public delegate void Log(string str);
    public interface IWriter
    {        
        event Log LogEvent;
        void WriteToCsv(DateTime requestTime, IEnumerable<PowerPeriodSummary> periodsSummary);
    }
}