namespace judu.Models
{
    /// <summary>
    /// 数据来源与获取的数据集合
    /// </summary>
    public class FeedReaderItem
    {
        public Models.StringItem FeedType;
        public ViewModels.MessageViewModel MessageViewModel { get; set; }
    }
}
