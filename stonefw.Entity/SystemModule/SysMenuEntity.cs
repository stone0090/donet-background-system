using System;
using System.Collections.Generic;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_Menu")]
    public partial class SysMenuEntity
    {
        public string MenuTreeName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string FuncPointId { get; set; }
        public string FuncPointName { get; set; }
        public List<SysMenuEntity> SubMenuList { get; set; }
    }
}
