using System.Collections.Generic;
using stonefw.Entity.Enum;
using stonefw.Entity.Extension;
using stonefw.Utility;

namespace stonefw.Biz.SystemModule
{
    public class SysPermsPointEnumBiz
    {
        const string CacheKey = "SysPermsPointEnumBiz-GetSysPermsPointEnumList";

        public List<SysPermsPointEnumEntity> GetSysPermsPointEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysPermsPointEnumListCache();
            return (List<SysPermsPointEnumEntity>)list;
        }

        private List<SysPermsPointEnumEntity> SetSysPermsPointEnumListCache()
        {
            var dt = EnumHelper.ToDataTable<SysPermsPointEnum>(SysEnumNameExBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysPermsPointEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }

    }
}
