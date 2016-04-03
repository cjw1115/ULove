using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ULove.Models;
using Windows.Data.Json;
using Newtonsoft.Json;

namespace ULove.Services
{
    public class ULoveCoreService
    {
        private class Result
        {
            public object Data { get; set; }
            public string Message { get; set; }
        }
        public static async Task<string> ShareForLover(UserEntity user,int imageID,string message)
        {
            ULoveShareEntity entity = new ULoveShareEntity();
            entity.UidFrom = user.Uid;
            entity.UidTo = user.UidOfU;
            entity.ULoveImageID = imageID;
            entity.Message = message;

            var webdal = new DAL.ULoveWebDAL(true,user.Uid);
            var dic = MapDic(entity);
            
            var re = await webdal.Post(App.rootUri + "api/share", dic);

            return re.Message;
        }
        public static async Task<ULoveShareEntity> GetSharedInfo(UserEntity user)
        {
            ULoveShareEntity entity = null;
            var webdal = new DAL.ULoveWebDAL(true,user.Uid);
            HttpClient client = new HttpClient();
            var re = await webdal.Get($"{ App.rootUri}api/getshareinfo?uid={user.Uid}");

            if (re != null && re.Data != null)
            {
                entity = Newtonsoft.Json.JsonConvert.DeserializeObject<ULoveShareEntity>(re.Data.ToString());
            }
            return entity;
        }
        public static async Task<ULoveShareEntity> GetLastSharedInfo(UserEntity user)
        {
            ULoveShareEntity entity = null;
            var webdal = new DAL.ULoveWebDAL(true,user.Uid);
            HttpClient client = new HttpClient();
            var result = await webdal.Get($"{ App.rootUri}api/getlastshareinfo?uidofu={user.Uid}");

            if (result != null && result.Data != null)
            {

                entity = Newtonsoft.Json.JsonConvert.DeserializeObject<ULoveShareEntity>(result.Data.ToString());
            }
            return entity;
        }
        public static async Task<UserEntity> GetLoverInfo(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return null;
            }
            var webdal = new DAL.ULoveWebDAL(true, uid);
            var re = await webdal.Get($"{App.rootUri}api/getbindlover?uid={uid}");
            if (re != null && re.Data != null)
            {
                var bindlover = Newtonsoft.Json.JsonConvert.DeserializeObject<UserEntity>(re.Data.ToString());
                return bindlover;
               
            }
            else
            {
                return null;

            }
        }

        public static async Task<string> BindLover(UserEntity user,string uidofu)
        {
            if (string.IsNullOrEmpty(uidofu))
            {
                return "请输入正确的ID";
            }
            if (!string.IsNullOrEmpty(user.UidOfU))
            {
                return "只能绑定一个Lover,请先解绑";
            }
            var webdal = new DAL.ULoveWebDAL(true, user.Uid);
            var re = await webdal.Get($"{App.rootUri}api/bind?uid={user.Uid}&uidofu={uidofu}");

            return re.Message;
        }
        public static async Task<string> GetBindRequst(string uid)
        {
            
            var webdal = new DAL.ULoveWebDAL(true,uid);
            var re = await webdal.Get($"{App.rootUri}api/getbindrequest?uid={uid}");
            if (re != null && re.Data != null)
            {
                return re.Data as string;
            }
            return null;
        }

        public static async Task<string> AcceptBindLover(string uid, string uidofu)
        {
            
            var webdal = new DAL.ULoveWebDAL(true,uid);
            
            var re = await webdal.Get($"{App.rootUri}api/acceptbindrequest?uid={uid}&uidofu={uidofu}");
            
            return re.Message;
        }
        public static async Task<string> RefuseBindLover(string uid,string uidofu)
        {
            var webdal = new DAL.ULoveWebDAL(true,uid);
            var re = await webdal.Get($"{App.rootUri}api/refusebindrequest?uid={uid}&uidofu={uidofu}");
            return re.Message;
        }
        public static async Task<string> UnBindLover(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.UidOfU))
            {
                return "还未绑定Lover";
            }
            var webdal = new DAL.ULoveWebDAL(true,user.Uid);
            var result = await webdal.Get($"{App.rootUri}api/unbind?uid={user.Uid}");
            return result.Message;
        }

        private static Dictionary<string,string> MapDic<T>(T entity) where T:class
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var props = entity.GetType().GetProperties();
            foreach (var item in props)
            {
                var value = item.GetValue(entity);
                if (value != null)
                {
                    dic.Add(item.Name,value.ToString() );
                }
                else
                {
                    dic.Add(item.Name, null);
                }
            }
            return dic;
        }
    }
    
}
