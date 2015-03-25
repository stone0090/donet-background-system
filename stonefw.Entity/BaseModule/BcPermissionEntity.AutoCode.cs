using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcPermissionEntity
    {
        [Field("UserRoleId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public int? UserRoleId { get; set; }
        [Field("PermissionType", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public int? PermissionType { get; set; }
        [Field("ModuleId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string ModuleId { get; set; }
        [Field("FuncPointId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string FuncPointId { get; set; }
        [Field("Permissions", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Permissions { get; set; }
    }
}
