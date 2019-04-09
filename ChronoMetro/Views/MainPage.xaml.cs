using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ChronoMetro.Data;
using ChronoMetro.Resources;
using Microsoft.Phone.Controls;

namespace ChronoMetro.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        public MainPage()
        {
            InitializeComponent();

            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var userData = UserData.GetUserData();
            if (userData.AppLaunchCount == 1)
                FirstStartPopup();
        }

        public void FirstStartPopup()
        {
            var link = new HyperlinkButton
            {
                Content = AppResources.VisitWebsite,
                NavigateUri = new Uri(AppResources.Website, UriKind.Absolute),
            };

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = AppResources.FirstStartCaption,
                Message = AppResources.FirstStartMessage,
                Content = link,
                IsRightButtonEnabled = false,
                LeftButtonContent = "ok",
            };

            messageBox.Show();
        }

        private void ButtonChronometer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.ChronometerPage);
        }

        private void ButtonCountdown_Click(object sender, RoutedEventArgs e)
        {
            //App.RootFrame.Navigate(Pages.SliderPage);
            NavigationService.Navigate(Pages.CountdownPage);
        }

        private void ButtonInterval_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Pages.IntervalPage);
        }
        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            //App.RootFrame.Navigate(Pages.SliderPage);
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
            //NavigationService.Navigate(Pages.SliderPage);

        }

        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}