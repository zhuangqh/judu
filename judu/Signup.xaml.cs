using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace judu
{
    /// <summary>
    /// 注册页面
    /// </summary>
    public sealed partial class Signup : Page
    {
        public Signup()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
        }
       
        /// <summary>
        /// 注册。成功后返回登录页面
        /// </summary>
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (Services.UserService.Signup(SignupUsername.Text, SignupPassword.Password, SignupPassword2.Password))
            {
                Frame.Navigate(typeof(Login));
            }
        }

        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }
    }
}
