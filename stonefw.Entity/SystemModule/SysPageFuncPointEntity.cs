using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_PageFuncPoint")]
    public partial class SysPageFuncPointEntity : BaseEntity
    {
        [Field("PageUrl")]
        public string PageUrl { get; set; }
        [Field("FuncPointId")]
        public string FuncPointId { get; set; }

        public string FuncPointName { get; set; }
    }
}
