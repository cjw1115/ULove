/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Helloworld"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ULove.Views;

namespace ULove.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            
            SimpleIoc.Default.Register<EditImageVM>();
            SimpleIoc.Default.Register<LocalImageVM>();
            SimpleIoc.Default.Register<MainViewVM>();
            SimpleIoc.Default.Register<OnlineImageVM>();
            SimpleIoc.Default.Register<ULoveImageShowVM>();
            SimpleIoc.Default.Register<UserVM>();
            SimpleIoc.Default.Register<SinaWeiboLoginVM>();
            SimpleIoc.Default.Register<SettingVM>();

            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register(() => navigationService);


        }
        public EditImageVM EditImage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditImageVM>();
            }
        }
        public OnlineImageVM OnlineImage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OnlineImageVM>();
            }
        }
        public LocalImageVM LocalImage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LocalImageVM>();
            }
        }
        public MainViewVM MainView
        {
            get { return SimpleIoc.Default.GetInstance < MainViewVM>(); }
        }
        public ULoveImageShowVM ULoveImageShow
        {
            get { return SimpleIoc.Default.GetInstance<ULoveImageShowVM>(); }
        }
        public INavigationService Navigation
        {
            get
            {
                return SimpleIoc.Default.GetInstance<INavigationService>();
            }
        }
        public UserVM User
        {
            get
            {
                return SimpleIoc.Default.GetInstance<UserVM>();
            }
        }

        public SinaWeiboLoginVM SinaWeiboLogin
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SinaWeiboLoginVM>();
            }
        }
        public SettingVM Setting
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SettingVM>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(typeof(EditImage).Name, typeof(EditImage));
            navigationService.Configure(typeof(LocalImageView).Name, typeof(LocalImageView));
            navigationService.Configure(typeof(OnlineImageView).Name, typeof(OnlineImageView));
            navigationService.Configure(typeof(MainView).Name, typeof(MainView));
            navigationService.Configure(typeof(ULoveImageShowView).Name, typeof(ULoveImageShowView));
            navigationService.Configure(typeof(UserView).Name, typeof(UserView));
            navigationService.Configure(typeof(SinaWeiboLoginView).Name, typeof(SinaWeiboLoginView));
            navigationService.Configure(typeof(Setting).Name, typeof(Setting));

            return navigationService;
        }
    }
}