using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ULoveWebService.Attributes;
using ULoveWebService.Models;
using ULoveWebService.Service;

namespace ULoveWebService.Controllers
{
    
    public class ULoveImagesController : ApiController
    {
       
        private ULoveWebServiceContext db = new ULoveWebServiceContext();

        
        // GET: api/ULoveImages
        public IQueryable<ULoveImage> GetULoveImages()
        {
            var list= db.ULoveImages;
            
            int index = Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.AbsolutePath);
            string host = Request.RequestUri.AbsoluteUri.Substring(0, index);
            foreach (var item in list)
            {

                //item.ImageSource1Uri = host+"/Images/"+item.ImageSource1Uri;
                //item.ImageSource2Uri = host + "/Images/"+ item.ImageSource2Uri;

                item.ImageSource1Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + item.ImageSource1Uri;
                item.ImageSource2Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + item.ImageSource2Uri;
            }
            return list;
        }

        public IQueryable<ULoveImage> GetULoveImages([FromUri]int? start,[FromUri]int ?count)
        {
            if (start == null || count == null)
                return null;
            var allImages = db.ULoveImages;

            var first = allImages.First();
            var list = allImages.Where(m => m.ID >= first.ID + start).Take(count.Value);

            int index = Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.AbsolutePath);
            string host = Request.RequestUri.AbsoluteUri.Substring(0, index);
            foreach (var item in list)
            {

                //item.ImageSource1Uri = host + "/Images/" + item.ImageSource1Uri;
                //item.ImageSource2Uri = host + "/Images/" + item.ImageSource2Uri;
                item.ImageSource1Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + item.ImageSource1Uri;
                item.ImageSource2Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + item.ImageSource2Uri;
            }
            return list;
        }

        // GET: api/ULoveImages/5
        [ResponseType(typeof(ULoveImage))]
        public IHttpActionResult GetULoveImage(int id)
        {
            ULoveImage uLoveImage = db.ULoveImages.Find(id);
            if (uLoveImage == null)
            {
                return NotFound();
            }
            int index = Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.AbsolutePath);
            string host = Request.RequestUri.AbsoluteUri.Substring(0, index);
            //uLoveImage.ImageSource1Uri = host + "/Images/" + uLoveImage.ImageSource1Uri;
            //uLoveImage.ImageSource2Uri = host + "/Images/" + uLoveImage.ImageSource2Uri;
            uLoveImage.ImageSource1Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + uLoveImage.ImageSource1Uri;
            uLoveImage.ImageSource2Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + uLoveImage.ImageSource2Uri;

            return Ok(uLoveImage);
            
        }

        //// Post: api/ULoveImages/
        //[ResponseType(typeof(ULoveImage))]
        //public IHttpActionResult GetULoveImage(int id)
        //{
        //    ULoveImage uLoveImage = db.ULoveImages.Find(id);
        //    if (uLoveImage == null)
        //    {
        //        return NotFound();
        //    }
        //    int index = Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.AbsolutePath);
        //    string host = Request.RequestUri.AbsoluteUri.Substring(0, index);
        //    //uLoveImage.ImageSource1Uri = host + "/Images/" + uLoveImage.ImageSource1Uri;
        //    //uLoveImage.ImageSource2Uri = host + "/Images/" + uLoveImage.ImageSource2Uri;
        //    uLoveImage.ImageSource1Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + uLoveImage.ImageSource1Uri;
        //    uLoveImage.ImageSource2Uri = "http://7xryan.com1.z0.glb.clouddn.com/" + uLoveImage.ImageSource2Uri;

        //    return Ok(uLoveImage);

        //}
        
        //POST: api/ULoveImages

        [TokenAuthorize]
        [HttpPost]
        [Route("api/gettoken")]
        public IHttpActionResult PostULoveImage(UploadImage uploadImage)
        {
            Result result = new Result();
            if(uploadImage==null)
            {
                result.Message = "数据错误";
                return Json(result);
            }
            if(string.IsNullOrEmpty(uploadImage.ImageName1)|| string.IsNullOrEmpty(uploadImage.ImageName2))
            {
                result.Message = "数据错误";
                return Json(result);
            }
            ULoveImage uloveimage = new ULoveImage();
            uloveimage.Title = string.IsNullOrEmpty(uploadImage.Title) ? uploadImage.ImageName1 : uploadImage.Title;
            uloveimage.Describe = uploadImage.Describe;
            uloveimage.ImageSource1Uri = uploadImage.ImageName1;
            uloveimage.ImageSource2Uri = uploadImage.ImageName2;
            db.ULoveImages.Add(uloveimage);
            try
            {
                db.SaveChanges();
                Service.QiniuService qiniu = new QiniuService();
                result.Data=qiniu.GetToken();
                result.Message = "成功";
                return Json(result);
            }
            catch(DbUpdateException dbexception)
            {
                result.Message = "存储错误";
                return Json(result);
            }
        }

        // DELETE: api/ULoveImages/5
        [ResponseType(typeof(ULoveImage))]
        public IHttpActionResult DeleteULoveImage(int id)
        {
            ULoveImage uLoveImage = db.ULoveImages.Find(id);
            if (uLoveImage == null)
            {
                return NotFound();
            }

            db.ULoveImages.Remove(uLoveImage);
            db.SaveChanges();

            return Ok(uLoveImage);
        }
        

    }
}