using System.Collections.Generic;

namespace judu.Models
{
    public class Ultity
    {
        private static Ultity ultityInstance;
        private Dictionary<string, int> FeedIndexTable = new Dictionary<string, int>();
        private Dictionary<string, string> FeedNameTable = new Dictionary<string, string>();

        private Ultity()
        {
            // 生成序号表
            FeedIndexTable["adaymag"] = 0;
            FeedIndexTable["dajia"] = 1;
            FeedIndexTable["duxieren"] = 2;
            FeedIndexTable["ecocn"] = 3;
            FeedIndexTable["ifanr"] = 4;
            FeedIndexTable["geekfan"] = 5;
            FeedIndexTable["ifeng"] = 6;
            FeedIndexTable["kr36"] = 7;
            FeedIndexTable["msa"] = 8;
            FeedIndexTable["appinn"] = 9;
            FeedIndexTable["zhihu"] = 10;
            FeedIndexTable["yidu"] = 11;

            // 生成名称表
            FeedNameTable["yidu"] = "壹读";
            FeedNameTable["engadget"] = "engadget";
            FeedNameTable["msa"] = "微软亚洲研究院";
            FeedNameTable["ifeng"] = "凤凰网";
            FeedNameTable["appinn"] = "小众软件";
            FeedNameTable["adaymag"] = "A Day";
            FeedNameTable["geekfan"] = "极客范";
            FeedNameTable["scipark"] = "科学公园";
            FeedNameTable["dajia"] = "大家论坛";
            FeedNameTable["ifanr"] = "爱范儿";
            FeedNameTable["kr36"] = "36氪";
            FeedNameTable["ecocn"] = "经济学人";
            FeedNameTable["zhihu"] = "知乎精选";
        }

        // 获取单例对象
        public static Ultity GetInstance()
        {
            if (ultityInstance == null)
                ultityInstance = new Ultity();
            return ultityInstance;
        }

        public int GetFeedIndex(string feedType)
        {
            return FeedIndexTable.ContainsKey(feedType) ? FeedIndexTable[feedType] : -1;
        }

        public string GetFeedName(string feedType)
        {
            return FeedNameTable.ContainsKey(feedType) ? FeedNameTable[feedType] : "";
        }
    }
}
