using System;
using System.Collections.Generic;
using UWP_04.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UWP_04
{
    public sealed partial class Weather : Page
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

        public Weather()
        {
            InitializeComponent();
            //Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Colors.Black;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                toggleUIWhileLoading(true);
                var myWeatherForecast = new WeatherApiProxy.RootObjectApi();

                if ((Application.Current as App).cityFind != null)
                {
                    switch ((Application.Current as App).cityFind)
                    {
                        case "Barcelona":
                            myWeatherForecast = await WeatherApiProxy.GetWeather("41.39", "2.158");
                            break;
                        case "Irakleion":
                            myWeatherForecast = await WeatherApiProxy.GetWeather("35.33", "25.14");
                            break;
                        case "Vienna":
                            myWeatherForecast = await WeatherApiProxy.GetWeather("48.21", "16.38");
                            break;
                        case "Toronto":
                            myWeatherForecast = await WeatherApiProxy.GetWeather("43.70", "-79.4");
                            break;
                        default:
                            myWeatherForecast = await WeatherApiProxy.GetWeather("55.75", "37.61");
                            break;
                    }
                }
                else
                {
                    var position = await LocationManager.GetPosition();
                    if (position == null)
                    {
                        myWeatherForecast = await WeatherApiProxy.GetWeather("0.000", "0.000");
                    }
                    else
                    {
                        myWeatherForecast = await WeatherApiProxy.GetWeather
                            (position.Coordinate.Latitude.ToString(),
                            position.Coordinate.Longitude.ToString());
                    }
                }

                City.Text = (Application.Current as App).cityTile = myWeatherForecast.city;

                int counter = 0;
                foreach (Image Img in new Image[] { Day0i, Day1i, Day2i, Day3i, Day4i })
                {
                    Img.Source = new BitmapImage(new Uri(string.Format("ms-appx:///Assets/{0}.png",
                        myWeatherForecast.forecastlist[counter].icon), UriKind.Absolute));

                    counter++;
                }

                counter = 1;
                foreach (TextBlock TBd in new TextBlock[] { Day1d, Day2d, Day3d, Day4d })
                {
                    TBd.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(counter));
                    counter++;
                }

                string modifier;
                List<int> tempList = new List<int>();
                switch ((Application.Current as App).tempFormat)
                {
                    case "Kelvin":
                        modifier = "K";
                        for (int i = 0; i < 5; i++)
                        {
                            tempList.Add(273 + myWeatherForecast.forecastlist[i].temp);
                        }
                        break;
                    case "Fahrenheit":
                        modifier = "F";
                        for (int i = 0; i < 5; i++)
                        {
                            tempList.Add(myWeatherForecast.forecastlist[i].temp * 9 / 5 + 32);
                        }
                        break;
                    default:
                        modifier = "°";
                        for (int i = 0; i < 5; i++)
                        {
                            tempList.Add(myWeatherForecast.forecastlist[i].temp);
                        }
                        break;
                }

                Day0.Text = tempList[0].ToString() + modifier + ", " + myWeatherForecast.forecastlist[0].descr;
                counter = 0;
                foreach (TextBlock TBi in new TextBlock[] { Day1t, Day2t, Day3t, Day4t })
                {
                    TBi.Text = tempList[counter].ToString();
                    counter++;
                }

                toggleUIWhileLoading(false);
                myWeatherForecast = null;
            }
            catch (Exception error)
            {
                throwError(error.Message.ToString());
            }
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.IsSelected)
            {
                Frame.Navigate(typeof(Settings));
            }
        }

        public void toggleUIWhileLoading(bool toggleStuff)
        {
            if (toggleStuff)
            {
                Error.Visibility = Visibility.Collapsed;
                ProgressRing.IsActive = true;
                ProgressRing.Visibility = Visibility.Visible;
                ToggleMe.Visibility = Visibility.Collapsed;
                ToggleMeTwo.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
                ToggleMe.Visibility = Visibility.Visible;
                ToggleMeTwo.Visibility = Visibility.Visible;
            }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Restore values stored in session state.
            if (e.PageState != null && e.PageState.ContainsKey("tempValue"))
            {
                switch (e.PageState["tempValue"].ToString())
                {
                    case "Celsius":
                        (Application.Current as App).tempFormat = "Celsius";
                        break;
                    case "Kelvin":
                        (Application.Current as App).tempFormat = "Kelvin";
                        break;
                    case "Fahrenheit":
                        (Application.Current as App).tempFormat = "Fahrenheit";
                        break;
                }
            }
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("tempValue"))
            {
                switch (roamingSettings.Values["tempValue"].ToString())
                {
                    case "Celsius":
                        (Application.Current as App).tempFormat = "Celsius";
                        break;
                    case "Kelvin":
                        (Application.Current as App).tempFormat = "Kelvin";
                        break;
                    case "Fahrenheit":
                        (Application.Current as App).tempFormat = "Fahrenheit";
                        break;
                }
            }

            if (e.PageState != null && e.PageState.ContainsKey("citySelected"))
            {
                (Application.Current as App).cityFind = roamingSettings.Values["citySelected"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("citySelected"))
            {
                (Application.Current as App).cityFind = roamingSettings.Values["citySelected"].ToString();
            }
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        public void throwError(string errorText = "please try again later")
        {
            Error.Text = errorText;
            Error.Visibility = Visibility.Visible;
            ProgressRing.IsActive = false;
            ProgressRing.Visibility = Visibility.Collapsed;
        }
    }
}
