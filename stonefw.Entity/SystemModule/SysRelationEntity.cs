using System;
using System.Collections.Generic;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_Relation")]
    public partial class SysRelationEntity
    {
        public string ModuleName { get; set; }
        public string FuncPointName { get; set; }
        public string PermissionsName { get; set; }
        public List<string> PermissionList { get; set; }
        public List<string> PermissionListName { get; set; }
    }
}
