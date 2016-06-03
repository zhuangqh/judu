using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace judu.Services
{
    /// <summary>
    /// 用于与服务器通信，处理用户的各项操作
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// 根据用户名，密码注册账号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="password2">确认密码</param>
        /// <returns>是否注册成功</returns>
        static public bool Signup(string username, string password, string password2)
        {
            // 确认信息是否填写正确
            string validateMsg;
            if ((validateMsg = UserValidator(username, password, password2)) != string.Empty) {
                var err = new MessageDialog(validateMsg).ShowAsync();
                return false;
            }
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

                httpClient.BaseAddress = new Uri("http://localhost:3000");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });
                // POST请求注册
                var result = httpClient.PostAsync("/api/regist", content).Result;
                
                string resultContent = result.Content.ReadAsStringAsync().Result;
                #endregion

                // 确认正确返回
                ErrorChecker(resultContent);
            } catch (Exception e)
            {
                var i = new MessageDialog(e.Message).ShowAsync();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据用户名密码登录
        /// </summary>
        /// <returns>是否登录成功</returns>
        static public bool Signin(string username, string password)
        {
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

                httpClient.BaseAddress = new Uri("http://localhost:3000");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });
                // POST请求登录
                var result = httpClient.PostAsync("/api/signin", content).Result;

                string resultContent = result.Content.ReadAsStringAsync().Result;
                #endregion
                // 确认是否正确返回
                ErrorChecker(resultContent);
            } catch (Exception e)
            {
                var i = new MessageDialog(e.Message).ShowAsync();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 用正则表达式检测用户填写信息是否正确
        /// </summary>
        /// <returns>错误提示信息，如无错误返回空字符串</returns>
        static private string UserValidator(string username, string password, string password2)
        {
            string validateMsg = "";
            if (password != password2) validateMsg += "重复的密码不正确\n";
            Regex usernameReg = new Regex("^[a-zA-Z][a-zA-Z0-9_]{5,18}$");  // 6~18位英文字母、数字或下划线，英文字母开头
            Regex passwordReg = new Regex("^[a-zA-Z][a-zA-Z0-9_]{5,12}$");  // 6~12位数字、字母、中划线、下划线，字母开头
            if (!usernameReg.IsMatch(username)) {
                validateMsg += "用户名不合法\n";
            }
            if (!passwordReg.IsMatch(password))
            {
                validateMsg += "密码不合法";
            }
            return validateMsg;
        }

        /// <summary>
        /// 校验错误码，若错误码非0，抛出错误
        /// </summary>
        /// <param name="resultContent"></param>
        static private void ErrorChecker(string resultContent)
        {
            JObject res = (JObject)JsonConvert.DeserializeObject(resultContent);
            if (res["errCode"].ToString() != "0")
                throw new Exception(res["errMsg"].ToString());
        }

        /// <summary>
        /// 将用户的信息（订阅源列表，收藏）发送至服务器
        /// </summary>
        /// <param name="userInfo"></param>
        static public void UpdateProfile(Models.UserInfo userInfo)
        {
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

                httpClient.BaseAddress = new Uri("http://localhost:3000");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("user", JsonConvert.SerializeObject(userInfo))
                });
                var result = httpClient.PostAsync("/api/profile", content).Result;

                string resultContent = result.Content.ReadAsStringAsync().Result;
                #endregion
                // 确认是否正确返回
                ErrorChecker(resultContent);
            } catch (Exception e)
            {
                var i = new MessageDialog(e.Message).ShowAsync();
            }
        }

        /// <summary>
        /// 从服务器获取用户数据
        /// </summary>
        /// <returns>异步等待，返回用户信息</returns>
        static public async Task<Models.UserInfo> GetProfile()
        {
            // 从数据库获取用户名
            string username = UserDatabase.GetUsername();
            Models.UserInfo userInfo = new Models.UserInfo();

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

                string getOverview_json = "http://localhost:3000/api/profile?username=" + username;
                HttpResponseMessage response = await httpClient.GetAsync(getOverview_json);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                ErrorChecker(result);
                // 反序列化结果字符串
                userInfo = JsonConvert.DeserializeObject<Models.UserInfo>(result);
                #endregion

            } catch (Exception e)
            {
                var ii = new MessageDialog(e.Message).ShowAsync();
            }

            return userInfo;
        }
    }
}
