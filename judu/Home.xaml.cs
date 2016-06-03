using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace judu
{
    /// <summary>
    /// 消息阅读页面
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.BurlyWood;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.BurlyWood;

            this.RequestedTheme = Services.UserDatabase.LoadSetting();
            UserDB = new Services.UserDatabase();
            ViewModel = new ViewModels.FeedReaderViewModel(UserDB.GetSubscription()); // 从网络获取订阅源数据
        }

        private Models.MessageItem SelectedItem { get; set; }
        private Services.UserDatabase UserDB { get; set; }
        private ViewModels.FeedReaderViewModel ViewModel { get; set; }

        //创建DataTransferManager，添加Windows.ApplicationModel.DataTransfer
        DataTransferManager dtm;

        /// <summary>
        /// 进入页面时注册DataRequested事件
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dtm = DataTransferManager.GetForCurrentView();
            //创建event handler
            dtm.DataRequested += Dtm_DataRequested;
            Frame rootFrame = Window.Current.Content as Frame;

            ((App)(App.Current)).BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        /// <summary>
        /// 离开页面时移除DataRequested事件
        /// </summary>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            dtm.DataRequested -= Dtm_DataRequested;
            ((App)App.Current).BackRequested -= OnBackRequested;
        }

        /// <summary>
        /// 创建DataPackage对象，并设置相应属性
        /// </summary>
        private void Dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (SelectedItem != null)
            {
                string textSource = "链接：" + SelectedItem.guid;
                string textTitle = SelectedItem.title;
                string textDescription = SelectedItem.feedType;
                DataPackage data = args.Request.Data;
                data.Properties.Title = textTitle;
                data.Properties.Description = textDescription;
                data.SetText(textSource);
            }
        }

        /// <summary>
        /// MessageItem点击事件函数
        /// 点击一个Item, 显示对应的网页
        /// </summary>
        private void MessageItem_Clicked(object sender, ItemClickEventArgs e)
        {
            SelectedItem = (e.ClickedItem as Models.MessageItem);
            if (SelectedItem.guid != null)
            {
                this.MyWebView.Navigate(new Uri(SelectedItem.guid));
            }
            if (WebViewGrid.Visibility == Visibility.Collapsed)
            {
                Frame.Navigate(typeof(MinWindow), SelectedItem.guid);
            }
            // 更新磁贴
            UpdateTile();
        }

        /// <summary>
        /// 更新磁贴
        /// </summary>
        public void UpdateTile()
        {
            int count = ViewModel.AllItems.Count;
            if (count > 0)
            {
                XmlDocument tileXml = new XmlDocument();
                tileXml.LoadXml(System.IO.File.ReadAllText("Tiles.xml"));
                XmlNodeList str = tileXml.GetElementsByTagName("text");
                for (int i = 0; i < str.Count; i++)
                {
                    ((XmlElement)str[i]).InnerText = ViewModel.AllItems[0].MessageViewModel.AllItems[0].title;
                    ((XmlElement)str[++i]).InnerText = ViewModel.AllItems[0].MessageViewModel.AllItems[0].publishDate.ToString();
                    ((XmlElement)str[++i]).InnerText = ViewModel.AllItems[0].MessageViewModel.AllItems[0].feedType;
                }

                TileNotification notifi = new TileNotification(tileXml);
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.Update(notifi);
            }
        }

        #region 底部功能按键
        /// <summary>
        /// 分享按钮点击事件函数，实现分享
        /// </summary>
        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem == null)
            {
                NotifyPopup notifyPopup = new NotifyPopup("还没选择要分享的内容呢!");
                notifyPopup.Show();
            }
            else
            {
                DataTransferManager.ShowShareUI();
            }
        }

        /// <summary>
        /// 刷新网页
        /// </summary>
        private  void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MyWebView.Refresh();
        }

        /// <summary>
        /// 收藏，若已收藏过，提示错误
        /// </summary>
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                 if (UserDB.AddFavorite(SelectedItem))
                {
                    NotifyPopup notifyPopup = new NotifyPopup("收藏成功!");
                    notifyPopup.Show();
                } else
                {
                    NotifyPopup notifyPopup = new NotifyPopup("收藏已存在!");
                    notifyPopup.Show();
                }
               
            }
        }
        #endregion
    }
}
