using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stonefw.Biz.SystemModule
{
    public class SysFuncPointEnumBiz
    {
        const string CacheKey = "SysFuncPointEnumBiz-GetSysFuncPointEnumList";

        public List<SysFuncPointEnumEntity> GetSysFuncPointEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysFuncPointEnumListCache();
            return (List<SysFuncPointEnumEntity>)list;
        }

        private List<SysFuncPointEnumEntity> SetSysFuncPointEnumListCache()
        {
            var dt = EnumHelper.ToDataTable<SysFuncPointEnum>(SysEnumNameExBiz.GetDescription);
            var list = DataTableHepler.DataTableToList<SysFuncPointEnumEntity>(dt);
            DataCache.SetCache(CacheKey, list);
            return list;
        }

    }
}
