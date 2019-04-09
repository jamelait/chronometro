using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using ChronoMetro.Business;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChronoMetro.UserControls
{
    public partial class IntervalBlock : UserControl, INotifyPropertyChanged
    {
        private static int _currentOrder = 0;

        public event EventHandler OnFinished;

        public int Order { get; private set; }
        
        private readonly Stopwatch _stopwatch;
        private readonly DispatcherTimer _dispatcherTimer;
        private TimeSpan _currentTime;
        private SolidColorBrush _fillColor;
        private SolidColorBrush _foregroundColor;
        
        private double _progressBarValue;
        private double _progressBarMax;

        public TimeSpan OriginalTime { get; private set; } // todo remove use GetOriginalTime() instead
        public TimeSpan ElapsedTime { get { return _stopwatch.Elapsed; } }
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; }
        public bool IsRunning { get { return _stopwatch.IsRunning; } }

        public SolidColorBrush FillColor
        {
            get { return _fillColor; }
            set { this.SetProperty(ref _fillColor, value); }
        }
        public SolidColorBrush ForegroundColor
        {
            get { return _foregroundColor; }
            set { this.SetProperty(ref _foregroundColor, value); }
        }
        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set { this.SetProperty(ref _currentTime, value); }
        }
        public String CenterText
        {
            get { return CenterTextBlock.Text; }
            set { CenterTextBlock.Text = value; }
        }
        public double ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                if (value <= 0) return; //  bug resolution : line 128 ProgressBar.Value not updating after reset (pas vraiment non)
                this.SetProperty(ref _progressBarValue, value);
            }
        }
        public double ProgressBarMax
        {
            get { return _progressBarMax; }
            set { this.SetProperty(ref _progressBarMax, value); }
        }

        public IntervalBlock()
        {
            InitializeComponent();

            Order = _currentOrder++;
            this.DataContext = this;

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += Tick;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);

            _stopwatch = new Stopwatch();
            
            HasFinished = false;
            CurrentTime = TimeSpan.FromSeconds(0);
            FillColor = Common.BlueColorBrush;
            ForegroundColor = Common.BlackColorBrush;
        }

        public TimeSpan GetOriginalTime()
        {
            if (HasStarted)
                return OriginalTime;
            else
                return CurrentTime;
        }

        public void InitValues(string label, TimeSpan time)
        {
            CenterText = label;
            _currentTime = time;
        }

        public void Start()
        {
            if (HasFinished)
                return;

            _stopwatch.Start();
            _dispatcherTimer.Start();

            if (!HasStarted)
            {
                OriginalTime = CurrentTime;
                ProgressBarMax = OriginalTime.TotalSeconds;
            }
            
            HasStarted = true;
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _dispatcherTimer.Stop();
        }
        private void Tick(object sender, EventArgs e)
        {
            if (HasFinished)
                throw new Exception();

            // update Time
            var current = OriginalTime.Subtract(_stopwatch.Elapsed);
            if (current.CompareTo(TimeSpan.Zero) < 0)
                current = TimeSpan.Zero;
            CurrentTime = current;
            HasFinished = CurrentTime.TotalSeconds <= 0;

            //ProgressBarValue = CurrentTime.TotalSeconds; // bug value not visible on view after reset
            ProgressBar.Value = CurrentTime.TotalSeconds;

            // update background color
            if (HasFinished)
            {
                FillColor = Common.GrayColorBrush;
                ForegroundColor = Common.DarkGrayColorBrush;
                if (OnFinished != null)
                    OnFinished(this, null);
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));

            Debug.WriteLine("OnPropertyChanged " + propertyName + " " + eventHandler);
        }

        #endregion

        public void Reset()
        {
            _stopwatch.Reset();

            FillColor = Common.BlueColorBrush;
            ForegroundColor = Common.BlackColorBrush;
            CurrentTime = GetOriginalTime();

            //ProgressBarValue = OriginalTime.TotalSeconds; // bug see line 128
            ProgressBar.Value = CurrentTime.TotalSeconds;

            HasStarted = false;
            HasFinished = false;
        }
    }
}
