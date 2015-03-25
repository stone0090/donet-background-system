using System;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_PageFuncPoint")]
    public partial class SysPageFuncPointEntity
    {
        public string FuncPointName { get; set; }
    }
}
