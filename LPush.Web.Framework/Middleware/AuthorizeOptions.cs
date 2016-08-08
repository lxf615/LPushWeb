using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPush.Web.Framework.Middleware
{
    public class AuthorizeOptions
    {
        public AuthorizeOptions()
        {
            IsAuthorize = true;
        }

        /// <summary>
        /// 配置路径
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// 是否需要Token验证,default :true
        /// </summary>
        public bool IsAuthorize { get; set; }
    }
}
