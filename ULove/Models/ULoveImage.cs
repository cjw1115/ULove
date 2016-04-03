using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULove.Models
{
    /// <summary>
    /// 用于序列化和反序列化服务器的通信数据
    /// </summary>
    public class ULoveImage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Like { get; set; }
        public string Describe { get; set; }
        public string ImageSource1Uri { get; set; }
        public string ImageSource2Uri { get; set; }
    }
}
