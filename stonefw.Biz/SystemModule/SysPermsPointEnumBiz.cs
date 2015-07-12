using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var dt = EnumHelper.ToDataTable<SysPermsPointEnum>(SysEnumNameExtensionBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysPermsPointEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }


        public object GetEnabledSysPermsPointEnumList(string[] permissionList)
        {
            throw new NotImplementedException();
        }
    }
}
