using System;
using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcLogErrorEntity
    {
        [Field("Id", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? Id { get; set; }
        [Field("UserId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? UserId { get; set; }
        [Field("UserName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string UserName { get; set; }
        [Field("OpUrl", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpUrl { get; set; }
        [Field("OpTime", FieldDBType = DbType.DateTime, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public DateTime? OpTime { get; set; }
        [Field("OpHostAddress", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpHostAddress { get; set; }
        [Field("OpHostName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpHostName { get; set; }
        [Field("OpUserAgent", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpUserAgent { get; set; }
        [Field("OpQueryString", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpQueryString { get; set; }
        [Field("OpHttpMethod", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OpHttpMethod { get; set; }
        [Field("Message", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Message { get; set; }
    }
}
