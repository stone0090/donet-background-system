using System;
using System.Collections.Generic;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.SystemModule
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