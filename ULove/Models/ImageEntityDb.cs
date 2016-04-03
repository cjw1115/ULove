using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace ULove.Models
{
    public class ImageEntityDb
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }
        //public /*Uri*/ ImageUri { get; set; } = null;
        public string ImageTitle { get; set; }
        public string ImageDescribe { get; set; }
        public byte[] ImageSource { get; set; } 
    }
}
