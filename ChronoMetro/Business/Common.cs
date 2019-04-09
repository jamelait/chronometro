using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Color = System.Windows.Media.Color;

namespace ChronoMetro.Business
{
    public static class Common
    {
        //_redColorBrush = new SolidColorBrush(Color.FromArgb(255, 124, 36, 36));
        //_greenColorBrush = new SolidColorBrush(Color.FromArgb(255, 42, 121, 53));
        //public static readonly SolidColorBrush GreenColorBrush = new SolidColorBrush(Color.FromArgb(255, 36, 184, 87));
        public static readonly SolidColorBrush DefaultForegroundColor = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);

        public static readonly SolidColorBrush GreenColorBrush = new SolidColorBrush(Color.FromArgb(255, 36, 124, 48));
        public static readonly SolidColorBrush RedColorBrush = new SolidColorBrush(Color.FromArgb(255, 252, 67, 51));

        // Interval
        public static readonly SolidColorBrush BlueColorBrush = new SolidColorBrush(Color.FromArgb(255, 42, 186, 228));
        public static readonly SolidColorBrush GrayColorBrush = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
        public static readonly SolidColorBrush DarkGrayColorBrush = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90));
        public static readonly SolidColorBrush BlackColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        public static readonly String AlertSound1 = "Sounds/beep-07.wav";
        //public static readonly String AlertSound2 = "Sounds/Windows_Foreground.wav";

        public static readonly String UserDataStore = "UserData.store";
        public static readonly String ChronometerStore = "Chronometers.store";
        public static readonly String IntervalBlockStore = "IntervalBlocks.store";





        public static string GetReadableTime(int hour, int minute, int second)
        {
            return String.Format("{0}:{1}:{2}", hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
        }

        public static string GetReadableTime(TimeSpan timeSpan)
        {
            return GetReadableTime(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static string GetAppVersion()
        {
            String appVersion = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0];

            return appVersion;
        }

        public static void PlaySound(string alertSoundPath)
        {
            Stream stream = TitleContainer.OpenStream(alertSoundPath);
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }
    }
}
