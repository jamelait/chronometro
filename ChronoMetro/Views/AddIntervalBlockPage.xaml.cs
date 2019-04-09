using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChronoMetro.Views
{
    public partial class AddIntervalBlockPage : PhoneApplicationPage
    {
        public AddIntervalBlockPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.IntervalPage);
        } 
    }
}