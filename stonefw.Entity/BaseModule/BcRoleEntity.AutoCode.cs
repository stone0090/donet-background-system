using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcRoleEntity
    {
        [Field("RoleId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? RoleId { get; set; }
        [Field("RoleName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string RoleName { get; set; }
    }
}
