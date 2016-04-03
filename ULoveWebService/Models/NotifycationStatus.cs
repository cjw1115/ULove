using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    
    public class NotifycationStatus
    {
        [Key]
        public string Uid { get; set;}
        public int Status { get; set; }//0.no notification 1.message 2.shareimage 3.bindlover 4.systemnotice
    }
}