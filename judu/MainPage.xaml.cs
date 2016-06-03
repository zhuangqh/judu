using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace judu
{
    /// <summary>
    /// 主页面，包含侧栏导航，消息阅读页面
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            M_ViewModel = new ViewModels.MessageViewModel();
            Nav_viewModel = new ViewModels.NavMenuViewModel();
            Func_viewModel = new ViewModels.NavMenuViewModel("func");
            UserDB = new Services.UserDatabase();

            ContentView.Navigate(typeof(Home));
        }

        ViewModels.MessageViewModel M_ViewModel { get; set; }
        ViewModels.NavMenuViewModel Nav_viewModel { get; set; }
        ViewModels.NavMenuViewModel Func_viewModel { get; set; }
        private Services.UserDatabase UserDB { get; set; }

        private void TopToggleButton_Click(object sender, RoutedEventArgs e)
        {
            M_ViewModel.TopToggle = !M_ViewModel.TopToggle;
        }

        /// <summary>
        /// 侧栏，主页面导航
        /// </summary>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Nav_viewModel.SelectedItem = (e.ClickedItem as Models.NavMenuItem);
            Type destPage = Nav_viewModel.SelectedItem.DestPage;
            if (destPage != null)
            {
                if (destPage == typeof(UserInfo))
                {
                    // 检查用户是否已经登录
                    Models.UserInfo user;
                    if ((user = UserDB.GetUser()) == null)
                    {
                        ContentView.Navigate(typeof(Login));
                    } else
                    {
                        ContentView.Navigate(typeof(UserInfo), user); // 传入用户信息
                    }
                } else
                {
                    ContentView.Navigate(destPage);
                }
            }

        }

        /// <summary>
        /// 底部功能键导航
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Func_ItemClick(object sender, ItemClickEventArgs e)
        {
            Func_viewModel.SelectedItem = (e.ClickedItem as Models.NavMenuItem);
            if (Func_viewModel.SelectedItem.DestPage != null)
            {
                ContentView.Navigate(Func_viewModel.SelectedItem.DestPage);
            }
        }
    }
}
