using Newtonsoft.Json;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace judu.Services
{
    /// <summary>
    /// 数据库，用户处理持久化数据的增删改查
    /// </summary>
    public class UserDatabase
    {
        /// <summary>
        /// 载入数据库时，连接数据库，创建表
        /// </summary>
        private void LoadDatabase()
        {
            conn = new SQLiteConnection("User.db");
            // 订阅源表
            string sql = @"CREATE TABLE IF NOT EXISTS "
                          + "Subscription (TableId    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                          + "Username    VARCHAR(100),"
                          + "FeedType    VARCHAR(100)"
                          + ");";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
            // 收藏夹表
            sql = @"CREATE TABLE IF NOT EXISTS "
                    + "Favorite (TableId    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                    + "Username    VARCHAR(100),"
                    + "Title       VARCHAR(100),"
                    + "FeedType    VARCHAR(100),"
                    + "Author      VARCHAR(100),"
                    + "Guid        VARCHAR(100),"
                    + "PublishDate VARCHAR(100)"
                    + ");";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
        }

        private SQLiteConnection conn { get; set; }
        private string Username { get; set; }

        public UserDatabase()
        {
            LoadDatabase();
            Username = GetUsername();
        }

        /// <summary>
        /// Database API
        /// </summary>

        #region 用户名相关
        static public void SetUser(string username)
        {
            ApplicationData.Current.RoamingSettings.Values["Username"] =
                JsonConvert.SerializeObject(username);
        }
        
        static public string GetUsername()
        {
            string username = "";
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Username"))
            {
                username = JsonConvert.DeserializeObject<string>(
                    (string)ApplicationData.Current.RoamingSettings.Values["Username"]);
            }
            return username;
        }

        public Models.UserInfo GetUser()
        {
            string username = "";
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Username"))
            {
                username = JsonConvert.DeserializeObject<string>(
                    (string)ApplicationData.Current.RoamingSettings.Values["Username"]);
            }

            if (username == string.Empty) return null;
            return new Models.UserInfo() {
                username = username,
                subscription = GetSubscription(),
                favorite = GetFavorite()
            };
        }
        #endregion

        #region 订阅源相关操作
        public List<string> GetSubscription()
        {
            string username = GetUsername();
            string sql = "SELECT FeedType FROM Subscription "
              + "WHERE Username = ?";
            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, username);

                List<string> res = new List<string>();
                while (statement.Step() == SQLiteResult.ROW)
                { // get result of one row
                    res.Add((string)statement[0]);
                }

                return res;
            }
        }

        /// <summary>
        /// 根据订阅表，更新数据库
        /// </summary>
        /// <param name="subsTable">订阅表</param>
        public void UpdateSubscription(List<string> subsTable)
        {
            if (subsTable == null) return;
            string username = GetUsername();

            string sql = "DELETE FROM Subscription WHERE Username = ?";
            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, username);
                statement.Step();
            }

            sql = "INSERT INTO Subscription (Username, FeedType) VALUES (?, ?)";
            for (int i = 0; i < subsTable.Count; ++i)
            {
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, username);
                    statement.Bind(2, subsTable[i]);
                    statement.Step();
                }
            }
        }
        #endregion

        #region 收藏夹相关操作
        /// <summary>
        /// 查询该用户的该收藏是否存在
        /// </summary>
        public bool IsFavoriteExist(Models.MessageItem msg)
        {
            string sql = "SELECT * FROM Favorite WHERE guid = ? AND Username = ?";
            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, msg.guid);
                statement.Bind(2, Username);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    return true;
                }

                return false;
            }
        }

        public bool AddFavorite(Models.MessageItem msg)
        {
            if (IsFavoriteExist(msg))
            {
                return false;
            } else
            {
                string sql = "INSERT INTO Favorite (Username, Title, FeedType, Author, Guid, PublishDate) "
                + "VALUES (?, ?, ?, ?, ?, ?)";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, GetUsername());
                    statement.Bind(2, msg.title);
                    statement.Bind(3, msg.feedType);
                    statement.Bind(4, msg.author);
                    statement.Bind(5, msg.guid);
                    statement.Bind(6, msg.publishDate.ToString());
                    statement.Step();
                }
                return true;
            }
        }

        public void DeleteFavorite(Models.MessageItem msg)
        {
            string sql = "DELETE FROM Favorite WHERE guid = ?";

            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, msg.guid);
                statement.Step();
            }
        }

        public List<Models.MessageItem> GetFavorite()
        {
            string sql = "SELECT * FROM Favorite WHERE Username = ?";
            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, Username);
                List<Models.MessageItem> res = new List<Models.MessageItem>();
                while (statement.Step() == SQLiteResult.ROW)
                {
                    res.Add(new Models.MessageItem() {
                        title = (string)statement[2],
                        feedType = (string)statement[3],
                        author = (string)statement[4],
                        guid = (string)statement[5],
                        publishDate =  DateTime.Parse((string)statement[6])
                    });
                }

                return res;
            }
        }

        public void UpdateFavorite(List<Models.MessageItem> msg)
        {
            if (msg == null) return;
            string username = GetUsername();

            string sql = "DELETE FROM Favorite WHERE Username = ?";
            using (var statement = conn.Prepare(sql))
            {
                statement.Bind(1, username);
                statement.Step();
            }

            for (int i = 0; i < msg.Count; ++i)
            {
                AddFavorite(msg[i]);
            }
        }
        #endregion

        #region 用户设置相关
        static public void SaveSetting(ElementTheme theme)
        {
            ApplicationData.Current.RoamingSettings.Values["Theme"] =
                JsonConvert.SerializeObject(theme);
        }

        static public ElementTheme LoadSetting()
        {
            ElementTheme theme = ElementTheme.Light;
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Theme"))
            {
                theme = JsonConvert.DeserializeObject<ElementTheme>(
                    (string)ApplicationData.Current.RoamingSettings.Values["Theme"]);
            }
            return theme;
        }
        #endregion
    }
}
