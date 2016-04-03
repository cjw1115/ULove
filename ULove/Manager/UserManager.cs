using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ULove.Models;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Windows.Data.Json;
using System.Xml;
using ULove.DAL;

namespace ULove.Manager
{
    
    public class UserManager
    {
        private static UserEntity _user { get; set; }
        private static readonly StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        private static readonly string localUserFileName = "userconfig";
        private static ULove.DAL.LocalUserDAL localUserDal;

        static UserManager()
        {
            localUserDal = new LocalUserDAL(localFolder, localUserFileName);
        }

        public static async Task<UserEntity> GetUser()
        {
            if (_user != null)
            {
                return _user;
            }
            else
            {
                return _user = await localUserDal.GetLocalInfo<UserEntity>();//获取本地的账号信息
            }
        }

        public async static Task SetUser(UserEntity user)
        {
            if (user != null)
            {
                _user = user;
                await localUserDal.SetLocalInfo<UserEntity>(_user);
            }
        }
        

        public async static Task<UserEntity> GetCloudUser(UserEntity user)
        {
            try
            {
                DAL.ULoveWebDAL webdal = new ULoveWebDAL(true, user.Uid);
                var re = await webdal.Get($"{ App.rootUri}api/user?uid={user.Uid}");
                if (re != null && re.Data != null)
                {
                    var newuser = Newtonsoft.Json.JsonConvert.DeserializeObject<UserEntity>(re.Data.ToString());
                    return newuser;
                }
                return null;
            }
            catch (JsonException json)
            {
                return null;
            }
        }
        public async static Task SetCloudUser(UserEntity user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Uid", user.Uid);
            dic.Add("UidOfU", user.UidOfU);
            dic.Add("NickName", user.NickName);
            dic.Add("ProfileImageUri", user.ProfileImageUri);
            dic.Add("Gender", user.Gender);

            ULoveWebDAL webdal = new ULoveWebDAL(true,user.Uid);
            await webdal.Post(App.rootUri + "api/user", dic);
        }

        public static void Logout()
        {
            DeleteLocalUserInfo();
            _user = null;
        }
        private static void DeleteLocalUserInfo()
        {
            localUserDal.DeleteInfo<UserEntity>();
        }

        
    }
}
