using System;
using System.Collections.Generic;
using System.Linq;

namespace LPush.Web.Models
{
    [Serializable]
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}