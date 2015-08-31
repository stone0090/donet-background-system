using System.Collections.Generic;

namespace Stonefw.Entity.Extension
{
    public partial class PermissionEntity
    {
        public string ModuleId { get; set; }
        public string FuncPointId { get; set; }
        public List<string> PermissionList { get; set; }
    }
}