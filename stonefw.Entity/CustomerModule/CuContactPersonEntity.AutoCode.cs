using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.CustomerModule
{
    partial class CuContactPersonEntity
    {
        [Field("CuId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string CuId { get; set; }
        [Field("CpId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? CpId { get; set; }
        [Field("CpName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string CpName { get; set; }
        [Field("Mobile", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Mobile { get; set; }
        [Field("Phone", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Phone { get; set; }
        [Field("QQ", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string QQ { get; set; }
        [Field("WeChat", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string WeChat { get; set; }
        [Field("Weibo", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Weibo { get; set; }
        [Field("Email", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Email { get; set; }
        [Field("Other", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Other { get; set; }
        [Field("Remark", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Remark { get; set; }
        [Field("IsDefault", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? IsDefault { get; set; }
        [Field("DeleteFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? DeleteFlag { get; set; }
    }
}
