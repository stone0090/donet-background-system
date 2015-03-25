using System;
using System.Collections.Generic;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_MfpRelation")]
    public partial class SysMfpRelationEntity
    {
        public string ModuleName { get; set; }
        public string FuncPointName { get; set; }
        public string PermissionsName { get; set; }
        public List<string> PermissionList { get; set; }
        public List<string> PermissionListName { get; set; }
    }
}
