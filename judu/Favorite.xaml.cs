using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace judu
{
    /// <summary>
    /// 收藏夹页面
    /// </summary>
    public sealed partial class Favorite : Page
    {
        public Favorite()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
            UserDB = new Services.UserDatabase();
            F_ViewModel = new ViewModels.MessageViewModel(UserDB.GetFavorite());
        }

        private Services.UserDatabase UserDB { get; set; }
        private ViewModels.MessageViewModel F_ViewModel { get; set; }
        private Models.MessageItem SelectedItem { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ((App)App.Current).BackRequested -= OnBackRequested;
        }
        /// <summary>
        /// 点击Item, 显示相应网页。若当前宽度太小，跳转至新页面显示
        /// </summary>
        private void MessageItemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedItem = (e.ClickedItem as Models.MessageItem);
            
            SelectedItem = (e.ClickedItem as Models.MessageItem);
            if (SelectedItem.guid != null)
            {
                this.MyWebView.Navigate(new Uri(SelectedItem.guid));
            }
            if (WebViewGrid.Visibility == Visibility.Collapsed)
            {
                Frame.Navigate(typeof(MinWindow), SelectedItem.guid);
            }
        }

        /// <summary>
        /// 删除收藏并提示
        /// </summary>
        private void DeleteFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                UserDB.DeleteFavorite(SelectedItem); // 从数据库删除
                F_ViewModel.RemoveMessageItem(SelectedItem.guid); // 从ViewModel删除
                SelectedItem = null;

                NotifyPopup notifyPopup = new NotifyPopup("删除收藏成功!");
                notifyPopup.Show();
            }
        }
    }
}
