using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Qiniu;
using Qiniu.RS;

namespace ULoveWebService.Service
{
    public class QiniuService
    {
        public QiniuService()
        {
            Qiniu.Conf.Config.ACCESS_KEY = ConfigurationManager.AppSettings["ACCESS_KEY"];
            Qiniu.Conf.Config.SECRET_KEY = ConfigurationManager.AppSettings["SECRET_KEY"];
        }
        public string GetToken()
        {
            string bucket = ConfigurationManager.AppSettings["uloveimages"];

            //普通上传,只需要设置上传的空间名就可以了,第二个参数可以设定token过期时间
            PutPolicy put = new PutPolicy(bucket, 3600);

            //调用Token()方法生成上传的Token
            string upToken = put.Token();
            return upToken;
        }
    }
}