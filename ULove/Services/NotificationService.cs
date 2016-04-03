using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ULove.DAL;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace ULove.Services
{
    public class NotificationService
    {
        
        public NotificationService()
        {
            
        }
        public async Task<string> SetNotificationChannel(string uid)
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                var webdal = new ULoveWebDAL(true, uid);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("uid", uid);
                dic.Add("ChannelUri", channel.Uri.ToString());

                var re = await webdal.Post($"{App.rootUri}api/setnotificationchannel", dic);
                return re.Message;
            }
            catch (Exception ex)
            {
                return "失败";
            }
            
            
        }
        public async Task<string> CancelNotificationChannel(string uid)
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                channel.Close();
                var webdal = new ULoveWebDAL(true, uid);
                var re = await webdal.Get($"{App.rootUri}api/cancelnotificationchannel?uid={uid}");

                return re.Message;
            }
            catch(Exception ex)
            {
                return "失败";
            }
            
            
        }
    }
}
