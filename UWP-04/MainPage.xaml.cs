using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Refresh.Visibility = Visibility.Collapsed;
                ProgressRing.IsActive = true;
                ProgressRing.Visibility = Visibility.Visible;
                ResultImage.Visibility = Visibility.Collapsed;
                TomorrowMain.Visibility = Visibility.Collapsed;
                AfterTomorrowMain.Visibility = Visibility.Collapsed;
                AfterAfterTomorrowMain.Visibility = Visibility.Collapsed;
                TomorrowTemp.Visibility = Visibility.Collapsed;
                AfterTomorrowTemp.Visibility = Visibility.Collapsed;
                AfterAfterTomorrowTemp.Visibility = Visibility.Collapsed;

                var position = await LocationManager.GetPosition();

                WeatherApiProxy.RootObjectApi myWeatherForecast =
                    await WeatherApiProxy.GetWeather(position.Coordinate.Latitude, position.Coordinate.Longitude);

                string iconBig = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[0].icon);
                ResultImage.Source = new BitmapImage(new Uri(iconBig, UriKind.Absolute));
                string icon1 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[1].icon);
                TomorrowMain.Source = new BitmapImage(new Uri(icon1, UriKind.Absolute));
                string icon2 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[2].icon);
                AfterAfterTomorrowMain.Source = new BitmapImage(new Uri(icon2, UriKind.Absolute));
                string icon3 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[3].icon);
                AfterTomorrowMain.Source = new BitmapImage(new Uri(icon3, UriKind.Absolute));

                City.Text = myWeatherForecast.city;
                Weather.Text = "°" + (myWeatherForecast.forecastlist[0].temp).ToString() + ", " + myWeatherForecast.forecastlist[0].descr;
                ForecastTime.Text = myWeatherForecast.time;

                TomorrowTemp.Text = "°" + (myWeatherForecast.forecastlist[1].temp).ToString();
                AfterTomorrowTemp.Text = "°" + (myWeatherForecast.forecastlist[2].temp).ToString();
                AfterAfterTomorrowTemp.Text = "°" + (myWeatherForecast.forecastlist[3].temp).ToString();

                //TomorrowMain.Text = myWeatherForecast.forecastlist[1].descr;
                //AfterTomorrowMain.Text = myWeatherForecast.forecastlist[2].descr;
                //AfterAfterTomorrowMain.Text = myWeatherForecast.forecastlist[3].descr;

                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
                Refresh.Visibility = Visibility.Visible;
                ResultImage.Visibility = Visibility.Visible;
                TomorrowMain.Visibility = Visibility.Visible;
                AfterTomorrowMain.Visibility = Visibility.Visible;
                AfterAfterTomorrowMain.Visibility = Visibility.Visible;
                TomorrowTemp.Visibility = Visibility.Visible;
                AfterTomorrowTemp.Visibility = Visibility.Visible;
                AfterAfterTomorrowTemp.Visibility = Visibility.Visible;

                myWeatherForecast = null;

            }
            catch
            {
                Weather.Text = "Unable to get weather at this time, please try again later.";
                City.Text = "Something went wrong :(";
                ForecastTime.Visibility = Visibility.Collapsed;
                ProgressRing.Visibility = Visibility.Collapsed;
                Refresh.Visibility = Visibility.Visible;
            }
        }
    }
}
