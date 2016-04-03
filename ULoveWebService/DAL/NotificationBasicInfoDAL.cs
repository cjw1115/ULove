using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ULoveWebService.Models;

namespace ULoveWebService.DAL
{
    public class NotificationBasicInfoDAL
    {
        private ULoveWebServiceContext db = new ULoveWebServiceContext();
        public void Save(NotificationBasicInfo basicinfo)
        {
            var item=db.NotificationBasicInfo.FirstOrDefault();
            if (item == null)
            {
                db.NotificationBasicInfo.Add(basicinfo);
            }
            else
            {
                item.AccessToken = basicinfo.AccessToken;
                item.TokenType = basicinfo.TokenType;
            }
            db.SaveChanges();

        }
        public NotificationBasicInfo Select()
        {
            var item = db.NotificationBasicInfo.FirstOrDefault();
            return item;
        }
    }
}