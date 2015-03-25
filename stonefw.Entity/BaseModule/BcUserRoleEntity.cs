using System;
using System.Data;
using System.Data.Common;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_UserRole")]
    public partial class BcUserRoleEntity
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
