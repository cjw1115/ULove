using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using ULove.Models;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ULove.Services;
namespace ULove.Manager
{
    public class LocalImagesManager
    {
        public DAL.LocalImageDAL<ULoveImageEntityDb> localImageDal { get; set; }
        public LocalImagesManager()
        {
            localImageDal = new DAL.LocalImageDAL<ULoveImageEntityDb>(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }
        //public async Task LoadImages(StorageFolder imagesFolder)
        //{

        //    List<ImageEntityDb> images = new List<ImageEntityDb>();
        //    if (imagesFolder == null)
        //        return;
        //    var folder = imagesFolder;
        //    var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
        //    foreach (var item in files)
        //    {
        //        //BitmapImage bitmapImage = new BitmapImage();
        //        var ras = await item.OpenReadAsync();
        //        //bitmapImage.SetSource(ras);
        //         image = new ImageEntityDb();
        //        using (Stream stream = ras.AsStream())
        //        {
        //            byte[] buffer = new byte[stream.Length];
        //            stream.Read(buffer, 0, (int)stream.Length);
        //            image.ImageSource = buffer;
        //        }
        //        image.ImageTitle = item.Name;
        //        //image.ImageUri = item.Path;
        //        images.Add(image);
        //    }

        //    localImageDal.CreateTable();
        //    foreach (var item in images)
        //    {
        //        localImageDal.Add(item);
        //    }
        //}
        public async Task<ObservableCollection<ULoveImageEntity>> GetImages()
        {
            ObservableCollection<ULoveImageEntity> collection = new ObservableCollection<ULoveImageEntity>();

            var list = localImageDal.SelectAll();
            foreach (var item in list)
            {
                var image = await MapImageEntity(item);
                collection.Add(image);
            }
            return collection;
        }

        public void DeleteImage(ULoveImageEntity data)
        {
            localImageDal.Delete(data.ID);
        }
        public async Task<ULoveImageEntity> MapImageEntity(ULoveImageEntityDb entity)
        {
            ULoveImageEntity goal = new ULoveImageEntity();
            goal.ID = entity.ID;
            goal.Describe = entity.Describe;
            goal.Title = entity.Title;

            goal.ImageSource1 = await entity.ImageSource1.SaveToImageSource();
            goal.ImageSource2 = await entity.ImageSource2.SaveToImageSource();
            
            return goal;
        }
        public async Task<ULoveImageEntityDb> MapImageEntityDb(ULoveImageEntity entity)
        {
            ULoveImageEntityDb entityDb = new ULoveImageEntityDb();
            entityDb.ID = entity.ID;
            entityDb.Title = entity.Title;
            entityDb.Describe = entity.Describe;

            entityDb.ImageSource1=await entity.ImageSource1.SaveToBytesAsync();
            entityDb.ImageSource2 = await entity.ImageSource2.SaveToBytesAsync();
            return entityDb;

        }
    }
}
