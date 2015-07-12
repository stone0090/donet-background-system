using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stonefw.Biz.SystemModule
{
    public class SysModuleEnumBiz
    {
        const string CacheKey = "SysModuleEnumBiz-GetSysModuleEnumList";

        public List<SysModuleEnumEntity> GetSysModuleEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysModuleEnumListCache();
            return (List<SysModuleEnumEntity>)list;
        }

        private List<SysModuleEnumEntity> SetSysModuleEnumListCache()
        {
            var dt = EnumHelper.ToDataTable<SysModuleEnum>(SysEnumNameExtensionBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysModuleEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }

    }
}
