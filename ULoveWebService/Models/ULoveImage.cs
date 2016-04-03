using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class ULoveImage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public int Like { get; set; }
        public string ImageSource1Uri { get; set; }
        public string ImageSource2Uri { get; set; }
    }
}