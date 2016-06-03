using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace judu
{
    /// <summary>
    /// 用于响应式UI的网页显示
    /// </summary>
    public sealed partial class MinWindow : Page
    {
        public MinWindow()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.Uri = e.Parameter as Models.UserInfo;
            this.MyWebView.Navigate(new Uri(e.Parameter as string));

            if (Frame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                Frame.GoBack();
            } else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            ((App)(App.Current)).BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ((App)App.Current).BackRequested -= OnBackRequested;
        }
    }
}
