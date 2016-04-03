using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULove.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserEntity
    {
        public string Uid { get; set; }
        public string UidOfU { get; set; }
        public string Gender { get; set; }
        public string NickName { get; set; }
        public string ProfileImageUri { get; set; }
        public string Token { get; set; }
    }
    
}
