using System;
using LPush.Core.Data;

namespace LPush.Model.Basic
{
    public class Enterprise : BaseEntity
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        public string RegisterAddress { get; set; }

        /// <summary>
        /// 企业法人
        /// </summary>
        public string LegalEntity { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public sbyte LegalIdentityType { get; set; }

        /// <summary>
        /// 企业法人证件号
        /// </summary>
        public string LegalIdentityNo { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 营业执照号
        /// </summary>
        public string BusinessLicenseNo { get; set; }

        /// <summary>
        /// 营业执照扫描件
        /// </summary>
        public string BusinessLicenseImage { get; set; }
    }
}
