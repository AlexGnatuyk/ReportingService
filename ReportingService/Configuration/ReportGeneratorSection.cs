using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Configuration
{
    public class ReportGeneratorSection : ConfigurationSection
    {
        [ConfigurationProperty("directory")]
        public DirectoryElement Directory => (DirectoryElement) this["directory"];

        [ConfigurationProperty("timeInterval")]
        public IntervalElement Interval => (IntervalElement) this["timeInterval"];
    }
}
