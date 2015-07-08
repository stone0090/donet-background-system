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

        public string GetDescription(string name)
        {
            var result = GetSysPermsPointEnumList().Where(n => n.Name == name).ToList();
            return result.Count > 0 ? result[0].Description : string.Empty;
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
