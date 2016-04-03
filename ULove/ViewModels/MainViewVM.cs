using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ULove.ViewModels
{
    public class MainViewVM : ViewModelBase
    {
        private Frame _pageContainer;
        public Frame PageContainer
        {
            get { return _pageContainer; }
            set
            {

                Set(ref _pageContainer, value);
                //if (value != null)
                //    NavigateSet(value);
            }
        }
        #region 页面内部导航
        private void NavigateSet(Frame frame)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            frame.Navigated += RootFrame_Navigated;
        }
        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var frame = (Frame)((Page)e.Content).Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (frame).BackStack.Any() ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = PageContainer;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }
        #endregion

        public ICommand ItemClickCommand { get; set; }
        private async void ItemClick(object param)
        {
            string pageName = (string)param;

            if (pageName != null && PageContainer != null)
            {
                switch (pageName)
                {
                    case "0":
                        MainTitle = "在线图库";
                        PageContainer.Navigate(typeof(Views.OnlineImageView));
                        break;
                    case "1":
                        MainTitle = "本地图库";
                        PageContainer.Navigate(typeof(Views.LocalImageView));
                        break;
                    case "2":
                        MainTitle = "个人中心";
                        var user = await Manager.UserManager.GetUser();
                       ViewModelLocator locator= Application.Current.Resources["Locator"] as ViewModelLocator;
                        locator.User.Logined += (o, e) => 
                        {
                            var args = e as ULove.ViewModels.UserVM.LoginEventArgs;
                            if (args.Status==true)
                                PageContainer.Navigate(typeof(Views.UserView));
                            else
                                PageContainer.Navigate(typeof(Views.UserLoginView));
                        };
                        locator.User.Logouted += (o,e) => { PageContainer.Navigate(typeof(Views.UserLoginView)); };
                        if (user == null)
                        {
                            PageContainer.Navigate(typeof(Views.UserLoginView));
                        }
                        else
                        {   
                            PageContainer.Navigate(typeof(Views.UserView));
                        }
                        break;
                    case "3":
                        MainTitle = "设置";
                        PageContainer.Navigate(typeof(Views.Setting));
                        break;
                    default:
                        break;
                }

            }
        }

        public ICommand NavigateToCommand { get; set; }
        private async void NavigateTo(object param)
        {
            var e=param as Models.SinaWeiboLoginParam;
            if (e != null)
            {
                var user=await Manager.UserManager.GetUser();
                user.Uid = e.Weribouser.id;
                user.NickName= e.Weribouser.screen_name;
                user.ProfileImageUri = e.Weribouser.profile_image_url;
                user.Gender = e.Weribouser.gender;

                PageContainer.Navigate(typeof(Views.UserView));
            }
            string index = param as string;
            if (index == "2")
            {
                ItemClick(param);
            }
            
        }

        //public ICommand SettingCommand { get; set; }
        //private async void Setting()
        //{
        //    MainTitle = "设置";
        //    PageContainer.Navigate(typeof(Views.Setting));
        //}

        private string _mainTitle;
        public string MainTitle
        {
            get
            {
                return _mainTitle;
            }

            set
            {
                Set(ref _mainTitle, value);
            }
        }
        public MainViewVM()
        {
            PageContainer = new Frame();
            ItemClickCommand = new RelayCommand<object>(ItemClick);
            NavigateToCommand = new RelayCommand<string>(NavigateTo);
            //SettingCommand = new RelayCommand(Setting);

            ItemClick("0");//默认跳转到在线图库
        }
    }
}
