using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_EnumName")]
    public partial class SysEnumNameEntity : BaseEntity
    {
        [Field("Type")]
        public string Type { get; set; }
        [Field("Value")]
        public string Value { get; set; }
        [Field("Name")]
        public string Name { get; set; }
    }
}
