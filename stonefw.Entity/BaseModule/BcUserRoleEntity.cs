using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_UserRole")]
    public partial class BcUserRoleEntity : BaseEntity
    {
        [Field("UserId")]
        public int? UserId { get; set; }

        [Field("RoleId")]
        public int? RoleId { get; set; }

        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}