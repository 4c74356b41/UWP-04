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
                var position = await LocationManager.GetPosition();

                OpenWeatherMapProxyForecast.RootObjectForecast myWeatherForecast =
                    await OpenWeatherMapProxyForecast.GetWeatherForecast(position.Coordinate.Latitude, position.Coordinate.Longitude);

                string icon = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.list[0].weather[0].icon);
                ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
                City.Text = myWeatherForecast.city.name;
                Celcius.Text = "Temperatura: °" + ((int)myWeatherForecast.list[0].main.temp).ToString();
                Weather.Text = "Como hace hoy: " + myWeatherForecast.list[0].weather[0].description;

                TomorrowTemp.Text = ((int)myWeatherForecast.list[8].main.temp).ToString();
                TomorrowMain.Text = myWeatherForecast.list[8].weather[0].main;

                AfterTomorrowTemp.Text = ((int)myWeatherForecast.list[16].main.temp).ToString();
                AfterTomorrowMain.Text = myWeatherForecast.list[16].weather[0].main;

                AfterAfterTomorrowTemp.Text = ((int)myWeatherForecast.list[24].main.temp).ToString();
                AfterAfterTomorrowMain.Text = myWeatherForecast.list[24].weather[0].main;

                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
                Refresh.Visibility = Visibility.Visible;

            }
            catch
            {
                Weather.Text = "Unable to get weather at this time, please try again later.";
                City.Text = "Something went wrong :(";
                ProgressRing.Visibility = Visibility.Collapsed;
                Refresh.Visibility = Visibility.Visible;
            }
        }
    }
}
