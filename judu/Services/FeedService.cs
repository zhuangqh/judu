using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using judu.Models;
using judu.ViewModels;
using System.Collections.ObjectModel;
using FeedData = System.Collections.Generic.List<judu.Models.FeedDataItem>;

namespace judu.Services
{
    /// <summary>
    /// 用于从服务器获取订阅源的数据
    /// </summary>
    public class FeedService
    {
        /// <summary>
        /// 根据订阅源的类型，从服务器获取数据
        /// </summary>
        /// <param name="feedType">订阅源类型</param>
        /// <returns>异步等待，返回一个ViewModel</returns>
        static public async Task<MessageViewModel> GetFeedAsync(string feedType)
        {
            MessageViewModel messageViewModel = new MessageViewModel();
            try
            {
                #region Get data from Internet
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.86 Safari/537.36";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                string getOverview_json = "http://localhost:3000/overview?type=" + feedType;
                HttpResponseMessage response = await httpClient.GetAsync(getOverview_json);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                // 反序列化结果字符串
                FeedData res = (FeedData)JsonConvert.DeserializeObject<FeedData>(result);
                #endregion

                #region Generate ViewModel

                // 根据服务器返回数据，生成ViewModel
                for (int i = 0; i < res.Count; i++)
                {
                    messageViewModel.AllItems.Add(new MessageItem(
                        res[i].title,
                        res[i].feedType,
                        res[i].author,
                        res[i].guid,
                        DateTimeOffset.Parse(res[i].pubDate)));
                }
                //var ii = new MessageDialog(res.Count.ToString()).ShowAsync();
                #endregion
            } catch (Exception e)
            {
                var ii = new MessageDialog(e.Message).ShowAsync();
            }
            return messageViewModel;
        }

        /// <summary>
        /// 从服务器获取个人订阅源的数据
        /// </summary>
        /// <returns>异步等待，返回一个ViewModel</returns>
        static public async Task<MessageViewModel> GetAllFeedAsync()
        {
            // 从数据库获取用户名
            string username = Services.UserDatabase.GetUsername();

            ViewModels.MessageViewModel messageViewModel = new MessageViewModel();
            try
            {
                #region Get user's subscription from Internet
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.86 Safari/537.36";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                string getOverview_json = "http://localhost:3000/overview?type=all&&username=" + username;
                HttpResponseMessage response = await httpClient.GetAsync(getOverview_json);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                // 反序列化结果字符串
                FeedData res = JsonConvert.DeserializeObject<FeedData>(result);

                #endregion

                // 根据服务器返回数据，生成ViewModel
                for (int i = 0; i < res.Count; i++)
                {
                    messageViewModel.AllItems.Add(new MessageItem(
                        res[i].title,
                        res[i].feedType,
                        res[i].author,
                        res[i].guid,
                        DateTimeOffset.Parse(res[i].pubDate)));
                }

            } catch (Exception e)
            {
                var ii = new MessageDialog(e.Message).ShowAsync();
            }

            return messageViewModel;
        }
    }
}
