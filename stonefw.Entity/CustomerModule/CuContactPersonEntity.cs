using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.CustomerModule
{
    [Serializable]
    [Table("Cu_ContactPerson")]
    public partial class CuContactPersonEntity : BaseEntity
    {
        [Field("CuId")]
        public string CuId { get; set; }

        [Field("CpId")]
        public int? CpId { get; set; }

        [Field("CpName")]
        public string CpName { get; set; }

        [Field("Mobile")]
        public string Mobile { get; set; }

        [Field("Phone")]
        public string Phone { get; set; }

        [Field("QQ")]
        public string QQ { get; set; }

        [Field("WeChat")]
        public string WeChat { get; set; }

        [Field("Weibo")]
        public string Weibo { get; set; }

        [Field("Email")]
        public string Email { get; set; }

        [Field("Other")]
        public string Other { get; set; }

        [Field("Remark")]
        public string Remark { get; set; }

        [Field("IsDefault")]
        public bool? IsDefault { get; set; }

        [Field("DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}