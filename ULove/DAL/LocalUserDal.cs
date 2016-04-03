using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ULove.Models;
using Windows.Storage;

namespace ULove.DAL
{
    public class LocalUserDAL
    {
        public StorageFolder LocalFolder { get; set; }
        public string LocalUserFileName { get; set; }

        
        public LocalUserDAL(StorageFolder localFolder, string _localUserFileName)
        {
            if (localFolder != null)
                LocalFolder = localFolder;
            else
                LocalFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (!string.IsNullOrEmpty(_localUserFileName))
                LocalUserFileName = _localUserFileName;
            else
                LocalUserFileName = "localfile";

        }

        /// <summary>
        /// 将一个.net对象序列化后，用指定的文件名称，存储到指定文件夹；
        /// </summary>
        /// <typeparam name="T">一个可以序列化的.Net对象</typeparam>
        /// <returns></returns>
        public async Task<T> GetLocalInfo<T>() where T : class
        {
            T entity = null;
            try
            {
                var item = await LocalFolder.TryGetItemAsync(LocalUserFileName);
                if (item == null)
                {
                    return entity;
                }
                var file = item as StorageFile;
                
                using (var ras = await file.OpenAsync(FileAccessMode.Read))
                {
                    using (var stream = ras.AsStream())
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(UserEntity));
                        var o = serializer.ReadObject(stream);
                        if (o != null)
                        {
                            entity = o as T;
                        }
                    }
                }
            }
            catch(NullReferenceException nullref)
            {
                return null;
            }
            catch (XmlException e)
            {
                return null;
            }
            return entity;
        }
        public async Task SetLocalInfo<T>(T entity)
            where T : class
        {

            var item = await LocalFolder.CreateFileAsync(LocalUserFileName, CreationCollisionOption.ReplaceExisting);

            var file = item as StorageFile;
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var stream = ras.AsStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(UserEntity));
                    serializer.WriteObject(stream, entity);
                }
            }
        }
        public async void DeleteInfo<T>()
        {
            var item = await LocalFolder.TryGetItemAsync(LocalUserFileName);
            if (item == null)
            {
                return;
            }
            var file = item as StorageFile;
            await file?.DeleteAsync();
        }

        public static T GetLocalSetting<T>(string key)where T :class
        {
            var settings=Windows.Storage.ApplicationData.Current.LocalSettings;
            object value;
            settings.Values.TryGetValue(key, out value);
            return value as T;
        }
        public static void SetLocalSetting<T>(string key,T value) where T : class
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.Values[key] = value;
        }
    }
}
