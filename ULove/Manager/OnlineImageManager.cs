using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ULove.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace ULove.Manager
{
    public class OnlineImageManager
    {
        /// <summary>
        /// 获取图片相关路由地址Api/ULoveImages
        /// </summary>
        private static readonly string baseUri = "Api/ULoveImages";

        /// <summary>
        /// 获取指定的ULoveImage
        /// </summary>
        /// <param name="id">ULoveImage中的ID</param>
        /// <returns>ULoveImage对象</returns>
        public static async Task<ULoveImage> GetUloveImage(int id)
        {
            ULoveImage uLoveImage = null;
            //HttpClient baseClinet = new HttpClient();
            //try
            //{

            //    HttpResponseMessage response = await baseClinet.GetAsync($"{App.rootUri}{baseUri}?id={id}");
            //    string jsonString = await response.Content.ReadAsStringAsync();
            //    uLoveImage = JsonConvert.DeserializeObject<ULoveImage>(jsonString);

            //}
            //catch(HttpRequestException requestException)
            //{

            //}
            //finally
            //{
            //    baseClinet?.Dispose();
            //}
            //return uLoveImage;
            var webdal = new DAL.ULoveWebDAL();
            var re=await webdal.Get<ULoveImage>($"{App.rootUri}{baseUri}?id={id}");
            return re;
        }

        /// <summary>
        /// 获取所有的ULoveImage
        /// </summary>
        /// <returns>返回所有获取到的数据，如果请求错误，就返回null</returns>
        public static async Task<IEnumerable<ULoveImage>> GetUloveImages()
        {
            IEnumerable<ULoveImage> uloveImages = null;
            //HttpClient baseClinet = new HttpClient();
            //try
            //{
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.Method = HttpMethod.Get;
            //    request.RequestUri = new Uri(baseUri);
            //    HttpResponseMessage response = await baseClinet.SendAsync(request);
            //    string jsonString = await response.Content.ReadAsStringAsync();
            //    uloveImages = JsonConvert.DeserializeObject<IList<ULoveImage>>(jsonString);

            //}
            //catch(HttpRequestException requestException)
            //{

            //}
            //finally
            //{
            //    baseClinet?.Dispose();
            //}
            //return uloveImages;
            var webdal = new DAL.ULoveWebDAL();
            var re = await webdal.Get<IList<ULoveImage>>(baseUri);
            return re;
        }

        /// <summary>
        /// 获取指定区间的ULoveImage
        /// </summary>
        /// <param name="start">开始的ID号</param>
        /// <param name="count">要获取的数目</param>
        /// <returns>返回获取到的数据,如果请求错误，返回null</returns>
        public static async Task<IEnumerable<ULoveImage>> GetUloveImages(int? start, int? count)
        {
            var webdal = new DAL.ULoveWebDAL();
            var re = await webdal.Get<IList<ULoveImage>>($"{App.rootUri}{baseUri}?start={start}&count={count}");
            return re;
        }

        /// <summary>
        /// 把同于传输的ULoveImage转换为用于数据绑定的ULoveImageEntity实体，并添加到ULoveImageEntity集合中
        /// </summary>
        /// <param name="onlineImages">用于存储转换成功的ULoveImageEntity的集合</param>
        /// <param name="uloveImages">用于获取ULoveImage的集合</param>
        public static async Task GetImageEntities(OnlineImages onlineImages, IEnumerable<ULoveImage> uloveImages)
        {
            HttpClient client = null;
            if (uloveImages == null|| uloveImages.Count() <= 0)
                return;
            try
            {
                client = new System.Net.Http.HttpClient();
                foreach (var item in uloveImages)
                {
                    ULoveImageEntity uLoveImageEntity = new ULoveImageEntity();
                    uLoveImageEntity.Title = item.Title;
                    uLoveImageEntity.ID = item.ID;
                    uLoveImageEntity.Describe = item.Describe;
                    var source1= await GetUloveImageEntity(client, item.ImageSource1Uri);
                    var source2 = await GetUloveImageEntity(client, item.ImageSource2Uri);
                    if (source1 != null && source2 != null)
                    {
                        uLoveImageEntity.ImageSource1 = source1;
                        uLoveImageEntity.ImageSource2 = source2;
                        onlineImages.Add(uLoveImageEntity);
                    }
                    
                }
            }
            catch (HttpRequestException  requestException)
            {

            }
            catch(Exception exception)
            {

            }
            finally
            {
                client?.Dispose();
            }
        }

        /// <summary>
        /// 通过指定的uri获取图片数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="imageUri"></param>
        /// <returns></returns>
        private async static Task<ImageSource> GetUloveImageEntity(HttpClient client, string imageUri)
        {

            using (var re = await client.GetAsync(imageUri))
            {
                var stream = await re.Content.ReadAsStreamAsync();
                var ras = stream.AsRandomAccessStream();
                WriteableBitmap bitmap1 = new WriteableBitmap(1, 1);
                await bitmap1.SetSourceAsync(ras);
                return bitmap1;
            }
                
        }


        public static async Task<ULoveImageEntity> GetImageEntity(ULoveImage uloveimage)
        {
            HttpClient client = null;
            try
            {
                client = new System.Net.Http.HttpClient();
                ULoveImageEntity uLoveImageEntity = new ULoveImageEntity();
                uLoveImageEntity.Title = uloveimage.Title;
                uLoveImageEntity.ID = uloveimage.ID;
                uLoveImageEntity.Describe = uloveimage.Describe;
                uLoveImageEntity.ImageSource1 = await GetUloveImageEntity(client, uloveimage.ImageSource1Uri);
                uLoveImageEntity.ImageSource2 = await GetUloveImageEntity(client, uloveimage.ImageSource2Uri);
                return uLoveImageEntity;
            }
            catch (HttpRequestException requestException)
            {
                return null;
            }
            finally
            {
                client?.Dispose();
            }   
        }
    }
}
