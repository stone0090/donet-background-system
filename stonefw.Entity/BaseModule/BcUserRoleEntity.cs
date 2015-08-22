using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.BaseModule
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
