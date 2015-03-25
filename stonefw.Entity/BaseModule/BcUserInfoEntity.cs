using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Entity.Extension;
using stonefw.Entity.SystemModule;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_UserInfo")]
    public partial class BcUserInfoEntity
    {
        public bool IsSuperAdmin { get; set; }

        public List<PermissionEntity> PermisionList { get; set; }

        public List<BcRoleEntity> RoleList { get; set; }

        public List<SysMenuEntity> MenuList { get; set; }

        public string GroupName { get; set; }

    }
}
