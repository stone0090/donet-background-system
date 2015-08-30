using System;
using Stonefw.Utility.EntitySql.Attribute;
using Stonefw.Utility.EntitySql.Entity;

namespace Stonefw.Entity.SystemModule
{
    [Serializable]
    [Table("Sys_GlobalSetting")]
    public partial class SysGlobalSettingEntity : BaseEntity
    {
        [Field("SysName")]
        public string SysName { get; set; }

        [Field("SysDescription")]
        public string SysDescription { get; set; }

        [Field("ErrorPage")]
        public string ErrorPage { get; set; }

        [Field("BuildingPage")]
        public string BuildingPage { get; set; }

        [Field("ErrorLogPath")]
        public string ErrorLogPath { get; set; }

        [Field("SuperAdmins")]
        public string SuperAdmins { get; set; }

        [Field("GridViewPageSize")]
        public string GridViewPageSize { get; set; }
    }
}