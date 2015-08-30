using System;
using System.Collections.Generic;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.BaseModule
{
    [Serializable]
    [Table("Bc_Permission")]
    public partial class BcPermissionEntity : BaseEntity
    {
        [Field("UserRoleId")]
        public int? UserRoleId { get; set; }

        [Field("PermissionType")]
        public int? PermissionType { get; set; }

        [Field("ModuleId")]
        public string ModuleId { get; set; }

        [Field("FuncPointId")]
        public string FuncPointId { get; set; }

        [Field("Permissions")]
        public string Permissions { get; set; }

        public string ModuleName { get; set; }
        public string FuncPointName { get; set; }
        public string UserRoleName { get; set; }
        public List<string> PermissionList { get; set; }
        public List<string> PermissionNameList { get; set; }
        public string PermissionNames { get; set; }
    }
}