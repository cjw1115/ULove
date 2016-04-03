using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ULoveWebService.Models;

namespace ULoveWebService.DAL
{
    public class NotificationStutusDAL
    {
        private ULoveWebServiceContext db = new ULoveWebServiceContext();
        public void Set(string uid,int statuscode)
        {
            var item=db.NotifycationStatus.Where(m=>m.Uid==uid).FirstOrDefault();
            if (item == null)
            {
                NotifycationStatus status = new NotifycationStatus() { Uid = uid, Status = statuscode };
                db.NotifycationStatus.Add(status);
            }
            else
            {
                item.Status = statuscode;
            }
            db.SaveChanges();
        }
    }
}