using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_04.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UWP_04
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //handling status bar
            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = Colors.White;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                toggleUIWhileLoading(true);
                var myWeatherForecast = new WeatherApiProxy.RootObjectApi();
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

                if (myWeatherForecast == null) throw new Exception("No data, try again later");
                //populate "view" with data
                string icon0 = string.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[0].icon);
                Day0i.Source = new BitmapImage(new Uri(icon0, UriKind.Absolute));
                string icon1 = string.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[1].icon);
                Day1i.Source = new BitmapImage(new Uri(icon1, UriKind.Absolute));
                string icon2 = string.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[2].icon);
                Day2i.Source = new BitmapImage(new Uri(icon2, UriKind.Absolute));
                string icon3 = string.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[3].icon);
                Day3i.Source = new BitmapImage(new Uri(icon3, UriKind.Absolute));
                string icon4 = string.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[4].icon);
                Day4i.Source = new BitmapImage(new Uri(icon4, UriKind.Absolute));

                City.Text = myWeatherForecast.city;
                Day0.Text = "°" + (myWeatherForecast.forecastlist[0].temp).ToString()
                    + ", " + myWeatherForecast.forecastlist[0].descr;

                Day1d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(1));
                Day2d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(2));
                Day3d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(3));
                Day4d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(4));

                Day1t.Text = "°" + (myWeatherForecast.forecastlist[1].temp).ToString();
                Day2t.Text = "°" + (myWeatherForecast.forecastlist[2].temp).ToString();
                Day3t.Text = "°" + (myWeatherForecast.forecastlist[3].temp).ToString();
                Day4t.Text = "°" + (myWeatherForecast.forecastlist[4].temp).ToString();

                if ((Application.Current as App).livetile)
                {
                    UpdateLiveTile(myWeatherForecast.city);
                }
                else
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                    TileUpdateManager.CreateTileUpdaterForApplication().StopPeriodicUpdate();
                }
                toggleUIWhileLoading(false);
                myWeatherForecast = null;
            }
            catch (Exception error)
            {
                throwError(error.Message.ToString());
            }
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

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.IsSelected)
            {
                Frame.Navigate(typeof(Settings), (Application.Current as App).livetile);
            }
        }

        public void toggleUIWhileLoading(bool toggleStuff)
        {
            if (toggleStuff)
            {
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

        public void throwError(string errorText = "please try again later")
        {
            City.Text = errorText;
            ProgressRing.IsActive = false;
            ProgressRing.Visibility = Visibility.Collapsed;
        }
    }
}
