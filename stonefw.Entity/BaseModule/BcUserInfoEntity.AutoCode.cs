using System.Data;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    partial class BcUserInfoEntity
    {
        [Field("UserId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = true, IsPrimaryKey = true)]
        public int? UserId { get; set; }
        [Field("GroupId", FieldDBType = DbType.Int32, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public int? GroupId { get; set; }
        [Field("UserAccount", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string UserAccount { get; set; }
        [Field("UserName", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string UserName { get; set; }
        [Field("Password", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Password { get; set; }
        [Field("Sex", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? Sex { get; set; }
        [Field("OfficePhone", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string OfficePhone { get; set; }
        [Field("MobilePhone", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string MobilePhone { get; set; }
        [Field("Email", FieldDBType = DbType.AnsiString, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public string Email { get; set; }
        [Field("ActivityFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag", FieldDBType = DbType.Boolean, FieldDesc = "", IsIdentityField = false, IsPrimaryKey = false)]
        public bool? DeleteFlag { get; set; }
    }
}
