using System;

using LPush.Core.Data;

namespace LPush.Model.Basic
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
    public class Login:BaseEntity
    {
        public Login() : base()
        {
            UserType = "U";
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 手机帐号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱帐号
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户类型
        ///'U:普通用户,A:管理员,S:超级管理员
        /// 默认值:'U'
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 用户表关联编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 企业表关联编号
        /// </summary>
        public int EnterpriseId { get; set; }
    }
}

