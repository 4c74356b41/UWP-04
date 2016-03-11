using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_04.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_04
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page, INotifyPropertyChanged
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private bool _IsOn;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsOn
        {
            get
            {
                return this._IsOn;
            }

            set
            {
                if (value != this._IsOn)
                {
                    this._IsOn = value;
                    NotifyPropertyChanged("IsOn");
                }
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public Settings()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void tswitch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).livetile = !(Application.Current as App).livetile;
            IsOn = !IsOn;

            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["tswitchValue"] = tswitch.IsOn;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LiveTileList.IsSelected)
            {
                this.Frame.Navigate(typeof(MainPage), (Application.Current as App).livetile);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), (Application.Current as App).livetile);
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            bool tempbool;
            // Restore values stored in session state.
            if (e.PageState != null && e.PageState.ContainsKey("tswitchValue"))
            {
                bool.TryParse(e.PageState["tswitchValue"].ToString(), out tempbool);
                (Application.Current as App).livetile = tempbool;
                tswitch.IsOn = tempbool;
            }

            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("tswitchValue"))
            {
                bool.TryParse(roamingSettings.Values["tswitchValue"].ToString(), out tempbool);
                (Application.Current as App).livetile = tempbool;
                tswitch.IsOn = tempbool;
            }
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["tswitchValue"] = tswitch.IsOn;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

    }
}
