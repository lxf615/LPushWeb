using System;
using LPush.Core.Data;

namespace LPush.Model.Permission
{
    /// <summary>
    /// 登录权限
    /// </summary>
    public class LoginPermission : BaseEntity
    {
		/// <summary>
        /// 登录表关联编号
        /// </summary>
        public int LoginId { get; set; }
		
        /// <summary>
        /// 功能表关联编号
        /// </summary>
        public int FeatureId { get; set; }

        /// <summary>
        /// 是否允许显示
        /// </summary>
        public bool AllowVisible { get; set; }

        /// <summary>
        /// 是否允许增加
        /// </summary>
        public bool AllowAdd { get; set; }

        /// <summary>
        /// 是否允许删除
        /// </summary>
        public bool AllowDelete { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }
    }
}
