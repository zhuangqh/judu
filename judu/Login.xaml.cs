using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace judu
{
    /// <summary>
    /// 登录页面
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
            UserDB = new Services.UserDatabase();
        }

        private Services.UserDatabase UserDB { get; set; }

        /// <summary>
        /// 登录。若登录成功，跳转至个人信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Services.UserService.Signin(LoginUsername.Text, LoginPassword.Password))
            {
                Services.UserDatabase.SetUser(LoginUsername.Text);
                NotifyPopup notifyPopup = new NotifyPopup("登录成功");
                notifyPopup.Show();

                // 从服务器获取个人存档
                // 并同步至本地
                Models.UserInfo userInfo =  await Services.UserService.GetProfile();
                UserDB.UpdateFavorite(userInfo.favorite);
                UserDB.UpdateSubscription(userInfo.subscription);

                Frame.Navigate(typeof(UserInfo), UserDB.GetUser());
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signup));
        }
    }
}
