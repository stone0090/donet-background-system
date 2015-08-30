using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_Group")]
    public partial class BcGroupEntity : BaseEntity
    {
        [Field("GroupId")]
        public int? GroupId { get; set; }

        [Field("GroupName")]
        public string GroupName { get; set; }
    }
}