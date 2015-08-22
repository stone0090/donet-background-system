using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.CustomerModule
{
    [Serializable]
    [Table("Cu_Customer")]
    public partial class CuCustomerEntity : BaseEntity
    {
        [Field("CuId")]
        public string CuId { get; set; }
        [Field("CuName")]
        public string CuName { get; set; }
        [Field("District")]
        public string District { get; set; }
        [Field("Address")]
        public string Address { get; set; }
        [Field("Remark")]
        public string Remark { get; set; }
        [Field("ActivityFlag")]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}
