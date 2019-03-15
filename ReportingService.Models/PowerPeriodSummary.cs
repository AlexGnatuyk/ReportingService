using System;
using System.Collections.Generic;

namespace ReportingService.Models
{
    public class PowerPeriodSummary
    {
        public string LocalTime { get; set; }

        public double Volume { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            PowerPeriodSummary other = obj as PowerPeriodSummary;
            if ((Object)other == null)
                return false;

            // here you need to compare two objects
            // below is just example implementation

            return this.LocalTime == other.LocalTime
                   && this.Volume == other.Volume;
        }

        public override int GetHashCode()
        {
            var hashCode = -706147816;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LocalTime);
            hashCode = hashCode * -1521134295 + Volume.GetHashCode();
            return hashCode;
        }
    }
}
