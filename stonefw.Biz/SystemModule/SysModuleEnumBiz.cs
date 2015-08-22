using stonefw.Entity.Enum;
using stonefw.Entity.Extension;
using stonefw.Utility;
using System.Collections.Generic;

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
            var dt = EnumHelper.ToDataTable<SysModuleEnum>(SysEnumNameExBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysModuleEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }

    }
}
