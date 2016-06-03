using System.Collections.Generic;

namespace judu.Models
{
    /// <summary>
    /// 获取的数据类型
    /// </summary>
    public class FeedData
    {
        public List<FeedDataItem> feedData { get; set; }
    }

    public class FeedDataItem
    {
        public string title { get; set; }
        public string feedType { get; set; }
        public string pubDate { get; set; }
        public string guid { get; set; }
        public string author { get; set; }
    }
}
