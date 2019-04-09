using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoMetro.Data.Chronometer
{
    public class ChronometerItem
    {
        public String Label { get; set; }
        public DateTime CreatedAt { get; set; }
        public TimeSpan Time { get; set; }
    }
}
