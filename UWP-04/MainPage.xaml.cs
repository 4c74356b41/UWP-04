﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = Colors.White;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressRing.IsActive = true;
                ProgressRing.Visibility = Visibility.Visible;
                ToggleMe.Visibility = Visibility.Collapsed;
                ToggleMeTwo.Visibility = Visibility.Collapsed;

                var position = await LocationManager.GetPosition();

                WeatherApiProxy.RootObjectApi myWeatherForecast =
                    await WeatherApiProxy.GetWeather(position.Coordinate.Latitude, position.Coordinate.Longitude);

                string icon0 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[0].icon);
                Day0i.Source = new BitmapImage(new Uri(icon0, UriKind.Absolute));
                string icon1 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[1].icon);
                Day1i.Source = new BitmapImage(new Uri(icon1, UriKind.Absolute));
                string icon2 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[2].icon);
                Day2i.Source = new BitmapImage(new Uri(icon2, UriKind.Absolute));
                string icon3 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[3].icon);
                Day3i.Source = new BitmapImage(new Uri(icon3, UriKind.Absolute));
                string icon4 = String.Format("ms-appx:///Assets/{0}.png", myWeatherForecast.forecastlist[4].icon);
                Day4i.Source = new BitmapImage(new Uri(icon4, UriKind.Absolute));

                City.Text = myWeatherForecast.city;
                Day0.Text = "°" + (myWeatherForecast.forecastlist[0].temp).ToString() + ", " + myWeatherForecast.forecastlist[0].descr;

                Day1d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(1));
                Day2d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(2));
                Day3d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(3));
                Day4d.Text = string.Format("{0:dd/MM}", DateTime.Today.AddDays(4));

                Day1t.Text = "°" + (myWeatherForecast.forecastlist[1].temp).ToString();
                Day2t.Text = "°" + (myWeatherForecast.forecastlist[2].temp).ToString();
                Day3t.Text = "°" + (myWeatherForecast.forecastlist[3].temp).ToString();
                Day4t.Text = "°" + (myWeatherForecast.forecastlist[4].temp).ToString();

                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
                ToggleMe.Visibility = Visibility.Visible;
                ToggleMeTwo.Visibility = Visibility.Visible;

                myWeatherForecast = null;
            }
            catch
            {
                Day0.Text = "Unable to get weather at this time, please try again later.";
                City.Text = "";

                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
            }
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
