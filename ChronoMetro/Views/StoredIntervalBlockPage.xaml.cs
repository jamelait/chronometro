using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ChronoMetro.Business;
using ChronoMetro.Resources;
using ChronoMetro.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChronoMetro.Views
{
    public partial class StoredIntervalBlockPage : PhoneApplicationPage
    {
        private StoredIntervalBlockViewModel _viewModel;
        
        public StoredIntervalBlockPage()
        {
            InitializeComponent();
            BuildApplicationBar();
            _viewModel = new StoredIntervalBlockViewModel();
            this.DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Guid guid = (Guid)((Button) sender).Tag;
            var uri = new Uri(Pages.IntervalPage.OriginalString + "?BlockSelected=" + guid, UriKind.Relative);
            // add paramter ?BlockSelected=true
            NavigationService.RemoveBackEntry(); // close previous page (old intervalpage)
            NavigationService.Navigate(uri);
        }

        private void SelectionEnabled_Click(object sender, EventArgs e)
        {
            _viewModel.IsSelectionEnabled = !_viewModel.IsSelectionEnabled;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            _viewModel.RemoveItems(BlockList.SelectedItems);
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // AddBlock
            var select = new ApplicationBarIconButton(Images.List) { Text = AppResources.ButtonList };
            select.Click += SelectionEnabled_Click;
            ApplicationBar.Buttons.Add(select);

            // AddBlock
            var delete = new ApplicationBarIconButton(Images.Delete) { Text = AppResources.ButtonDelete };
            delete.Click += Delete_Click;
            ApplicationBar.Buttons.Add(delete);
        }
    }
}