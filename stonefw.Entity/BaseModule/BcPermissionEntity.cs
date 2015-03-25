using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_Permission")]
    public partial class BcPermissionEntity
    {
        public string UserRoleName { get; set; }
        public string ModuleName { get; set; }
        public string FuncPointName { get; set; }
        public string PermissionNames { get; set; }
        public List<string> PermissionList { get; set; }
        public List<string> PermissionNameList { get; set; }
        public bool ActivityFlag { get; set; }

    }
}
