using System;
using LPush.Core.Data;

namespace LPush.Model.Permission
{
    /// <summary>
    /// 企业数据权限
    /// </summary>
    public class DataPermission : BaseEntity
    {
		/// <summary>
        /// 登录表关联编号
        /// </summary>
        public int LoginId { get; set; }
		
        /// <summary>
        /// 企业表关联编号
        /// </summary>
        public int EnterpriseId { get; set; }

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
