using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULove.Models;
using ULove.ViewModels;
using ULove.Views;
using Windows.UI.Xaml;

namespace ULove.Services
{
    public class ActivedService
    {
        /// <summary>
        /// 通过Toast通知激活应用
        /// </summary>
        /// <returns></returns>
        public static async Task ToastActived()
        {
            var user = await Manager.UserManager.GetUser();
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;

            if (user != null)
            { //GetNotificationType()  1.message 2.imageshare 3.bind id 4.system notification
                ULove.DAL.ULoveWebDAL webdal = new DAL.ULoveWebDAL(true,user.Uid);
                var re=await webdal.Get($"{App.rootUri}/api/getnotificationtype?uid={user.Uid}");
                if (re != null && re.Data != null)
                {
                    int code = int.Parse(re.Data.ToString());
                    switch (code)
                    {
                        case 1:
                            break;
                        case 2:
                            ULoveShareEntity sharedinfo = await Services.ULoveCoreService.GetLastSharedInfo(user);
                            ULoveImage ULoveImage = await ULove.Manager.OnlineImageManager.GetUloveImage(sharedinfo.ULoveImageID);

                            ULoveImageEntity entity = await Manager.OnlineImageManager.GetImageEntity(ULoveImage);
                            
                            locator.ULoveImageShow.ULoveImage = entity;
                            locator.ULoveImageShow.IsShared = true;
                            locator.Navigation.NavigateTo(typeof(ULoveImageShowView).Name);
                            break;
                        case 3:
                            var uidof=await ULoveCoreService.GetBindRequst(user.Uid);
                            if (!string.IsNullOrEmpty(uidof))
                            {
                                locator.User.BindRequestID = uidof;
                                locator.Navigation.NavigateTo(typeof(MainView).Name,"2");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
