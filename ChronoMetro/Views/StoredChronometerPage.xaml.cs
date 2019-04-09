using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ChronoMetro.Business;
using ChronoMetro.Data.Chronometer;
using ChronoMetro.Resources;
using ChronoMetro.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChronoMetro.Views
{
    public partial class StoredChronometerPage : PhoneApplicationPage
    {
        private StoredChronometerViewModel _viewModel;
        public StoredChronometerPage()
        {
            InitializeComponent();
            
            BuildApplicationBar();

            _viewModel = new StoredChronometerViewModel();

            DataContext = _viewModel;
        }

        private void SelectionEnabled_Click(object sender, EventArgs e)
        {
            _viewModel.IsSelectionEnabled = !_viewModel.IsSelectionEnabled;
        }

        private void ChronoList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            _viewModel.RemoveItems(ChronoList.SelectedItems);
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // AddBlock
            var select = new ApplicationBarIconButton(Images.List) { Text = AppResources.ButtonSelect };
            select.Click += SelectionEnabled_Click;
            ApplicationBar.Buttons.Add(select);

            // AddBlock
            var delete = new ApplicationBarIconButton(Images.Delete) { Text = AppResources.ButtonDelete };
            delete.Click += Delete_Click;
            ApplicationBar.Buttons.Add(delete);
        }

    }
}