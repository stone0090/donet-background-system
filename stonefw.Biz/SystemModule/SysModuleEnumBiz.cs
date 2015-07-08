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

        public string GetDescription(string name)
        {
            var result = GetSysModuleEnumList().Where(n => n.Name == name).ToList();
            return result.Count > 0 ? result[0].Description : string.Empty;
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
