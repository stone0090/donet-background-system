using System.Collections.Generic;
using Stonefw.Entity.Enum;
using Stonefw.Entity.Extension;
using Stonefw.Utility;

namespace Stonefw.Biz.SystemModule
{
    public class SysPermsPointEnumBiz
    {
        private const string CacheKey = "SysPermsPointEnumBiz-GetSysPermsPointEnumList";

        public List<SysPermsPointEnumEntity> GetSysPermsPointEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysPermsPointEnumListCache();
            return (List<SysPermsPointEnumEntity>) list;
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