using System.Collections.Generic;
using Stonefw.Entity.Enum;
using Stonefw.Entity.Extension;
using Stonefw.Utility;

namespace Stonefw.Biz.SystemModule
{
    public class SysFuncPointEnumBiz
    {
        private const string CacheKey = "SysFuncPointEnumBiz-GetSysFuncPointEnumList";

        public List<SysFuncPointEnumEntity> GetSysFuncPointEnumList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysFuncPointEnumListCache();
            return (List<SysFuncPointEnumEntity>) list;
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