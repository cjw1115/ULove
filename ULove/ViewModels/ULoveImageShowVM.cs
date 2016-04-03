using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ULove.Models;
using ULove.Services;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using ULove.Manager;

namespace ULove.ViewModels
{
    public class ULoveImageShowVM:ViewModelBase
    {
        //代表两张UloveImage
        private ULoveImageEntity _uloveImage;
        public ULoveImageEntity ULoveImage
        {
            get { return _uloveImage; }
            set
            {
                _uloveImage = value;
                if(value!=null)
                {
                    ImagesSource.Clear();
                    ImagesSource.Add(value.ImageSource1);
                    ImagesSource.Add(value.ImageSource2);
                }
            }
        }

        private IList<ImageSource> _imagesSource;
        public IList<ImageSource> ImagesSource
        {
            get
            {
                return _imagesSource;
            }

            set
            {
                Set(ref _imagesSource, value);
            }
        }

        private ImageSource _currentImage;
        public ImageSource CurrentImage
        {
            get { return _currentImage; }
            set
            { Set(ref _currentImage, value); }
        }

        //代表页面是否由推荐通道打开
        public bool IsShared { get; set; } = false;

        public ICommand DownloadCommand { get; set; }
        public ICommand SetBackgroundCommand { get; set; }
        private async void Download()
        {
            LocalImagesManager localImageManager = new LocalImagesManager();
            ULoveImageEntityDb entityDb = await localImageManager.MapImageEntityDb(ULoveImage);
            localImageManager.localImageDal.Add(entityDb);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("下载完成");
        }
        private async void SetBackground()
        {
            
            if (SetBackgroundLib.BackgroundService.IsSupportSetBackground() == false)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设备不支持！");
                return;
            }
            if (CurrentImage != null)
            {
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var tempFile = await ULoveExtendMethod.SaveImageToFile("background.jpg", CurrentImage, localFolder);

                //MessageDialog dlg = new MessageDialog("是否推荐给ta？");
                //dlg.Commands.Add(new UICommand("推荐给ta", (c) => { IsShare = true; }));
                //dlg.Commands.Add(new UICommand("不用了", (c) => { IsShare = false; }));

                //GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(dlg);
                
                
                bool re = await SetBackgroundLib.BackgroundService.SetBackground(tempFile);
                if (re == true)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设置成功~");


                }
                else
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("设置失败~");
                }
                await tempFile.DeleteAsync();

                //打开推送对话框
                if (!IsShared)
                {
                    Open = true;
                }
                IsShared = false;
            }

        }
        private bool _open = false;
        public bool Open
        {
            get { return _open; }
            set { Set(ref _open, value); }
        }
        public ICommand OkCommand { get; set; }
        private async void Ok(object param)
        {
            string msg = param as string;
            var user = await Manager.UserManager.GetUser();
            if (user != null)
            {
                var message = await ULove.Services.ULoveCoreService.ShareForLover(user, ULoveImage.ID, msg);
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>($"{message}");
            }
            
            Open = false;
        }
        public ICommand CancelCommand { get; set; }
        private void Cancel()
        {
            Open = false;
        }
        public ULoveImageShowVM()
        {

            ImagesSource = new List<ImageSource>();
            DownloadCommand = new RelayCommand(Download);
            SetBackgroundCommand = new RelayCommand(SetBackground);

            OkCommand = new RelayCommand<object>(Ok);
            CancelCommand = new RelayCommand(Cancel);

        }
    }
}
