using System;
using LPush.Core.Data;

namespace LPush.Model
{
    /// <summary>
    /// 功能表
    /// </summary>
    public partial class Feature
    {
        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级菜单
        /// </summary>
        public int ParentId { get; set; }
		
		/// <summary>
        /// 功能类型,0:菜单,1:按钮,2:操作
        /// </summary>
		public int Type{ get; set; }
        
		/// <summary>
        /// 功能类型,0:菜单,1:按钮,2:操作
        /// </summary>
		public int Order{ get; set; }
		
    }
}
