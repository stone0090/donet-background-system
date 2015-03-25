using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    partial class SysFuncPointEntity
    {
        [Field("FuncPointId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string FuncPointId { get; set; }
        [Field("FuncPointName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string FuncPointName { get; set; }
        [Field("IsAutoCode", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? IsAutoCode { get; set; }
    }
}
