using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace judu.ViewModels
{
    /// <summary>
    /// 消息阅读界面ViewModel
    /// </summary>
    public class FeedReaderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Models.FeedReaderItem> allItems = new ObservableCollection<Models.FeedReaderItem>();
        public ObservableCollection<Models.FeedReaderItem> AllItems
        {
            get { return allItems; }
            set
            {
                allItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllItems)));
            }
        }

        public FeedReaderViewModel()
        {
        }

        public FeedReaderViewModel(List<string> FeedList)
        {
            GetFeed(FeedList);
        }

        /// <summary>
        /// 从网络获取每个订阅类型的数据
        /// </summary>
        /// <param name="FeedList">订阅表</param>
        private async void GetFeed(List<string> FeedList)
        {
            for (int i = 0; i < FeedList.Count; ++i)
            {
                allItems.Add(new Models.FeedReaderItem() {
                    FeedType = new Models.StringItem(FeedList[i]),
                    MessageViewModel = await Services.FeedService.GetFeedAsync(FeedList[i])
                });
            }
        }
    }
}
