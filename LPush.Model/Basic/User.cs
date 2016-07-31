using System;

using LPush.Core.Data;

namespace LPush.Model.Basic
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class User : BaseEntity
    {
        public User() : base()
        {
            EnterpriseId = -1;
            Sex = -1;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别:0女,1男
        /// </summary>
        public sbyte Sex { get; set; }

        /// <summary>
        /// 证件类开
        /// </summary>
        public sbyte IdentityType { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string IdentityNo { get; set; }

        /// <summary>
        /// 证件正面照
        /// </summary>
        public string IdentityImageA { get; set; }

        /// <summary>
        /// 证件反面照
        /// </summary>
        public string IdentityImageB { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 企业编号
        /// 默认值：-1
        /// </summary>
        public int EnterpriseId { get; set; }

        /// <summary>
        /// 登录表id
        /// </summary>
        public string LoginId { get; set; }
    }
}
