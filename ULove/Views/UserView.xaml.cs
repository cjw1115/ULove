using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ULove.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserView : Page
    {
        public UserView()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
            this.Loaded += UserView_Loaded;
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this,
                 (s) =>
                 {
                     notificationBar.ShowMessage(s);
                     //Services.NotificationService.Notify(s);
                 });
        }

        private void UserView_Loaded(object sender, RoutedEventArgs e)
        {
            itemStoryboard.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetNavigateToCommand(this)?.Execute(null);
        }
        public static readonly DependencyProperty NavigateToCommandProperty = DependencyProperty.Register("NavigateToCommand", typeof(ICommand), typeof(UserView), new PropertyMetadata(null));
        public static ICommand GetNavigateToCommand(DependencyObject o)
        {
            return (ICommand)o.GetValue(NavigateToCommandProperty);
        }
        public static void SetNavigateToCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(NavigateToCommandProperty, value);
        }

    }
}
