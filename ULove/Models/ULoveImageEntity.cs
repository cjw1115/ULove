using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace ULove.Models
{
    /// <summary>
    /// 用于在XAML中绑定数据用的实体
    /// </summary>
    public class ULoveImageEntity
    {  
        public int ID { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public ImageSource ImageSource1 { get; set; }
        public ImageSource ImageSource2 { get; set; }
    }
}
