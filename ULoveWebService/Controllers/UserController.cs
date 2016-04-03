using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using ULoveWebService.Attributes;
using ULoveWebService.Models;

namespace ULoveWebService.Controllers
{

    public class UserController : ApiController
    {
        private ULoveWebServiceContext db = new ULoveWebServiceContext();

        [TokenAuthorize]
        // GET: api/User
        public IQueryable<UserEntity> GetUsers()
        {
            return db.Users;
        }

        [TokenAuthorize]
        [Route("api/login")]
        [HttpPost]
        public void Login(UserEntity userEntity)
        {

        }

        // GET: api/User/5
        [TokenAuthorize]
        [ResponseType(typeof(UserEntity))]
        public IHttpActionResult GetUserEntity(string uid)
        {
            Result re = new Result();
            var user = db.Users.Where(m => m.Uid == uid).FirstOrDefault();
            if (user==null)
            {
                re.Message = "账号不存在";
            }
            re.Data = user;
            re.Message = "成功";
            return Json(re);
        }

        //// PUT: api/User/5
        //[TokenAuthorize]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutUserEntity(string uid, UserEntity userEntity)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (uid != userEntity.Uid)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(userEntity).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserEntityExists(uid))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/User
        //[TokenAuthorize]
        //[ResponseType(typeof(UserEntity))]
        //public IHttpActionResult PostUserEntity(UserEntity userEntity)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Users.Add(userEntity);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (UserEntityExists(userEntity.Uid))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = userEntity.Uid }, userEntity);
        //}

        //// DELETE: api/User/5
        //[TokenAuthorize]
        //[ResponseType(typeof(UserEntity))]
        //public IHttpActionResult DeleteUserEntity(string id)
        //{
        //    UserEntity userEntity = db.Users.Find(id);
        //    if (userEntity == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(userEntity);
        //    db.SaveChanges();

        //    return Ok(userEntity);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserEntityExists(string id)
        {
            return db.Users.Count(e => e.Uid == id) > 0;
        }


        private class Result
        {
            public object Data { get; set; }
            public string Message { get; set; }
        }

        [Route("api/share")]
        [TokenAuthorize]
        public IHttpActionResult ShareForLover(ULoveShareEntity shareModel)
        {
            Result result = new Result();

            string fromUid = shareModel.UidFrom;
            string toUid = shareModel.UidTo;
            int imageId = shareModel.ULoveImageID;
            if (fromUid == null)
            {
                result.Message = "参数错误";
                return Json(result);
            }
            if (toUid == null)
            {
                result.Message = "先找个对象吧(-｡-;)";
                return Json(result);
            }

            if (db.Users.Where(m => m.Uid == fromUid && m.UidOfU == toUid).FirstOrDefault() == null)
            {
                result.Message = "请先绑定Lover";
                return Json(result);
            }

            if (db.ULoveImages.Where(m => m.ID == imageId).FirstOrDefault() == null)
            {
                result.Message = "图片不存在";
                return Json(result);
            }

            shareModel.Time = DateTime.Now;
            db.ULoveShareEntity.Add(shareModel);
            try
            {
                db.SaveChanges();
                //

                //此处添加推送通知动作
                var notification = new Service.NotificationService(shareModel.UidTo, Service.NotificationType.ShareImage);//创建推送服务，并设置推送状态码

                var xmlstring = notification.GetTextToastString(shareModel.Message, Service.NotificationType.ShareImage);
                var uri = db.NotificationChannel.Where(m => m.uid == shareModel.UidTo).Select(m => m.ChannelUri).FirstOrDefault();
                if (!string.IsNullOrEmpty(uri))
                {
                    var re = notification.PostToWns(uri, xmlstring);
                    result.Message = re;
                }
                else
                {
                    result.Message = "推送服务没有开启";
                }
            }
            catch
            {
                result.Message = "系统异常";

            }
            return Json(result);
        }

        [Route("api/getshareinfo")]
        [TokenAuthorize]
        public IHttpActionResult GetShareInfo(string uid)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uid))
            {
                result.Message = "uid错误";
                return Json(result);
            }
            var item = db.ULoveShareEntity.Where(m => m.UidFrom == uid).OrderByDescending(m => m.Time).FirstOrDefault();
            if (item == null)
            {
                result.Message = "没有数据";
                return Json(result);
            }
            result.Data = item;
            result.Message = "成功";

            return Json(result);
        }

        [Route("api/getlastshareinfo")]
        [TokenAuthorize]
        public IHttpActionResult GetLastShareInfo(string uidofu)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uidofu))
            {
                result.Message = "uid错误";
                return Json(result);
            }
            var item = db.ULoveShareEntity.Where(m => m.UidTo == uidofu).OrderByDescending(m => m.Time).FirstOrDefault();
            if (item == null)
            {
                result.Message = "没有数据";
                return Json(result);
            }
            result.Data = item;
            result.Message = "成功";

            return Json(result);
        }


        [Route("api/bind")]
        [TokenAuthorize]
        [HttpGet]
        public IHttpActionResult Bind(string uid, string uidofu)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uidofu))
            {
                result.Message = "ID错误";
                return Json(result);
            }

            var user = db.Users.Where(m => m.Uid == uid).FirstOrDefault();
            if (!string.IsNullOrEmpty(user.UidOfU))
            {
                result.Message = "只能绑定一个Lover,请先解绑";
                return Json(result);
            }

            var lover = db.Users.Where(m => m.Uid == uidofu).FirstOrDefault();
            if (lover == null)
            {
                result.Message = "该ID不存在(-｡-;)";
                return Json(result);
            }
            if (!string.IsNullOrEmpty(lover.UidOfU))
            {
                result.Message = "该ID已和其他账号绑定";
                return Json(result);
            }
           

            //此处查找的是被绑定人uidofu在数据表中的记录
            var record = db.BindLoverRecord.Where(m => m.uid == uidofu).FirstOrDefault();
            if (record == null)
            {
                record = new BindLoverRecord { uid = uidofu, uidofu = uid, Date = DateTime.Now };
                db.BindLoverRecord.Add(record);
            }
            else
            {
                record.uidofu = uid;
                record.Date = DateTime.Now;
            }
            try
            {
                db.SaveChanges();
                
                //将绑定信息加入数据库，并通知被绑定人
                var notification = new Service.NotificationService(uidofu, Service.NotificationType.BindLover);//创建推送服务，并设置推送状态码

                var xmlstring = notification.GetTextToastString(uid, Service.NotificationType.BindLover);
                var uri = db.NotificationChannel.Where(m => m.uid == uidofu).Select(m => m.ChannelUri).FirstOrDefault();
                if (!string.IsNullOrEmpty(uri))
                {
                    var re = notification.PostToWns(uri, xmlstring);
                    result.Message = re;
                }
                else
                {
                    result.Message = "请求已提交";
                }
                return Json(result);
            }
            catch (DbUpdateException dbUpdateException)
            {
                result.Message = "数据库错误";
                return Json(result);
            }
        }

        [Route("api/getbindrequest")]
        [HttpGet]
        [TokenAuthorize]
        public IHttpActionResult GetBindRequest(string uid)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uid))
            {
                result.Message = "ID错误";
                return Json(result);
            }

            var user = db.BindLoverRecord.Where(m => m.uid == uid).FirstOrDefault();
            if (user == null || string.IsNullOrEmpty(user.uidofu))
            {
                result.Message = "当前没有绑定请求";
                return Json(result);
            }
            result.Data = user.uidofu;//将请求绑定的人的uid送到被绑定人的客户端
            result.Message = "当前有用户请求绑定账号";
            return Json(result);
        }

        [TokenAuthorize]
        [Route("api/acceptbindrequest")]
        [HttpGet]
        public IHttpActionResult AcceptBindRequest(string uid, string uidofu)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uid)|| string.IsNullOrEmpty(uidofu))
            {
                result.Message = "ID错误";
                return Json(result);
            }
            var user = db.Users.Where(m => m.Uid == uid).FirstOrDefault();
            var lover = db.Users.Where(m => m.Uid == uidofu).FirstOrDefault();
            if (user != null && lover!=null)
            {
                user.UidOfU = uidofu;
                lover.UidOfU = uid;
                var record = db.BindLoverRecord.Where(m => m.uid == uid && m.uidofu == uidofu).FirstOrDefault();
                if (record != null)
                {
                    db.BindLoverRecord.Remove(record);
                }

                try
                {
                    db.SaveChanges();
                    result.Message = "绑定成功";
                    return Json(result);
                }
                catch (DbUpdateException updateException)
                {
                    result.Message = "绑定失败";
                    return Json(result);
                }
               
                
                
            }
            else
            {

                result.Message = "绑定出错";
                return Json(result);
            }
        }

        [TokenAuthorize]
        [Route("api/refusebindrequest")]
        [HttpGet]
        public IHttpActionResult RefuseBindRequest(string uid, string uidofu)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(uidofu))
            {
                result.Message = "ID错误";
                return Json(result);
            }
            var record=db.BindLoverRecord.Where(m => m.uid == uid && m.uidofu == uidofu).FirstOrDefault();
            if (record != null)
            {
                db.BindLoverRecord.Remove(record);
            }
            try
            {
                db.SaveChanges();
                result.Message = "拒绝成功";
                return Json(result);
            }
            catch (DbUpdateException updateException)
            {
                result.Message = "拒绝失败";
                return Json(result);
            }
        }

        [TokenAuthorize]
        [Route("api/unbind")]
        [HttpGet]
        public IHttpActionResult UnBind(string uid)
        {
            Result result = new Result();

            if (string.IsNullOrEmpty(uid))
            {
                result.Message = "ID错误";
                return Json(result);
            }

            var user = db.Users.Where(m => m.Uid == uid).FirstOrDefault();
            if (string.IsNullOrEmpty(user.UidOfU))
            {
                result.Message = "还没有绑定Lover";
                return Json(result);
            }
            var lover= db.Users.Where(m => m.Uid == user.UidOfU).FirstOrDefault();
            if (lover == null)
            {
                result.Message = "要解绑的ID不存在";
                return Json(result);
            }
            lover.UidOfU = null;
            user.UidOfU = null;

            
            try
            {
                db.SaveChanges();
                result.Message = "解除绑定成功";
                return Json(result);
            }
            catch (DbUpdateException dbUpdateException)
            {
                result.Message = "服务器故障！";
                return Json(result);
            }
        }

        [TokenAuthorize]
        [Route("api/getbindlover")]
        [HttpGet]
        public IHttpActionResult GetBindLover(string uid)
        {
            Result re = new Result();
            var uidofu=db.Users.Where(m => m.Uid == uid).Select(m => m.UidOfU).FirstOrDefault();
            if (string.IsNullOrEmpty(uidofu))
            {
                re.Message = "没有绑定的账号";
            }
            var bindlover = db.Users.Where(m => m.Uid == uidofu).FirstOrDefault();
            re.Data = bindlover;
            re.Message = "成功";
            return Json(re);
        }
        [TokenAuthorize]
        [Route("api/setnotificationchannel")]
        [HttpPost]
        public IHttpActionResult SetNotificationChannel(Models.NotificationChannel model )
        {
            Result result = new Result();
            if (model == null)
            {
                result.Message = "数据错误";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.uid))
            {
                result.Message = "用户ID错误";
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.ChannelUri))
            {
                result.Message = "推送通道错误";
                return Json(result);
            }
            var user=db.NotificationChannel.Where(m => m.uid == model.uid).FirstOrDefault();
            if (user == null)
            {
                NotificationChannel item = new NotificationChannel { uid = model.uid, ChannelUri = model.ChannelUri };
                db.NotificationChannel.Add(item);
            }
            else
            {
                user.ChannelUri = model.ChannelUri;
            }
            try
            {
                db.SaveChanges();
                result.Message = "设置成功";
                return Json(result);
            }
            catch(DbUpdateException updateException)
            {
                result.Message = "数据库异常";
                return Json(result);
            }
            
        }

        [TokenAuthorize]
        [Route("api/cancelnotificationchannel")]
        [HttpGet]
        public IHttpActionResult CancelNotificationChannel(string uid)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(uid))
            {
                result.Message = "uid错误";
                return Json(result);
            }
            var item=db.NotificationChannel.Where(m => m.uid == uid).FirstOrDefault();
            if (item == null)
            {
                result.Message = "取消成功";
                return Json(result);
            }
            try
            {
                item.ChannelUri = null;

                db.SaveChanges();
                result.Message = "取消成功";
                return Json(result);
            }
            catch (DbUpdateException updateException)
            {
                result.Message = "数据库异常";
                return Json(result);
            }

        }

        [TokenAuthorize]
        [Route("api/getnotificationtype")]
        [HttpGet]
        public IHttpActionResult GetNotificationType(string uid)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(uid))
            {
                result.Message = "uid错误";
                return Json(result);
            }
            var item = db.NotifycationStatus.Where(m => m.Uid == uid).FirstOrDefault();
            if (item == null)
            {
                result.Message = "没有新通知";
                return Json(result);
            }
            result.Data = item.Status;
            result.Message = "获取新通知类型成功";
            return Json(result);
        }

        [Route("api/back/weibo")]
        [HttpGet]
        public IHttpActionResult back_weibo(string code)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(code))
            {
                result.Message = "授权码出错";
                return Json(result);
            }
            var token = Service.WeiboOauth.GetToken(code);
            if (token == null)
            {
                result.Message = "获取令牌出错";
                return Json(result);
            }
            var weibouser=Service.WeiboOauth.GetUserInfo(token);
            if (weibouser == null)
            {
                result.Message = "获取令牌出错";
                return Json(result);
            }

            var user=db.Users.Where(m => m.Uid == weibouser.id).FirstOrDefault();
            if (user != null)//已注册用户
            {

            }
            else//第一次登陆的用户
            {
                user = new UserEntity();
                user.Uid = weibouser.id;
                user.NickName = weibouser.screen_name;
                user.ProfileImageUri = weibouser.profile_image_url;
                user.Gender = weibouser.gender;

                db.Users.Add(user);
                db.SaveChanges();
            }
            //发放认证成功的token
            var guid = Guid.NewGuid();
            var authtoken=guid.ToString("N");
            user.Token = authtoken;
            
            db.SaveChanges();

            result.Data = user;
            result.Message = "登陆成功";
            return Json(result);
        }
    }
}