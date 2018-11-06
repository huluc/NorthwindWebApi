using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindWebApi.Models
{
    public class UserInfoModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Lang { get; set; }
    }
}