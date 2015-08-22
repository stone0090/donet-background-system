using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;
using stonefw.Entity.Extension;
using System.Collections.Generic;
using stonefw.Entity.SystemModule;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_UserInfo")]
    public partial class BcUserInfoEntity : BaseEntity
    {
        [Field("UserId")]
        public int? UserId { get; set; }
        [Field("GroupId")]
        public int? GroupId { get; set; }
        [Field("UserAccount")]
        public string UserAccount { get; set; }
        [Field("UserName")]
        public string UserName { get; set; }
        [Field("Password")]
        public string Password { get; set; }
        [Field("Sex")]
        public bool? Sex { get; set; }
        [Field("OfficePhone")]
        public string OfficePhone { get; set; }
        [Field("MobilePhone")]
        public string MobilePhone { get; set; }
        [Field("Email")]
        public string Email { get; set; }
        [Field("ActivityFlag")]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        public bool IsSuperAdmin { get; set; }
        public List<PermissionEntity> PermisionList { get; set; }
        public List<BcRoleEntity> RoleList { get; set; }
        public List<SysMenuEntity> MenuList { get; set; }
        public string GroupName { get; set; }
    }
}
