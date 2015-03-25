using System;
using System.Collections.Generic;
using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility.EntityExpressions;

namespace stonefw.Biz.SystemModule
{
    public class SysEnumNameBiz
    {
        private SysEnumNameDao _dao;
        private SysEnumNameDao Dao
        {
            get { return _dao ?? (_dao = new SysEnumNameDao()); }
        }

        public List<SysEnumNameEntity> GetSysEnumNameList()
        { return EntityExecution.ReadEntityList2<SysEnumNameEntity>(null); }
        public void DeleteSysEnumName(string type, string value)
        {
            SysEnumNameEntity entity = new SysEnumNameEntity() { Type = type, Value = value };
            EntityExecution.DeleteEntity(entity);
        }
        public void AddNewSysEnumName(SysEnumNameEntity entity)
        {
            EntityExecution.InsertEntity(entity);
        }
        public void UpdateSysEnumName(SysEnumNameEntity entity) { EntityExecution.UpdateEntity(entity); }
        public SysEnumNameEntity GetSingleSysEnumName(string type, string value) { return EntityExecution.ReadEntity2<SysEnumNameEntity>(n => n.Type == type && n.Value == value); }
    }

    public static class NameManager
    {
        public static string GetName<T>(this T str)
        {
            var enumType = typeof(T);
            var value = Enum.GetName(enumType, str);
            var entity = new SysEnumNameBiz().GetSingleSysEnumName(enumType.Name, value);
            return entity != null ? entity.Name : value;
        }
    }
}
