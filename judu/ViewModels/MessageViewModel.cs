using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace judu.ViewModels
{
    /// <summary>
    /// 消息的ViewModel
    /// </summary>
    public class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Models.MessageItem> allItems = new ObservableCollection<Models.MessageItem>();
        public ObservableCollection<Models.MessageItem> AllItems
        {
            get { return allItems; }
            set
            {
                allItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllItems"));
            }
        }

        private bool _TopToggle;
        public bool TopToggle
        {
            get { return _TopToggle; }
            set
            {
                _TopToggle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TopToggle"));
            }
        }

        public MessageViewModel()
        {
            _TopToggle = false;
        }

        public MessageViewModel(List<Models.MessageItem> MsgList)
        {
            for (int i = 0; i < MsgList.Count; ++i)
            {
                allItems.Add(MsgList[i]);
            }
        }

        #region 增删改操作
        public void AddTodoItem(Models.MessageItem message)
        {
            this.allItems.Add(message);
        }

        public void RemoveMessageItem(string guid)
        {
            for (int i = 0; i < this.allItems.Count; i++)
            {
                if (this.allItems[i].guid == guid)
                {
                    this.allItems.Remove(this.allItems[i]);
                    break;
                }
            }
        }

        public void UpdateMessageItem(Models.MessageItem OriginMessage, Models.MessageItem UpdateInfo)
        {
            int index = this.allItems.IndexOf(OriginMessage);
            if (index >= 0 && index < this.allItems.Count)
            {
                this.allItems[index] = UpdateInfo;
            }
        }
        #endregion
    }
}
