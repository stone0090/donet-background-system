using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.BaseModule
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