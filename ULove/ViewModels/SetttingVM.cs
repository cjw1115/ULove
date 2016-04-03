using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULove.ViewModels
{
    public class SettingVM:ViewModelBase
    {
        private bool _isOn;
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                Set(ref _isOn, value);
                if (value == true)
                {
                    On();
                }
                else
                {
                    Off();
                }
            }
        }
        public SettingVM()
        {
            var status = Windows.Storage.ApplicationData.Current.LocalSettings.Values["notification"];
            if(status!=null)
                Set(ref _isOn, (bool)status);
        }
        private async void On()
        {
            var user = await Manager.UserManager.GetUser();
            if (user == null)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请先登录");
                Set(ref _isOn, false);
                return;
            }
            var service = new Services.NotificationService();
            var re=await service.SetNotificationChannel(user.Uid);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["notification"] = true;
        }
        private async void Off()
        {
            
            var user = await Manager.UserManager.GetUser();
            if (user == null)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>("请先登录");
                Set(ref _isOn, true);
                return;
            }
            var service = new Services.NotificationService();
            var re = await service.CancelNotificationChannel(user.Uid);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(re);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["notification"] = false;
        }
    }
}
