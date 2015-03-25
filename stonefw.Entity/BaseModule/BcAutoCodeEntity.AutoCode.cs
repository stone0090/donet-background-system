using System;
using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcAutoCodeEntity
    {
        [Field("Id", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? Id { get; set; }
        [Field("Prefix", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Prefix { get; set; }
        [Field("DateFormat", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string DateFormat { get; set; }
        [Field("FuncPointId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string FuncPointId { get; set; }
        [Field("Digit", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? Digit { get; set; }
        [Field("IsDefault", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? IsDefault { get; set; }
        [Field("CurrentDate", FieldDBType = DbType.DateTime, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public DateTime? CurrentDate { get; set; }
        [Field("CurrentCode", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? CurrentCode { get; set; }
    }
}
