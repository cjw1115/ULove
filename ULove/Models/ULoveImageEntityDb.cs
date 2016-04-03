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
    /// 用于存储图片到本地数据库用的实体
    /// </summary>
    public class ULoveImageEntityDb
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public byte[] ImageSource1 { get; set; }
        public byte[] ImageSource2 { get; set; }
    }
}
