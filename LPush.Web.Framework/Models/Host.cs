using System;

namespace LPush.Web.Framework.Models
{
    public class Host
    {
        /// <summary>
        /// Token认证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// IM服务器地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// IM服务器端口
        /// </summary>
        public int Port { get; set; }
    }
}
