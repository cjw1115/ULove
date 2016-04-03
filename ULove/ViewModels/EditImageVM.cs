using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ULove.Models;
using ULove.Services;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
namespace ULove.ViewModels
{
    public class EditImageVM : ViewModelBase
    {
        public ICommand ClipCommand { get; set; }
        public ICommand SaveToLibCommand { get; set; }
        public ICommand SetBackgroundCommand { get; set; }


        private ULoveImageEntity _imageEntity;
        public ULoveImageEntity ImageEntity
        {
            get { return _imageEntity; }
            set
            {
                _imageEntity = value;
                if (value != null)
                {
                    ImagesSource.Clear();
                    ImagesSource.Add(value.ImageSource1);
                    ImagesSource.Add(value.ImageSource2);
                }
            }
        }

        private ImageSource _currentImage;
        public ImageSource CurrentImage
        {
            get { return _currentImage; }
            set
            { Set(ref _currentImage, value); }
        }


        private IList<ImageSource> _imagesSource;
        public IList<ImageSource> ImagesSource
        {
            get { return _imagesSource;}
            set { Set(ref _imagesSource, value); }
        }

        public EditImageVM()
        {
            ImagesSource = new List<ImageSource>();
            ClipCommand = new RelayCommand(Clip);
            SaveToLibCommand = new RelayCommand(SaveToLib);
            SetBackgroundCommand = new RelayCommand(SetBackground);
        }
        private void Clip()
        { }
        private async void SaveToLib()
        {
            var picLib = await Windows.Storage.StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            var folder=picLib.SaveFolder;
            var re1=await ULoveExtendMethod.SaveImageToFile(ImageEntity.Title+"1.jpg", ImageEntity.ImageSource1, folder);
            var re2 = await ULoveExtendMethod.SaveImageToFile(ImageEntity.Title+"2.jpg", ImageEntity.ImageSource2, folder);
            if (re1 != null&&re2!=null)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("同步到图片库成功");
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("同步到图片库失败");
            }
        }
        /// <summary>
        /// 设置壁纸
        /// </summary>
        private async void SetBackground()
        {
            if (SetBackgroundLib.BackgroundService.IsSupportSetBackground() == false)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设备不支持！");
                return;
            }
            if (CurrentImage!= null)
            {
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var tempFile = await ULoveExtendMethod.SaveImageToFile("background.jpg", CurrentImage, localFolder);
                bool re = await SetBackgroundLib.BackgroundService.SetBackground(tempFile);
                if (re == true)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设置成功~");
                }
                else
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设置失败~");
                }
                await tempFile.DeleteAsync(StorageDeleteOption.Default);
            }
            
        }
    }
}
