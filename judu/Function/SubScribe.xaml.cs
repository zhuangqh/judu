using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;


namespace judu.Function
{
    /// <summary>
    /// 添加或删除消息源
    /// </summary>
    public sealed partial class add : Page
    {
        List<string> allSource;
        private Services.UserDatabase UserDB { get; set; }
        public add()
        {
            this.InitializeComponent();
            this.RequestedTheme = Services.UserDatabase.LoadSetting();
            UserDB = new Services.UserDatabase();
            LoadFeedSetting();
        }

        //添加或删除资讯源
        private void FeedGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Image FeedSource = (e.ClickedItem as Image);
            if (allSource.Contains(FeedSource.Name))
                allSource.Remove(FeedSource.Name);
            else
                allSource.Add(FeedSource.Name);
        }

        // 根据序号给元素打勾
        private void LoadFeedSetting()
        {
            allSource = UserDB.GetSubscription();
            Models.Ultity ulti = Models.Ultity.GetInstance();
            List<int> selectedIndex = new List<int>();

            // 获取元素对应的序号
            foreach (var i in allSource)
                selectedIndex.Add(ulti.GetFeedIndex(i)); 
            
            for (int i = 0; i < selectedIndex.Count; ++i)
                FeedGridView.SelectRange(new ItemIndexRange(selectedIndex[i], 1));
        }

        //确认选择
        private void FeedSelect_Confirm(object sender, RoutedEventArgs e)
        {
            UserDB.UpdateSubscription(allSource);
            NotifyPopup notifyPopup = new NotifyPopup("订阅成功!");
            notifyPopup.Show();
        }
    }
}
