using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULove.Services;

namespace ULove.Models
{
    public class SinaWeiboLoginParam
    {
        public WeiboUser Weribouser { get; set; }
        public string param { get; set; }//辅助参数
    }
}
