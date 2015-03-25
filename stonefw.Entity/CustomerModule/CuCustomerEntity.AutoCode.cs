using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.CustomerModule
{
    partial class CuCustomerEntity
    {
        [Field("CuId", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = true)]
        public string CuId { get; set; }
        [Field("CuName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string CuName { get; set; }
        [Field("District", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string District { get; set; }
        [Field("Address", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Address { get; set; }
        [Field("Remark", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Remark { get; set; }
        [Field("ActivityFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? DeleteFlag { get; set; }
    }
}
