using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    partial class SysPageFuncPointEntity
    {
        [Field("PageUrl", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string PageUrl { get; set; }
        [Field("FuncPointId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string FuncPointId { get; set; }
    }
}
