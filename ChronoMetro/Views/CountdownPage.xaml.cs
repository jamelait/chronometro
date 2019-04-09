using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ChronoMetro.Business;
using ChronoMetro.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SubsonicDesign;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChronoMetro.Views
{
    public partial class CountdownPage : PhoneApplicationPage
    {
        private static Brush _defaultColorBrush; // Regular color (white or black)

        private readonly DispatcherTimer _dispatcherTimer;
        private readonly TimeSpan _oneSecond = new TimeSpan(0, 0, 1);
        private TimeSpan _tsCountDownFrom;
        private TimeSpan _currentTime; 
        
        private bool _started;

        public CountdownPage()
        {
            InitializeComponent();

            _defaultColorBrush = TbkHour.Foreground;

            //  DispatcherTimer setup
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = _oneSecond;

            // Select the Seconds
            TbkTime_Tap(TbkMinute, null);
        }

        private void ButtonStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (_started)
                Stop();
            else
                Start();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            if (_started)
                Stop();

            var tsDisplayedTime = new TimeSpan(Int32.Parse(TbkHour.Text), Int32.Parse(TbkMinute.Text), Int32.Parse(TbkSecond.Text));
            if (tsDisplayedTime.Duration().Equals(_tsCountDownFrom.Duration())) // The original time is already displayed, set to 0
                DisplayTime(0,0,0);
            else
                DisplayTime(_tsCountDownFrom.Hours, _tsCountDownFrom.Minutes, _tsCountDownFrom.Seconds);

            if (SelectedTextBlock != null)
                TbkTime_Tap(SelectedTextBlock, null);
        }

        private void Start()
        {
            int hour = Int32.Parse(TbkHour.Text);
            int minute = Int32.Parse(TbkMinute.Text);
            int second = Int32.Parse(TbkSecond.Text);

            if (hour <= 0 && minute <= 0 && second <= 0)
                return;

            _tsCountDownFrom = new TimeSpan(hour, minute, second);
            _currentTime = _tsCountDownFrom;
            _started = true;
            _dispatcherTimer.Start();
            
            BtnStartStop.Content = AppResources.ButtonStop;
            BtnStartStop.Background = Common.RedColorBrush;

            // Set slider to 0
            slider.CurrentValue = 0;

            // Restore time color
            SelectedTextBlock.Foreground = _defaultColorBrush;
            //TbkMinute.Foreground = _defaultColorBrush;
            //TbkSecond.Foreground = _defaultColorBrush;
            
            // Hide +/- buttons
            //ButtonnAddTime.Visibility = _started ? Visibility.Collapsed : Visibility.Visible;
            //ButtonSubstractTime.Visibility = _started ? Visibility.Collapsed : Visibility.Visible;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        private void Stop()
        {
            _dispatcherTimer.Stop();
            _started = false;
            BtnStartStop.Content = AppResources.ButtonStart;
            BtnStartStop.Background = Common.GreenColorBrush;

            // Show +/- buttons
            //ButtonnAddTime.Visibility = _started ? Visibility.Collapsed : Visibility.Visible;
            //ButtonSubstractTime.Visibility = _started ? Visibility.Collapsed : Visibility.Visible;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        private void TimesUp()
        {
            Stop();
        }

        
        void dispatcherTimer_Tick(object sender, object e)
        {
            _currentTime = _currentTime.Subtract(_oneSecond);
            var ts = _currentTime;
            
            if (ts.Seconds < 0)
                TimesUp();
            else
                DisplayTime(ts.Hours, ts.Minutes, ts.Seconds);
        }
        
        private void DisplayTime(int hours, int minutes, int seconds)
        {
            TbkHour.Text = hours.ToString("00");
            TbkMinute.Text = minutes.ToString("00");
            TbkSecond.Text = seconds.ToString("00");
        }

        private void SliderValueChanged(object sender, SliderValueChangedEventArgs e)
        {
            if (_started)
                return;

            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    SelectedTextBlock.Text = slider.CurrentValue.ToString("00");
                });
            }
            catch (NullReferenceException)
            {
            }
        }

        public TextBlock SelectedTextBlock { get; set; }
        
        private void TbkTime_Tap(object sender, GestureEventArgs e)
        {
            var tbk = sender as TextBlock;
            if (tbk == null) return;
            
            SelectedTextBlock = tbk;
            tbk.Foreground = Common.DefaultForegroundColor;
            slider.CurrentValue = Int32.Parse(tbk.Text);
            switch (tbk.Name)
            {
                case "TbkHour":
                    TbkMinute.Foreground = _defaultColorBrush;
                    TbkSecond.Foreground = _defaultColorBrush;
                    slider.MaximumValue = 24;
                    break;
                case "TbkMinute":
                    TbkHour.Foreground = _defaultColorBrush;
                    TbkSecond.Foreground = _defaultColorBrush;
                    slider.MaximumValue = 60;
                    break;
                case "TbkSecond":
                    TbkHour.Foreground = _defaultColorBrush;
                    TbkMinute.Foreground = _defaultColorBrush;
                    slider.MaximumValue = 60;
                    break;
            }
        }

        private void ButtonnAddTime_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTextBlock == null || _started)
                return;

            int currentValue = Int32.Parse(SelectedTextBlock.Text);
            int newValue = 0;
            switch (SelectedTextBlock.Name)
            {
                case "TbkHour":
                    if (currentValue < 23)
                        newValue = currentValue + 1;
                    break;
                case "TbkMinute":
                case "TbkSecond":
                    if (currentValue < 59)
                        newValue = currentValue + 1;
                    break;
            }
            SelectedTextBlock.Text = newValue.ToString("00");
            slider.CurrentValue = newValue;
        }

        private void ButtonSubstractTime_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTextBlock == null || _started)
                return;

            int currentValue = Int32.Parse(SelectedTextBlock.Text);
            int newValue = 0;
            switch (SelectedTextBlock.Name)
            {
                case "TbkHour":
                    if (currentValue > 0)
                        newValue = currentValue - 1;
                    else
                        newValue = 23;
                    break;
                case "TbkMinute":
                case "TbkSecond":
                    if (currentValue > 0)
                        newValue = currentValue - 1;
                    else
                        newValue = 59;
                    break;
            }
            SelectedTextBlock.Text = newValue.ToString("00");
            slider.CurrentValue = newValue;
        }
    }
}