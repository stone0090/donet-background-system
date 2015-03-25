using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    partial class SysModuleEntity
    {
        [Field("ModuleId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string ModuleId { get; set; }
        [Field("ModuleName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string ModuleName { get; set; }
    }
}
