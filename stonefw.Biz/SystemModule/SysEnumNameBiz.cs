using System.Collections.Generic;
using System.Linq;
using Stonefw.Entity.SystemModule;
using Stonefw.Utility;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.SystemModule
{
    public class SysEnumNameBiz
    {
        private const string CacheKey = "SysEnumNameBiz-GetSysEnumNameList";

        public List<SysEnumNameEntity> GetSysEnumNameList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysEnumNameListCache();
            return (List<SysEnumNameEntity>) list;
        }

        public void DeleteSysEnumName(string type, string value)
        {
            SysEnumNameEntity entity = new SysEnumNameEntity() {Type = type, Value = value};
            EntityExecution.Delete(entity);
            SetSysEnumNameListCache();
        }

        public void AddNewSysEnumName(SysEnumNameEntity entity)
        {
            entity.Insert();
            SetSysEnumNameListCache();
        }

        public void UpdateSysEnumName(SysEnumNameEntity entity)
        {
            entity.Update();
            SetSysEnumNameListCache();
        }

        public SysEnumNameEntity GetSingleSysEnumName(string type, string value)
        {
            var list = GetSysEnumNameList().Where(n => n.Type == type && n.Value == value).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        private List<SysEnumNameEntity> SetSysEnumNameListCache()
        {
            var list = EntityExecution.SelectAll<SysEnumNameEntity>();
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}