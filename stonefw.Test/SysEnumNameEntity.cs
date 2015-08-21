using stonefw.Utility.EntityToSql.Attribute;
using System;
using System.Data;

namespace stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_EnumName")]
    public class SysEnumNameEntity
    {
        [Field("Type")]
        public string Type { get; set; }

        [Field("Value")]
        public string Value { get; set; }

        [Field("Name")]
        public string Name { get; set; }
    }
}
