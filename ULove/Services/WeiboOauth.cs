using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;

namespace ULove.Services
{
    public class WeiboOauth
    {
        private static readonly string key = "1189519362";
        private static readonly string secret = "03e2f87874a5ae0dbd2b12ec0376c7cd";
        private static readonly string redirect_uri = "http://www.ulove.com/api/back/weibo";
        private static readonly string authorize_uri = "https://api.weibo.com/OAuth2/authorize ";
        private static readonly string access_token = "https://api.weibo.com/OAuth2/access_token";

        //https://api.weibo.com/oauth2/access_token?client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&grant_type=authorization_code&redirect_uri=YOUR_REGISTERED_REDIRECT_URI&code=CODE
        public static async Task<AccessTokenInfo> GetToken(string code)
        {
            AccessTokenInfo info = new AccessTokenInfo();
            string accessUri = $"https://api.weibo.com/oauth2/access_token?client_id={key}&client_secret={secret}&grant_type=authorization_code&redirect_uri={redirect_uri}&code={code}";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, accessUri);
            HttpResponseMessage response=await client.SendAsync(request);
            string jsonString=await response.Content.ReadAsStringAsync();
            var json=JsonObject.Parse(jsonString);
            info.access_token= json["access_token"].GetString();
            info.uid= json["uid"].GetString();
            return info;
        }
        public static async Task<WeiboUser> GetUserInfo(AccessTokenInfo info)
        {
            WeiboUser user = null;
            string userinfoUri = $"https://api.weibo.com/2/users/show.json?access_token={info.access_token}&uid={info.uid}";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, userinfoUri);
            HttpResponseMessage response = await client.SendAsync(request);
            string jsonString = await response.Content.ReadAsStringAsync();
            var json = JsonObject.Parse(jsonString);

            user = new WeiboUser();
            user.id=json["idstr"].GetString();
            user.screen_name = json["screen_name"].GetString();
            user.profile_image_url = json["profile_image_url"].GetString();
            user.gender = json["gender"].GetString();

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
}
