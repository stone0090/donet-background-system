using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Test
{
    [Serializable]
    [Table("Sys_EnumName")]
    public class SysEnumNameEntity : BaseEntity
    {
        [Field("Type")]
        public string Type { get; set; }

        [Field("Value")]
        public string Value { get; set; }

        [Field("Name")]
        public string Name { get; set; }
    }
}
