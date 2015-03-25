using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    partial class SysMenuEntity
    {
        [Field("MenuId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? MenuId { get; set; }
        [Field("MenuName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string MenuName { get; set; }
        [Field("MenuLevel", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? MenuLevel { get; set; }
        [Field("Seq", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? Seq { get; set; }
        [Field("FatherNode", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? FatherNode { get; set; }
        [Field("Description", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Description { get; set; }
        [Field("PageUrl", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string PageUrl { get; set; }
        [Field("UrlParameter", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string UrlParameter { get; set; }
        [Field("ActivityFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? DeleteFlag { get; set; }
    }
}
