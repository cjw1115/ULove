using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULoveWebService.Models
{
    public class UserEntity
    {
        [Key]
        public string Uid { get; set; }
        public string UidOfU { get; set; }
        public string Gender { get; set; }
        public string NickName { get; set; }
        public string ProfileImageUri { get; set; }

        public string Token { get; set; }
    }
    
}
