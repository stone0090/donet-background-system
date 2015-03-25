using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcUserRoleEntity
    {
        [Field("UserId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public int? UserId { get; set; }
        [Field("RoleId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public int? RoleId { get; set; }
    }
}
