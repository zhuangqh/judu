using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace judu.Models
{
    /// <summary>
    /// 左侧导航数据类型
    /// </summary>
    class NavMenuItem
    {
        public string id;
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Type DestPage { get; set; }

        public NavMenuItem(string Label, Symbol symbol, Type DestPage)
        {
            this.id = Guid.NewGuid().ToString();
            this.Label = Label;
            this.Symbol = symbol;
            this.DestPage = DestPage;
        }
    }
}
