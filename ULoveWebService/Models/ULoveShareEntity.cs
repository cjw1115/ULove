using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class ULoveShareEntity
    {
        public int? ID { get; set; }
        public string UidFrom { get; set; }
        public string UidTo { get; set; }
        public int ULoveImageID { get; set; } 
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}