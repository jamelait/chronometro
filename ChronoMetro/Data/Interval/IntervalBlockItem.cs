using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoMetro.Data.Interval
{
    public class IntervalBlockItem
    {
        public String Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid Guid { get; set; }

        public Dictionary<string, TimeSpan> Blocks { get; set; } // inervalblock.label, inervalblock.time

        public TimeSpan TotalTime { get; set; }

        public IntervalBlockItem()
        {
            Blocks = new Dictionary<string, TimeSpan>();
            CreatedAt = DateTime.Now;
            Guid = Guid.NewGuid();
            TotalTime = new TimeSpan(0, 0, 0);
        }
    }
}
