using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class NotificationChannel
    {
        [Key]
        public string uid { get; set; }
        public string ChannelUri { get; set; }
    }
}