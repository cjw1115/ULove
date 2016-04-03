using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using Windows.Data.Json;

namespace ULoveWebService.Service
{
    public class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }


    }
    public enum NotificationType
    {
        Message=1,
        ShareImage,
        BindLover,
        SystemNotice
    }

    public class NotificationService
    {
        public NotificationService(string uid,NotificationType type)
        {
            DAL.NotificationStutusDAL dal = new DAL.NotificationStutusDAL();
            dal.Set(uid, (int)type);
        }
        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            dynamic o=Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
            OAuthToken token = new OAuthToken();
            token.AccessToken = o.access_token;
            token.TokenType = o.token_type;
            return token;
        }

        protected OAuthToken GetNewAccessToken()
        {
            DAL.NotificationBasicInfoDAL dal = new DAL.NotificationBasicInfoDAL();
            var item = dal.Select();
            string secret = item.Secret;
            string sid = item.Sid;

            var urlEncodedSecret = HttpUtility.UrlEncode(secret);
            var urlEncodedSid = HttpUtility.UrlEncode(sid);

            var body =
              String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com", urlEncodedSid, urlEncodedSecret);

            string response;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = client.UploadString("https://login.live.com/accesstoken.srf", body);
            }
            //更新到数据库
            var token= GetOAuthTokenFromJson(response);
            item.AccessToken = token.AccessToken;
            item.TokenType = token.TokenType;
            dal.Save(item);

            return token;
        }
        protected OAuthToken GetAccessToken()
        {
            DAL.NotificationBasicInfoDAL dal = new DAL.NotificationBasicInfoDAL();
            var item=dal.Select();
            
            OAuthToken token = new OAuthToken();
            token.AccessToken = item.AccessToken;
            token.TokenType = item.TokenType;
            return token;
        }
        public string PostToWns(string uri, string xml,string notificationType= "wns/toast", string contentType= "text/xml")
        {
            try
            {
                var accessToken = GetAccessToken();
                byte[] contentInBytes = Encoding.UTF8.GetBytes(xml);

                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Method = "POST";
                request.Headers.Add("X-WNS-Type", notificationType);
                request.ContentType = contentType;
                request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                    return webResponse.StatusCode.ToString();
            }

            catch (WebException webException)
            {
                HttpStatusCode status = ((HttpWebResponse)webException.Response).StatusCode;

                if (status == HttpStatusCode.Unauthorized)
                {
                    // The access token you presented has expired. Get a new one and then try sending
                    // your notification again.

                    // Because your cached access token expires after 24 hours, you can expect to get 
                    // this response from WNS at least once a day.

                    /*var token = */GetNewAccessToken();

                    // We recommend that you implement a maximum retry policy.
                    return PostToWns(uri, xml,notificationType, contentType);
                }
                else if (status == HttpStatusCode.Gone || status == HttpStatusCode.NotFound)
                {
                    // The channel URI is no longer valid.

                    // Remove this channel from your database to prevent further attempts
                    // to send notifications to it.

                    // The next time that this user launches your app, request a new WNS channel.
                    // Your app should detect that its channel has changed, which should trigger
                    // the app to send the new channel URI to your app server.

                    return "推送被拒绝";
                }
                else if (status == HttpStatusCode.NotAcceptable)
                {
                    // This channel is being throttled by WNS.

                    // Implement a retry strategy that exponentially reduces the amount of
                    // notifications being sent in order to prevent being throttled again.

                    // Also, consider the scenarios that are causing your notifications to be throttled. 
                    // You will provide a richer user experience by limiting the notifications you send 
                    // to those that add true value.

                    return "未开启推送服务";
                }
                else
                {
                    // WNS responded with a less common error. Log this error to assist in debugging.

                    // You can see a full list of WNS response codes here:
                    // http://msdn.microsoft.com/en-us/library/windows/apps/hh868245.aspx#wnsresponsecodes

                    string[] debugOutput = {
                                       status.ToString(),
                                       webException.Response.Headers["X-WNS-Debug-Trace"],
                                       webException.Response.Headers["X-WNS-Error-Description"],
                                       webException.Response.Headers["X-WNS-Msg-ID"],
                                       webException.Response.Headers["X-WNS-Status"]
                                   };
                    return string.Join(" | ", debugOutput);
                }
            }

            catch (Exception ex)
            {
                return "EXCEPTION: " + ex.Message;
            }
        }

        public  XmlDocument GetTextToastXml(string message)
        {
            string toastxml=$"<toast><visual><binding template=\"ToastGeneric\"><text>ta向你推送了一张壁纸</text><text>{message}</text></binding></visual></toast>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(toastxml);
            return doc;
        }
        public string GetTextToastString(string message,NotificationType type)
        {
            string toastxml = null;
            switch (type)
            {
                case NotificationType.Message:
                    break;
                case NotificationType.ShareImage:
                    toastxml = $"<toast><visual><binding template=\"ToastGeneric\"><text>ta向你推送了一张壁纸</text><text>{message}</text></binding></visual></toast>";
                    break;
                case NotificationType.BindLover:
                    toastxml= $"<toast><visual><binding template=\"ToastGeneric\"><text>{message}请求和你绑定账号</text><text>点击查看详情</text></binding></visual></toast>";
                    break;
                case NotificationType.SystemNotice:
                    break;
                default:
                    break;
            }
            return toastxml;
        }
    }

}