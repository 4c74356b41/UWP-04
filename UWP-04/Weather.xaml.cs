using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP_04
{
    public sealed partial class Weather : Page
    {
        public Weather()
        {
            InitializeComponent();
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

                City.Text = (Application.Current as App).cityTile = myWeatherForecast.city;
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
