using Windows.UI.Xaml.Controls;

namespace judu.Function
{
    /// <summary>
    /// 作者信息
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
        }
    }
}
