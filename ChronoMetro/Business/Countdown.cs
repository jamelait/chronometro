using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ChronoMetro.Business
{
    class Countdown
    {
        private DispatcherTimer _dispatcherTimer;
        public TimeSpan StartTime { get; set; }
        public TimeSpan CurrentTime { get; set; }

        private readonly TimeSpan _oneSecond = new TimeSpan(0,0,1);

        // TODO create event OnSecondPassed

        public int Seconds
        {
            get
            {
                return CurrentTime.Seconds;
            }
        }

        public Countdown()
        {
            StartTime = new TimeSpan(2, 2, 1); // todo remove

            //  DispatcherTimer setup
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = _oneSecond;
        }

        public void Start()
        {
            CurrentTime = StartTime;
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            CurrentTime = CurrentTime.Subtract(_oneSecond);
            Debug.WriteLine(CurrentTime.Seconds);
        }

        public void Stop()
        {
            _dispatcherTimer.Stop();
        }
    }
}
