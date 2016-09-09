using System;

using LPush.Core.Data;

namespace LPush.Model
{
    public enum UserType
    {
        /// <summary>
        /// 超级管理员Admin
        /// </summary>
        S,
        /// <summary>
        /// 系统管理员
        /// </summary>
        A,
        /// <summary>
        /// 普通用户
        /// </summary>
        U,
    }

    /// <summary>
    /// 登录帐号表
    /// </summary>
    public partial class Login
    {
        public Login() : base()
        {
            UserType = "U";
        }
    }
}

