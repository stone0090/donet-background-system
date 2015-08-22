using System;
using stonefw.Utility.EntitySql.Attribute;
using stonefw.Utility.EntitySql.Entity;

namespace stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_AutoCode")]
    public partial class BcAutoCodeEntity : BaseEntity
    {
        [Field("Id")]
        public int? Id { get; set; }
        [Field("Prefix")]
        public string Prefix { get; set; }
        [Field("DateFormat")]
        public string DateFormat { get; set; }
        [Field("FuncPointId")]
        public string FuncPointId { get; set; }
        [Field("Digit")]
        public int? Digit { get; set; }
        [Field("IsDefault")]
        public bool? IsDefault { get; set; }
        [Field("CurrentDate")]
        public DateTime? CurrentDate { get; set; }
        [Field("CurrentCode")]
        public int? CurrentCode { get; set; }
    }
}
