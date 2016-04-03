using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class NotificationBasicInfo
    {
        [Key]
        public string Sid { get; set; }
        public string Secret { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }
}