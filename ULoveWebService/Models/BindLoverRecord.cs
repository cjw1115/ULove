using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class BindLoverRecord
    {
        [Key]
        public string uid { get; set; }
        public string uidofu { get; set; }
        public DateTime Date { get; set; }
    }
}