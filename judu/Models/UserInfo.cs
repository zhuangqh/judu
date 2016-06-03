using System.Collections.Generic;

namespace judu.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        public string username { get; set; }
        public List<string> subscription { get; set; }
        public List<MessageItem> favorite { get; set; }
    }
}
