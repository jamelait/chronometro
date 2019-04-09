using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoMetro.Business
{
    public class Lap
    {
        public TimeSpan Time { get; set; }
        public String Label { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Label, Common.GetReadableTime(Time.Hours, Time.Minutes, Time.Seconds));
        }
    }
}
