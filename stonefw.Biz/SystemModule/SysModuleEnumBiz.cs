using System.Collections.Generic;
using Stonefw.Entity.Enum;
using Stonefw.Entity.Extension;
using Stonefw.Utility;

namespace Stonefw.Biz.SystemModule
{
    public class SysModuleEnumBiz
    {
        private const string CacheKey = "SysModuleEnumBiz-GetSysModuleEnumList";

        public List<SysModuleEnumEntity> GetSysModuleEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysModuleEnumListCache();
            return (List<SysModuleEnumEntity>) list;
        }

        private List<SysModuleEnumEntity> SetSysModuleEnumListCache()
        {
            var dt = EnumHelper.ToDataTable<SysModuleEnum>(SysEnumNameExBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysModuleEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}