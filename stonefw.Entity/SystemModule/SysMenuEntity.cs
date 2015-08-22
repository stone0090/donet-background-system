using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;
using System.Collections.Generic;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_Menu")]
    public partial class SysMenuEntity : BaseEntity
    {
        [Field("MenuId")]
        public int? MenuId { get; set; }
        [Field("MenuName")]
        public string MenuName { get; set; }
        [Field("MenuLevel")]
        public int? MenuLevel { get; set; }
        [Field("Seq")]
        public int? Seq { get; set; }
        [Field("FatherNode")]
        public int? FatherNode { get; set; }
        [Field("Description")]
        public string Description { get; set; }
        [Field("PageUrl")]
        public string PageUrl { get; set; }
        [Field("UrlParameter")]
        public string UrlParameter { get; set; }
        [Field("ActivityFlag")]
        public bool? ActivityFlag { get; set; }
        [Field("DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        public string MenuTreeName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string FuncPointId { get; set; }
        public string FuncPointName { get; set; }
        public List<SysMenuEntity> SubMenuList { get; set; }
    }
}
