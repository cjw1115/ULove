using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ULove.Models;
using ULove.Services;
using Windows.UI.Xaml;
using ULove.Manager;
using ULove.Views;

namespace ULove.ViewModels
{
    public class UserVM:ViewModelBase
    {
        private UserEntity _user;
        public UserEntity User
        {
            get { return _user; }
            set
            {
                Set(ref _user, value);
                if (!string.IsNullOrEmpty(value.UidOfU))
                {
                    Bind = true;
                }
                else
                {
                    Bind = false;
                }
            }
        }
        private UserEntity _lover;
        public UserEntity Lover
        {
            get { return _lover; }
            set { Set(ref _lover, value); }
        }

        private ULoveShareEntity _mysuloveSharedInfo;
        public ULoveShareEntity MyULoveSharedInfo
        {
            get { return _mysuloveSharedInfo; }
            set
            {
                if (value != null)
                {
                    Set(ref _mysuloveSharedInfo, value);
                }
            }
        }
        private ULove.Models.ULoveImage _myuloveImage;
        public ULoveImage MyULoveImage
        {
            get { return _myuloveImage; }
            set { Set(ref _myuloveImage, value); }
        }

        private ULoveShareEntity _loversuloveSharedInfo;
        public ULoveShareEntity LoverULoveSharedInfo
        {
            get { return _loversuloveSharedInfo; }
            set
            {
                if (value != null)
                {
                    Set(ref _loversuloveSharedInfo, value);
                }
            }
        }
        private ULove.Models.ULoveImage _loveruloveImage;
        public ULoveImage LoverULoveImage
        {
            get { return _loveruloveImage; }
            set { Set(ref _loveruloveImage, value); }
        }

        private bool _bind;
        public bool Bind
        {
            get { return _bind; }
            set { Set(ref _bind, value); }
        }

        private string _loverUid;
        public string LoverUid
        {
            get { return _loverUid; }
            set { Set(ref _loverUid, value); }
        }

        public ULove.Services.WeiboCode Code { get; set; }
        public UserVM()
        {
            
            SinaWeiboLoginCommand = new RelayCommand(SinaWeiboLogin);
            NavigateToCommand = new RelayCommand(NavigateTo);
            LogoutCommand = new RelayCommand(Logout);
            BindLoverCommand = new RelayCommand(BindLover);
            UnBindLoverCommand = new RelayCommand(UnBindLover);
            MyClickCommand = new RelayCommand(MyClick);
            LoverClickCommand = new RelayCommand(LoverClick);
            AcceptCommand = new RelayCommand(Accept);
            RefuseCommand = new RelayCommand(Refuse);

            
        }

        public ICommand SinaWeiboLoginCommand { get; set; }
        public void SinaWeiboLogin()
        {
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.Navigation.NavigateTo(typeof(ULove.Views.SinaWeiboLoginView).Name,Logined);
        }

        public ICommand BindLoverCommand { get; set; }
        public async void BindLover()
        {
            var re=await ULoveCoreService.BindLover(User,LoverUid);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);

            GetCloudUserInfo();
        }

        public ICommand UnBindLoverCommand { get; set; }
        public async void UnBindLover()
        {
            var re = await Services.ULoveCoreService.UnBindLover(User);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);
            GetCloudUserInfo();
            GetSharedInfo();
        }

        public ICommand NavigateToCommand { get; set; }
        private async void NavigateTo()
        {
            User = await Manager.UserManager.GetUser();
            GetCloudUserInfo();
            GetBindLover();
            GetSharedInfo();
            GetLoverSharedInfo();
            GetBindRequest();
        }

        public ICommand MyClickCommand { get; set; }
        private async void MyClick()
        {
            ULoveImageEntity entity = await Manager.OnlineImageManager.GetImageEntity(MyULoveImage);
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.ULoveImageShow.ULoveImage = entity;
            locator.ULoveImageShow.IsShared = true;
            locator.Navigation.NavigateTo(typeof(ULoveImageShowView).Name);
        }
        public ICommand LoverClickCommand { get; set; }
        private async void LoverClick()
        {
            ULoveImageEntity entity = await Manager.OnlineImageManager.GetImageEntity(LoverULoveImage);
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.ULoveImageShow.ULoveImage = entity;
            locator.ULoveImageShow.IsShared = true;
            locator.Navigation.NavigateTo(typeof(ULoveImageShowView).Name);
        }

        //接收绑定请求
        public ICommand AcceptCommand { get; set; }
        private async void Accept()
        {
            var re=await ULoveCoreService.AcceptBindLover(User.Uid, BindRequestID);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);
            GetCloudUserInfo();
            GetBindLover();
            BindRequestID = string.Empty;//手动

            ShowHeart = true;
        }
        //拒绝绑定请求
        public ICommand RefuseCommand { get; set; }
        private async void Refuse()
        {
            var re = await ULoveCoreService.RefuseBindLover(User.Uid, BindRequestID );
            //GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);
            GetCloudUserInfo();
            BindRequestID = string.Empty;//手动
        }

        private bool _showHeart;

        public bool ShowHeart
        {
            get { return _showHeart; }
            set { Set(ref _showHeart, value); }
        }

        private string _bindRequestID;
        public string BindRequestID
        {
            get { return _bindRequestID; }
            set { Set(ref _bindRequestID, value); }
        }
        private async void GetBindLover()
        {
            Lover = await Services.ULoveCoreService.GetLoverInfo(User.Uid);
        }
        private async void GetCloudUserInfo()
        {
            if (User != null)
            {
                var re=await Manager.UserManager.GetCloudUser(User);//获取个人信息后，随即获取lover信息
                if (re != null)
                {
                    User = re;
                    await Manager.UserManager.SetUser(User);
                }
            }
        }
        private async Task GetSharedInfo()
        {
            if (User!=null&&!string.IsNullOrEmpty(User.UidOfU))
            {
                MyULoveSharedInfo= await ULoveCoreService.GetSharedInfo(User);
                if (MyULoveSharedInfo != null)
                {
                    var re = await OnlineImageManager.GetUloveImage(MyULoveSharedInfo.ULoveImageID);
                    MyULoveImage = re;
                }
            }

        }
        private async Task GetLoverSharedInfo()
        {
            if (User != null&& !string.IsNullOrEmpty(User.UidOfU))
            {
                LoverULoveSharedInfo = await ULoveCoreService.GetLastSharedInfo(User);
                if (LoverULoveSharedInfo != null)
                {
                    var re = await OnlineImageManager.GetUloveImage(LoverULoveSharedInfo.ULoveImageID);
                    LoverULoveImage = re;
                }
            }
        }
        private async Task GetBindRequest()
        {
            if (User != null)
            {
                var uidof = await ULoveCoreService.GetBindRequst(User.Uid);
                if (uidof == null)
                {
                    BindRequestID = string.Empty;
                }
                else
                    BindRequestID = uidof;
            }
            
        }
        public ICommand LogoutCommand { get; set;}
        private void Logout()
        {
            Manager.UserManager.Logout();
            Logouted?.Invoke(null, null);
        }

        public event EventHandler Logined;
        public event EventHandler Logouted;
        public class LoginEventArgs : EventArgs
        {
            public bool Status { get; set; }
        }
    }
}
