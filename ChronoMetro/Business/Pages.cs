using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoMetro.Views
{
    public static class Pages
    {
        private const String RootFolder = "/Views/";
        
        public static readonly Uri ChronometerPage = new Uri(RootFolder + "ChronometerPage.xaml", UriKind.Relative);
        public static readonly Uri CountdownPage = new Uri(RootFolder + "CountdownPage.xaml", UriKind.Relative);
        public static readonly Uri IntervalPage = new Uri(RootFolder + "IntervalPage.xaml", UriKind.Relative);
        public static readonly Uri YourLastAboutDialog = new Uri("YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative);
        public static readonly Uri SliderPage = new Uri(RootFolder + "SliderPage.xaml", UriKind.Relative);
        public static readonly Uri MainPage = new Uri(RootFolder + "MainPage.xaml", UriKind.Relative);
        public static readonly Uri StoredChronometerPage = new Uri(RootFolder + "StoredChronometerPage.xaml", UriKind.Relative);
        public static readonly Uri StoredIntervalBlockPage = new Uri(RootFolder + "StoredIntervalBlockPage.xaml", UriKind.Relative);
    }
}
