using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;
using System.Collections.Generic;
using System.Linq;

namespace stonefw.Biz.SystemModule
{
    public class SysEnumNameBiz
    {
        const string CacheKey = "SysEnumNameBiz-GetSysEnumNameList";

        private SysEnumNameDao _dao;
        private SysEnumNameDao Dao
        {
            get { return _dao ?? (_dao = new SysEnumNameDao()); }
        }

        public List<SysEnumNameEntity> GetSysEnumNameList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysEnumNameListCache();
            return (List<SysEnumNameEntity>)list;
        }
        public void DeleteSysEnumName(string type, string value)
        {
            SysEnumNameEntity entity = new SysEnumNameEntity() { Type = type, Value = value };
            EntityExecution.DeleteEntity(entity);
            SetSysEnumNameListCache();
        }
        public void AddNewSysEnumName(SysEnumNameEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysEnumNameListCache();
        }
        public void UpdateSysEnumName(SysEnumNameEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysEnumNameListCache();
        }
        public SysEnumNameEntity GetSingleSysEnumName(string type, string value)
        {

            var list = GetSysEnumNameList().Where(n => n.Type == type && n.Value == value).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        private List<SysEnumNameEntity> SetSysEnumNameListCache()
        {
            var list = EntityExecution.ReadEntityList2<SysEnumNameEntity>(null);
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }

    public static class SysEnumNameExtensionBiz
    {
        public static string GetDescription<T>(this T enumValue)
        {
            var enumType = typeof(T);
            var enumName = enumValue.ToString();
            var entity = new SysEnumNameBiz().GetSingleSysEnumName(enumType.Name, enumName);
            if (entity != null && !string.IsNullOrEmpty(entity.Name))
                return entity.Name;
            return enumName;
        }

        public static string GetDescription<T>(this string enumName)
        {
            var enumType = typeof(T);
            var entity = new SysEnumNameBiz().GetSingleSysEnumName(enumType.Name, enumName);
            if (entity != null && !string.IsNullOrEmpty(entity.Name))
                return entity.Name;
            return enumName;
        }
    }
}
