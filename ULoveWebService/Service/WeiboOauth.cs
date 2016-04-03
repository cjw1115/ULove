using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ULoveWebService.Service
{
    public class WeiboOauth
    {
        private static readonly string key = "1189519362";
        private static readonly string secret = "03e2f87874a5ae0dbd2b12ec0376c7cd";
        private static readonly string redirect_uri = "http://uloveweb.azurewebsites.net/api/back/weibo";
        private static readonly string authorize_uri = "https://api.weibo.com/OAuth2/authorize ";
        private static readonly string access_token = "https://api.weibo.com/OAuth2/access_token";

        //https://api.weibo.com/oauth2/access_token?client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&grant_type=authorization_code&redirect_uri=YOUR_REGISTERED_REDIRECT_URI&code=CODE
        public static AccessTokenInfo GetToken(string code)
        {
            AccessTokenInfo info = new AccessTokenInfo();
            string accessUri = $"https://api.weibo.com/oauth2/access_token?client_id={key}&client_secret={secret}&grant_type=authorization_code&redirect_uri={redirect_uri}&code={code}";

            //Result re = new Result();
            ////HttpHelper(re, accessUri, "post");
            //HttpHelper(accessUri, "post");
            //var jsonString = re.Data as string;
            var jsonString=HttpHelper(accessUri, "post");
            if (jsonString == null)
            {
                return null;
            }
            dynamic json=JsonConvert.DeserializeObject(jsonString);
            
            info.access_token = json.access_token;
            info.uid = json.uid;
            return info;
        }
        public static string HttpHelper(string uri, string method = "post", string content = "")
        {
            try
            {

                byte[] contentInBytes = Encoding.UTF8.GetBytes(content);
                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Method = method;
                if (method.Trim().ToLower() == "post")
                {
                    request.ContentType = "application/x-www-form-urlencoded";

                    using (Stream requestStream = request.GetRequestStream())
                        requestStream.Write(contentInBytes, 0, contentInBytes.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using(StreamReader reader=new StreamReader(stream))
                        {
                            string re=reader.ReadToEnd();
                            return re;
                        }
                        //byte[] buffer = new byte[webResponse.ContentLength];
                        //stream.Read(buffer, 0, buffer.Length);
                        //string re = Encoding.UTF8.GetString(buffer);
                        //return re;
                    }
                }
            }
            catch (WebException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //public async static void HttpHelper(Result result,string uri, string method = "post", string content = "")
        //{
        //    result.Data= await Requst(uri, method, content);
        //}
        //public async static Task<string> Requst(string uri, string method = "post", string content = "")
        //{
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response;
        //    if (method.Trim().ToLower() == "post")
        //    {
        //        response = await client.PostAsync(uri, new StringContent(content));
        //    }
        //    else
        //    {
        //        response = await client.GetAsync(uri);
        //    }
        //    string jsonString = await response.Content.ReadAsStringAsync();
        //    return jsonString;
        //}
        public static WeiboUser GetUserInfo(AccessTokenInfo info)
        {
            WeiboUser user = null;
            string userinfoUri = $"https://api.weibo.com/2/users/show.json?access_token={info.access_token}&uid={info.uid}";

            //Result re = new Result();
            //HttpHelper(re,userinfoUri, "get");
            //var jsonString = re.Data as string;
            var jsonString= HttpHelper(userinfoUri, "get");
            dynamic json = JsonConvert.DeserializeObject(jsonString);
            
            user = new WeiboUser();
            if (json.idstr == null)
            {
                return null;
            }
            user.id = json.idstr;
            user.screen_name = json.screen_name;
            user.profile_image_url = json.profile_image_url;
            user.gender = json.gender;

            return user;
        }
    }
    public class AccessTokenInfo
    {
        public string access_token { get; set; }
        public string uid { get; set; }
    }
    public class WeiboCode
    {
        public string code { get; set; }
    }
    public class WeiboUser
    {
        public string id { get; set; }//uid
        public string screen_name {get;set;}//昵称
        public string gender { get; set; }//性别
        public string profile_image_url { get; set; }//头像
    }
    public class Result
    {
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
