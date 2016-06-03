using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace judu.Function
{
    /// <summary>
    /// 设置：日间/夜间模式选择、用户退出当前账户
    /// </summary>
    public sealed partial class Setting : Page
    {
        private Services.UserDatabase UserDB { get; set; }

        public Setting()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
            UserDB = new Services.UserDatabase();

            toggleSwitch.Toggled -= Toggled;
            try
            {
                if (this.RequestedTheme == ElementTheme.Dark)
                    toggleSwitch.IsOn = true;
            } finally
            {
                toggleSwitch.Toggled += Toggled;
            }
        }
        
        //设置主题并保存
        private void Toggled(object sender, RoutedEventArgs e)
        {
            if (this.RequestedTheme == ElementTheme.Light)
            {
                this.RequestedTheme = ElementTheme.Dark;
                Services.UserDatabase.SaveSetting(ElementTheme.Dark);
            }
            else
            {
                this.RequestedTheme = ElementTheme.Light;
                Services.UserDatabase.SaveSetting(ElementTheme.Light);
            }
        }

        //用户退出
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Services.UserService.UpdateProfile(UserDB.GetUser());
            Services.UserDatabase.SetUser("");
            NotifyPopup notifyPopup = new NotifyPopup("退出成功");
            notifyPopup.Show();
            Frame.Navigate(typeof(Login));
        }
    }
}
