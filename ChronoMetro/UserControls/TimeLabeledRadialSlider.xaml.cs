using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using ChronoMetro.Business;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ChronoMetro.UserControls
{
    public partial class TimeLabeledRadialSlider : UserControl, INotifyPropertyChanged
    {
        private double _textFontSize;

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
        }

        #endregion

        //private string _hourValue;
        //private string _minuteValue;
        //private string _secondValue;

        private Brush ForegroundDefaultTextTime { get; set; }
        private Brush ForegroundSelectedTextTime { get; set; }

        //public String HourValue
        //{
        //    get { return _hourValue; }
        //    set { this.SetProperty(ref _hourValue, value); }
        //}

        //public String MinuteValue
        //{
        //    get { return _minuteValue; }
        //    set { this.SetProperty(ref _minuteValue, value); }
        //}

        //public String SecondValue
        //{
        //    get { return _secondValue; }
        //    set { this.SetProperty(ref _secondValue, value); }
        //}

        //private TimeSpan TimeValue { get; set; }

        public double TextFontSize
        {
            get 
            { 
                if (_textFontSize == 0)
                _textFontSize = 100;
                return _textFontSize;
            }
            set { this.SetProperty(ref _textFontSize, value); }
        }



        private TextBlock SelectTextTime { get; set; }
        public TimeLabeledRadialSlider()
        {
            InitializeComponent();
            this.DataContext = this;
            Slider.SliderValueChanged += SliderValueChanged;
            ForegroundDefaultTextTime = TextHour.Foreground;
            ForegroundSelectedTextTime = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
            TextTime_Tap(TextMinute, null);
        }

        void SliderValueChanged(object sender, SubsonicDesign.SliderValueChangedEventArgs e)
        {
            //return;
            //if (SelectTextTime == null)
            //    return;

            //SelectTextTime.Text = e.NewValue.ToString("00");
            //SelectTextTime.Text = Slider.CurrentValue.ToString("00");
            if (Slider != null)
                SetValue(Slider.CurrentValue);

            return;
            if (SelectTextTime == TextHour)
            {
                //TimeValue = new TimeSpan((int) e.NewValue, TimeValue.Minutes, TimeValue.Seconds);
                //HourValue = e.NewValue.ToString(CultureInfo.InvariantCulture);
                TextHour.Text = e.NewValue.ToString("00");
            }
            else if (SelectTextTime == TextMinute)
            {
                //TimeValue = new TimeSpan(TimeValue.Hours, (int) e.NewValue, TimeValue.Seconds);
                TextMinute.Text = e.NewValue.ToString("00");
            }
            else if (SelectTextTime == TextSecond)
            {
                //TimeValue = new TimeSpan(TimeValue.Hours, TimeValue.Minutes, (int)e.NewValue);
                TextSecond.Text = e.NewValue.ToString("00");
            }
        }

        private void SetValue(double value)
        {
            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    SelectTextTime.Text = value.ToString("00");
                });
            }
            catch (NullReferenceException)
            {
            }
            
        }

        private void TextTime_Tap(object sender, GestureEventArgs e)
        {
            SelectTextTime = (TextBlock) sender;
            
            // Default foreground color
            TextHour.Foreground = ForegroundDefaultTextTime;
            TextMinute.Foreground = ForegroundDefaultTextTime;
            TextSecond.Foreground = ForegroundDefaultTextTime;

            // Highlited foreground color
            SelectTextTime.Foreground = ForegroundSelectedTextTime;

            if (SelectTextTime == TextHour)
            {
                Slider.MaximumValue = 23;
            }
            else if (SelectTextTime == TextMinute)
            {
                Slider.MaximumValue = 59;
            }
            else if (SelectTextTime == TextSecond)
            {
                Slider.MaximumValue = 59;
            }

            Slider.CurrentValue = Double.Parse(SelectTextTime.Text);
        }

        public TimeSpan GetTimeValue()
        {
            //return new TimeSpan(Int32.Parse(HourValue), Int32.Parse(MinuteValue), Int32.Parse(SecondValue));
            return new TimeSpan(Int32.Parse(TextHour.Text), Int32.Parse(TextMinute.Text), Int32.Parse(TextSecond.Text));
        }

        public void SetTimeValue(TimeSpan value)
        {
            TextHour.Text = value.Hours.ToString("00");
            TextMinute.Text = value.Minutes.ToString("00");
            TextSecond.Text = value.Seconds.ToString("00");

            if (SelectTextTime != null)
                TextTime_Tap(SelectTextTime, null);
        }

        private void ButtonnAddTime_Click(object sender, RoutedEventArgs e)
        {
            var value = Double.Parse(SelectTextTime.Text) + 1;
            if (value > Slider.MaximumValue)
                value = Slider.MinimumValue;
            Slider.CurrentValue = value;
        }

        private void ButtonSubstractTime_Click(object sender, RoutedEventArgs e)
        {
            var value = Double.Parse(SelectTextTime.Text) - 1;
            Slider.CurrentValue = value;
        }
    }
}
