using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace judu
{
    /// <summary>
    /// 个人信息展示页面
    /// </summary>
    public sealed partial class UserInfo : Page
    {
        public UserInfo()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
        }

        private Models.UserInfo userInfo { get; set; }
        private ViewModels.StringViewModel subScriptionList { get; set; }

        // 从MainPage 跳转过来，传入用户信息
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.userInfo = e.Parameter as Models.UserInfo;
            this.subScriptionList = new ViewModels.StringViewModel(userInfo.subscription);
        }

    }
}
