using System;
using System.ComponentModel;
using System.Linq;
using UWP_04.Common;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP_04
{
    public sealed partial class Settings : Page
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
        }

        private NavigationHelper navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public Settings()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void tswitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (tswitch.IsOn)
            {
                UpdateLiveTile((Application.Current as App).cityTile);
            }
            else
            {
                RemoveLiveTile();
            }

            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["tswitchValue"] = tswitch.IsOn;
        }

        private void UpdateLiveTile(string city)
        {
            var offset = DateTime.Now - DateTime.UtcNow;
            var uri = string.Format("http://weatherap1.azurewebsites.net/tile/?city={0}&offset={1}", city, offset.Hours.ToString());

            var tileContent = new Uri(uri);
            var requestedInterval = PeriodicUpdateRecurrence.SixHours;

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(tileContent, requestedInterval);
        }

        private void RemoveLiveTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().StopPeriodicUpdate();
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeatherList.IsSelected)
            {
                Frame.Navigate(typeof(Weather));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Weather));
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            bool tempbool;
            // Restore values stored in session state.
            if (e.PageState != null && e.PageState.ContainsKey("tswitchValue"))
            {
                bool.TryParse(e.PageState["tswitchValue"].ToString(), out tempbool);
                tswitch.IsOn = tempbool;
            }

            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("tswitchValue"))
            {
                bool.TryParse(roamingSettings.Values["tswitchValue"].ToString(), out tempbool);
                tswitch.IsOn = tempbool;
            }

            if (e.PageState != null && e.PageState.ContainsKey("tempValue"))
            {
                switch (e.PageState["tempValue"].ToString())
                {
                    case "Celsius":
                        ComboBox.SelectedItem = Celsius;
                        break;
                    case "Kelvin":
                        ComboBox.SelectedItem = Kelvin;
                        break;
                    case "Fahrenheit":
                        ComboBox.SelectedItem = Fahrenheit;
                        break;
                }
            }

            if (roamingSettings.Values.ContainsKey("tempValue"))
            {
                switch (roamingSettings.Values["tempValue"].ToString())
                {
                    case "Celsius":
                        ComboBox.SelectedItem = Celsius;
                        break;
                    case "Kelvin":
                        ComboBox.SelectedItem = Kelvin;
                        break;
                    case "Fahrenheit":
                        ComboBox.SelectedItem = Fahrenheit;
                        break;
                }
            }

            if (e.PageState != null && e.PageState.ContainsKey("citySelected"))
            {
                asb.Text = (Application.Current as App).cityFind = roamingSettings.Values["citySelected"].ToString();
            }

            if (roamingSettings.Values.ContainsKey("citySelected"))
            {
                asb.Text = (Application.Current as App).cityFind = roamingSettings.Values["citySelected"].ToString();
            }
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["tswitchValue"] = tswitch.IsOn;
            e.PageState["tempValue"] = (Application.Current as App).tempFormat;
            e.PageState["citySelected"] = (Application.Current as App).cityFind;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
               Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (Celsius.IsSelected)
            {
                roamingSettings.Values["tempValue"] = (Application.Current as App).tempFormat = "Celsius";
            }
            else if (Kelvin.IsSelected)
            {
                roamingSettings.Values["tempValue"] = (Application.Current as App).tempFormat = "Kelvin";
            }
            else
            {
                roamingSettings.Values["tempValue"] = (Application.Current as App).tempFormat = "Fahrenheit";
            }
        }

        private void asb_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matchingCities = CitiesSampleSource.GetMatchingCities(sender.Text);
                sender.ItemsSource = matchingCities.ToList();
            }
        }

        private void asb_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                Windows.Storage.ApplicationDataContainer roamingSettings =
                    Windows.Storage.ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["citySelected"] = (Application.Current as App).cityFind
                    = args.ChosenSuggestion.ToString();
            }
            else
            {
                //Do a fuzzy search on the query text
                var matchingContacts = CitiesSampleSource.GetMatchingCities(args.QueryText);
            }
        }

        private void asb_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var city = args.SelectedItem as City;
            sender.Text = string.Format("{0} ", city.CityName);
        }
    }
}
