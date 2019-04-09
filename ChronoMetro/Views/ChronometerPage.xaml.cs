using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ChronoMetro.Business;
using ChronoMetro.Data.Chronometer;
using ChronoMetro.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;

namespace ChronoMetro.Views
{
    public partial class ChronometerPage : PhoneApplicationPage
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private Stopwatch _stopWatch;
        private bool _started;
        private ChronometerStore _chronometerStore;

        //public ObservableCollection<Lap> Laps { get; set; }

        public ChronometerPage()
        {
            InitializeComponent();
            
            BuildApplicationBar();

            _chronometerStore = ChronometerStore.Prepare();

            //var v = DeviceExtendedProperties.GetValue("DeviceUniqueId") as byte[]; // Needs ID_CAP_IDENTITY_DEVICE enabled
            //var str = System.Text.Encoding.BigEndianUnicode.GetString(v,0, v.Length);

            //byte[] id = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            //string deviceID = Convert.ToBase64String(id);

            _started = false;

            //  DispatcherTimer setup
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (_started) // Chronometer running
                Stop();
            else
                Start();
        }

        private void Start()
        {
            _started = true;
            if (_stopWatch == null)
                _stopWatch = Stopwatch.StartNew();

            _dispatcherTimer.Start();
            _stopWatch.Start();

            BtnStartStop.Background = Common.RedColorBrush;
            BtnStartStop.Content = AppResources.ButtonStop;

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        private void Stop()
        {
            _started = false;
            _dispatcherTimer.Stop();
            _stopWatch.Stop();

            BtnStartStop.Background = Common.GreenColorBrush;
            BtnStartStop.Content = AppResources.ButtonStart;

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            TbkTime.Text = Common.GetReadableTime(_stopWatch.Elapsed.Hours, _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds);
        }

        private void BtnnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_started)
                Stop();
            _stopWatch = null;

            TbkTime.Text = Common.GetReadableTime(0, 0, 0);
        }

        //private void ButtonLap_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!_started)
        //        return;

        //    Dispatcher.BeginInvoke(() =>
        //    {
        //        Laps.Add(new Lap()
        //        {
        //            Label = "Lap " + Laps.Count,
        //            Time = _stopWatch.Elapsed
        //        });
        //    });
        //}

        private void Save_Click(object sender, EventArgs e)
        {
            if (_started || _stopWatch == null || Math.Abs(_stopWatch.Elapsed.TotalMilliseconds) < 0)
                return;

            var tbx = new TextBox()
            {
                Margin = new Thickness(0, 14, 0, -2)
            };

            var messageBox = new CustomMessageBox()
            {
                Caption = AppResources.StopwatchSaveNotice,
                Content = tbx,
                LeftButtonContent = AppResources.ButtonOk,
                RightButtonContent = AppResources.ButtonCancel,
                IsFullScreen = false
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        var saved = _chronometerStore.SaveNew(tbx.Text, _stopWatch.Elapsed);
                        break;
                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                    default:
                        break;
                }
            };

            messageBox.Show();
        }

        private void List_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(Pages.StoredChronometerPage);
        }
        
        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var appBarButtonSave = new ApplicationBarIconButton(Images.Save) { Text = AppResources.ButtonSave };
            appBarButtonSave.Click += Save_Click;
            ApplicationBar.Buttons.Add(appBarButtonSave);

            var appBarButtonList = new ApplicationBarIconButton(Images.List) { Text = AppResources.ButtonList };
            appBarButtonList.Click += List_Click;
            ApplicationBar.Buttons.Add(appBarButtonList);
        }
    }
}