using System;

using LPush.Core.Data;
namespace LPush.Model.Sample
{
    public class Example:BaseEntity
    {
        /// <summary>
        /// 名
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 姓
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDt { get; set; }
        /// <summary>
        /// 创建人,登录用户名
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 实体是否被删除
        /// </summary>
        public bool Deleted { get; set; }
    }
}
