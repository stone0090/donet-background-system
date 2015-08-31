using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.SystemModule
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