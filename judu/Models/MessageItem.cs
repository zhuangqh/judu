using System;
using System.Collections.Generic;

namespace judu.Models
{
    /// <summary>
    /// 资讯数据
    /// </summary>
    public class MessageItem
    {
        public string id;
        public string title { get; set; }
        public string feedType { get; set; }
        public string author { get; set; }
        public string guid { get; set; }
        public DateTime publishDate { get; set; }

        private Dictionary<string, string> FeedTable { get; set; }

        public MessageItem() { }

        public MessageItem(string title, string digest, string author, string guid, DateTimeOffset publishDate)
        {
            this.id = Guid.NewGuid().ToString(); //生成id
            this.title = title;
            setStyle(digest);
            this.author = author;
            this.guid = guid;
            this.publishDate = publishDate.DateTime;
        }

        //更新MessageItem
        public void Update(ref MessageItem UpdateInfo)
        {
            this.title = UpdateInfo.title;
            setStyle(UpdateInfo.feedType);
            this.author = UpdateInfo.author;
            this.guid = UpdateInfo.guid;
            this.publishDate = UpdateInfo.publishDate;
        }

        //设置样式
        private void setStyle(string brief)
        {
            Ultity util = Ultity.GetInstance();
            this.feedType = "From " + util.GetFeedName(brief);
        }
    }
}
