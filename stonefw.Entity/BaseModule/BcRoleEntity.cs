using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_Role")]
    public partial class BcRoleEntity : BaseEntity
    {
        [Field("RoleId")]
        public int? RoleId { get; set; }
        [Field("RoleName")]
        public string RoleName { get; set; }
    }
}
