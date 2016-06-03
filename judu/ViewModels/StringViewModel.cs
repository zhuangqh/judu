using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace judu.ViewModels
{
    /// <summary>
    /// 字符串视图
    /// </summary>
    public class StringViewModel
    {
        private ObservableCollection<Models.StringItem> allItems = new ObservableCollection<Models.StringItem>();
        public ObservableCollection<Models.StringItem> AllItems { get { return this.allItems; } }
       
        public StringViewModel() {
        }

        public StringViewModel(List<string> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                allItems.Add(new Models.StringItem(source[i]));
            }
        }
    }
}
