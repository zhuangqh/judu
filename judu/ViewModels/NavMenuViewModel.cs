using judu.Function;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace judu.ViewModels
{
    /// <summary>
    /// 侧栏导航的ViewModel
    /// </summary>
    class NavMenuViewModel
    {
        private ObservableCollection<Models.NavMenuItem> allItems = new ObservableCollection<Models.NavMenuItem>();
        public ObservableCollection<Models.NavMenuItem> AllItems { get { return this.allItems; } }
        private Models.NavMenuItem selectedItem = null;

        public Models.NavMenuItem SelectedItem
        {
            get { return selectedItem; }
            set { this.selectedItem = value; }
        }

        public NavMenuViewModel()
        {

            allItems.Add(new Models.NavMenuItem("主页", Windows.UI.Xaml.Controls.Symbol.Home, typeof(Home)));
            allItems.Add(new Models.NavMenuItem("用户信息", Windows.UI.Xaml.Controls.Symbol.Contact, typeof(UserInfo)));
            allItems.Add(new Models.NavMenuItem("收藏", Windows.UI.Xaml.Controls.Symbol.OutlineStar, typeof(Favorite)));
        }

        public NavMenuViewModel(string mode)
        {
            allItems.Add(new Models.NavMenuItem("订阅", Windows.UI.Xaml.Controls.Symbol.Add, typeof(add)));
            allItems.Add(new Models.NavMenuItem("设置", Windows.UI.Xaml.Controls.Symbol.Setting, typeof(Setting)));
        }

        public void RemoveNavMenuItem(string Id)
        {
            for (int i = 0; i < this.allItems.Count; i++)
            {
                if (this.allItems[i].id == Id)
                {
                    this.allItems.Remove(this.allItems[i]);
                    break;
                }
            }

            this.selectedItem = null;
        }

        public void AddTodoItem(Models.NavMenuItem NavMenu)
        {
            this.allItems.Add(NavMenu);
        }
    }
}
