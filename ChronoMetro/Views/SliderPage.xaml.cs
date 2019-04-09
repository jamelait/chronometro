using System;
using System.Windows;
using System.Windows.Controls;
using ChronoMetro.UserControls;
using ChronoMetro.ViewModels;
using Microsoft.Phone.Controls;
using SubsonicDesign;

namespace ChronoMetro.Views
{
    public partial class SliderPage : PhoneApplicationPage
    {
        private SimpleViewModel _viewModel;
        public SliderPage()
        {
            InitializeComponent();

            //var radialSlider = new RadialSlider();
            //radialSlider.Height = 200;
            //radialSlider.Width = radialSlider.Height;
            //radialSlider.MaximumValue = 60;
            //radialSlider.MinimumValue = 0;

            //radialSlider.ManipulationCompleted += radialSlider_ManipulationCompleted;

            //ContentPanel.Children.Add(radialSlider);

            _viewModel = new SimpleViewModel();
            DataContext = _viewModel;
        }

        //void radialSlider_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        //{
            
        //}

        private void sliderValueChanged(object sender, SliderValueChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            var tbx = new PhoneTextBox()
            {
                Hint = "Titre",
                Margin = new Thickness(0, 14, 0, -2)
            };
            
            var uc = new TimeLabeledRadialSlider();

            TiltEffect.SetIsTiltEnabled(tbx, true);

            var messageBox = new CustomMessageBox()
            {
                Caption = "Donnez un titre à ce temps",
                Content = uc,
                LeftButtonContent = "ok",
                RightButtonContent = "annuler",
                IsFullScreen = false
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        var b = (Button) sender;
                        b.Content = uc.GetTimeValue().ToString();
                        //var saved = _chronometerStore.SaveNew(tbx.Text, _stopWatch.Elapsed);
                        //var message = saved ? "Le temps a été enregistré." : "Une erreur est survenue.";
                        //Popup(message); // confirmation
                        break;
                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                    default:
                        break;
                }
            };

            messageBox.Show();
            //*/
        }
    }
}