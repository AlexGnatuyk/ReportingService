using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Configuration
{
    public class IntervalElement : ConfigurationElement
    {
        [ConfigurationProperty("interval", IsRequired = true)]
        public int Interval => (int) this["interval"];
    }
}
