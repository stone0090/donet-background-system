using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.SystemModule
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