using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class ULoveUserModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int? LoverID { get; set; }
        public string Password { get; set; }
    }
}