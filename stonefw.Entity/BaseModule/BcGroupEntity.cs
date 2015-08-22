using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_Group")]
    public partial class BcGroupEntity : BaseEntity
    {
        [Field("GroupId")]
        public int? GroupId { get; set; }
        [Field("GroupName")]
        public string GroupName { get; set; }
    }
}
