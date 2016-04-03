using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;

namespace ULoveWebService.Models
{
    public class ULoveWebServiceContextInitializer : DropCreateDatabaseIfModelChanges<ULoveWebServiceContext>
    {
        protected override void Seed(ULoveWebServiceContext context)
        {
            List<ULoveImage> images = new List<ULoveImage>();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(basePath, "Images");
            string[] files=Directory.GetFiles(imagePath);

            for (int i = 0; i < files.Length; i+=2)
            {
                ULoveImage image = new ULoveImage();
                image.Title = Path.GetFileNameWithoutExtension(files[i]);
                image.Describe = i.ToString();
                image.Like = 0;
                image.ImageSource1Uri = Path.GetFileName(files[i]);
                image.ImageSource2Uri = Path.GetFileName(files[i+1 ]);
                context.ULoveImages.Add(image);
            }

            context.NotificationBasicInfo.Add(new NotificationBasicInfo { Sid = "ms-app://s-1-15-2-3769682262-1369527739-2206876657-291492137-2158702331-644568056-1155916071", Secret = "cl1s6bGbSQveagHIzrfzQ96NSWUzAjTD" });
            context.SaveChanges();
            
        }
        public ULoveWebServiceContextInitializer()
        {
        }
    }
}