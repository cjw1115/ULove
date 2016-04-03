using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ULove.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WorkshopView : Page
    {
        StorageFile imageFile;
        public WorkshopView()
        {
           
            this.InitializeComponent();
        }

        private async void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            imageFile = await picker.PickSingleFileAsync();
            if (imageFile == null)
            {
                return;
            }
            BitmapImage bitmap = new BitmapImage();
            using (var rms = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmap.SetSource(rms);
                img.Source = bitmap;
            }
        }

        private async void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var user=await Manager.UserManager.GetUser();
            DAL.ULoveWebDAL webdal = new DAL.ULoveWebDAL(true, user.Uid);
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("Title", "test");
            param.Add("Describe", "Describe");
            param.Add("ImageName1", "ImageName1");
            param.Add("ImageName2", "ImageName2");
            param.Add("AuthorID", user.Uid);

            var re=await webdal.Post($"{App.rootUri}api/gettoken", param);

            string token = re.Data.ToString();
            string uri = "http://up.qiniu.com/";

            HttpClient client = new HttpClient();
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);

            StringBuilder sb = new StringBuilder();
            sb.Append($"----frontier{Environment.NewLine}");
            sb.Append($"Content-Disposition:form-data; name=\"token\"{Environment.NewLine}");
            sb.Append(token);
            sb.Append($"{Environment.NewLine}");
            sb.Append($"----frontier\r\n");
            sb.Append($"Content-Disposition:form-data; name=\"key\"{Environment.NewLine}");
            sb.Append("ImageName1");
            sb.Append($"{Environment.NewLine}");
            sb.Append($"----frontier\r\n");
            sb.Append($"Content-Disposition:form-data; name=\"file\";filename=\"ImageName1\"{Environment.NewLine}");
            sb.Append($"Content-Type:application/octet-stream{Environment.NewLine}");
            using (var stream = await imageFile.OpenStreamForReadAsync())
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                sb.Append(Encoding.UTF8.GetString(buffer));
            }
            sb.Append(Environment.NewLine);
            sb.Append($"----frontier--{Environment.NewLine}");

            StringContent content = new StringContent(sb.ToString());
            request.Content = content;
            request.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data;boundary=--frontier");
            HttpResponseMessage response=await client.SendAsync(request);

        }
    }
}
