using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ULoveWebService.Models
{
    public class UploadImage
    {
        public string Title { get; set; }
        public string Describe { get; set; }
        public string ImageName1 { get; set; }
        public string ImageName2 { get; set; }
        public string AuthorID { get; set; }
    }
}