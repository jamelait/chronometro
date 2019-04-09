using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ChronoMetro.Business;
using ChronoMetro.Data.Interval;
using ChronoMetro.Resources;
using ChronoMetro.UserControls;
using ChronoMetro.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChronoMetro.Views
{
    public partial class IntervalPage : PhoneApplicationPage
    {
        private static readonly TimeSpan DefaultTime = new TimeSpan(0, 0, 10);
        private IntervalPageViewModel _viewModel;
        private StackPanel _addBlockPanel;

        private PhoneTextBox _tbxLabel;
        private TimeLabeledRadialSlider _timeSlider;
        
        public IntervalPage()
        {
            InitializeComponent();
            
            BuildApplicationBar();

            _viewModel = new IntervalPageViewModel();
            DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string query = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("BlockSelected", out query))
            {
                Guid guid = new Guid(query);
                _viewModel.Populate(guid);
                NavigationService.RemoveBackEntry(); // close previous page (list of stored intervalblocks)
            }
        }
        
        private void AddBlock_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }
        
        private StackPanel GetAddBlockPanel()
        {
            var margin = new Thickness(0,0,0,20);
            const double fontSize = 30;
            _addBlockPanel = new StackPanel();

            var lastTimeValue = _timeSlider != null ? _timeSlider.GetTimeValue() : DefaultTime;
            
            var label = new TextBlock
            {
                Text = AppResources.IntervalAddLabel,
                Margin = margin,
                TextWrapping = TextWrapping.Wrap,
                FontSize = fontSize
            };
            _tbxLabel = new PhoneTextBox
            {
                Height = 72,
                Width = 456,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Hint = "Block n°" + (_viewModel.Blocks.Count + 1)
            };
            var tbkTemps = new TextBlock
            {
                Text = AppResources.IntervalAddTime,
                Margin = margin,
                TextWrapping = TextWrapping.Wrap,
                FontSize = fontSize
            };
            _timeSlider = new TimeLabeledRadialSlider
            {
                TextFontSize = 90,
            };
            _timeSlider.SetTimeValue(lastTimeValue);
            _addBlockPanel.Children.Add(label);
            _addBlockPanel.Children.Add(_tbxLabel);
            _addBlockPanel.Children.Add(tbkTemps);
            _addBlockPanel.Children.Add(_timeSlider);

            return _addBlockPanel;
        }

        private void ShowPopup()
        {
            var addBlockMessageBox = new CustomMessageBox()
            {
                Content = GetAddBlockPanel(),
                LeftButtonContent = AppResources.ButtonAdd,
                RightButtonContent = AppResources.ButtonCancel,
                IsFullScreen = true
            };

            addBlockMessageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        var label = String.IsNullOrEmpty(_tbxLabel.Text) ? _tbxLabel.Hint : _tbxLabel.Text;
                        _viewModel.AddBlock(label, _timeSlider.GetTimeValue());
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Do something.
                        break;
                    case CustomMessageBoxResult.None:
                        // Do something.
                        break;
                    default:
                        break;
                }
            };

            addBlockMessageBox.Show();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            bool started = (string)BtnStartStop.Content == AppResources.ButtonStop;
            if (!started)
                _viewModel.Start();
            else
                _viewModel.Stop();

            ApplicationBar.IsVisible = !_viewModel.HasStarted;
        }

        private void BtnnReset_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Reset();
            ApplicationBar.IsVisible = !_viewModel.HasStarted;
        }

        private void RemoveAllBlocks(object sender, EventArgs e)
        {
            _viewModel.Stop();
            _viewModel = new IntervalPageViewModel();
            this.DataContext = _viewModel;
        }

        private void SaveBlocks(object sender, EventArgs e)
        {
            if (!_viewModel.Blocks.Any())
                return;

            var tbx = new TextBox()
            {
                Margin = new Thickness(0, 14, 0, -2)
            };

            var messageBox = new CustomMessageBox()
            {
                Caption = AppResources.IntervalSaveNotice,
                Content = tbx,
                LeftButtonContent = AppResources.ButtonOk,
                RightButtonContent = AppResources.ButtonCancel,
                IsFullScreen = false
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                if (e1.Result == CustomMessageBoxResult.LeftButton)
                    try
                    {
                        _viewModel.Persist(tbx.Text);
                    }
                    catch (Exception)
                    {
                        Popup("Error");
                    }
                    
            };

            messageBox.Show();
        }

        private void ListBlocks(object sender, EventArgs e)
        {
            NavigationService.Navigate(Pages.StoredIntervalBlockPage);
        }
        public void Popup(string message)
        {
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Message = message,
                IsRightButtonEnabled = false,
                LeftButtonContent = "ok",
            };

            messageBox.Show();
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // AddBlock
            var addBlock = new ApplicationBarIconButton(Images.Add) { Text = AppResources.ButtonAdd };
            addBlock.Click += AddBlock_Click;
            ApplicationBar.Buttons.Add(addBlock);

            // RemoveAllBlocks
            var removeAllBlocks = new ApplicationBarMenuItem(AppResources.ButtonNew);
            removeAllBlocks.Click += RemoveAllBlocks;
            ApplicationBar.MenuItems.Add(removeAllBlocks);
            // SaveBlocks
            var saveBlocks = new ApplicationBarMenuItem(AppResources.ButtonSave);
            saveBlocks.Click += SaveBlocks;
            ApplicationBar.MenuItems.Add(saveBlocks);
            // ListBlocks
            var listBlocks = new ApplicationBarMenuItem(AppResources.ButtonList);
            listBlocks.Click += ListBlocks;
            ApplicationBar.MenuItems.Add(listBlocks);
        }
    }
}