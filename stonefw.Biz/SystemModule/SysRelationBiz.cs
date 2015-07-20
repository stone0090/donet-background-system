using stonefw.Biz.BaseModule;
using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace stonefw.Biz.SystemModule
{
    public class SysRelationBiz
    {
        const string CacheKey = "SysRelationBiz-GetSysRelationList";

        private SysRelationDao _dao;
        private SysRelationDao Dao
        {
            get { return _dao ?? (_dao = new SysRelationDao()); }
        }

        public List<SysRelationEntity> GetSysRelationList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysRelationListCache();
            return (List<SysRelationEntity>)list;
        }
        public List<SysRelationEntity> GetSysRelationList(string moduleId)
        {
            return GetSysRelationList().Where(n => n.ModuleId == moduleId).ToList();
        }
        public void DeleteSysRelation(string moduleId, string funcPointId)
        {
            SysRelationEntity entity = new SysRelationEntity() { ModuleId = moduleId, FuncPointId = funcPointId };
            EntityExecution.DeleteEntity(entity);
            SetSysRelationListCache();
        }
        public void AddNewSysRelation(SysRelationEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysRelationListCache();
        }
        public void UpdateSysRelation(SysRelationEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysRelationListCache();
        }
        public SysRelationEntity GetSingleSysRelation(string moduleId, string funcPointId)
        {
            var list = GetSysRelationList().Where(n => n.ModuleId == moduleId && n.FuncPointId == funcPointId).ToList();
            return list.Count > 0 ? list[0] : null;
        }
        public List<SysRelationEntity> GetEnabledSysRelationList()
        {
            var listEnabledSysRelation = GetSysRelationList();

            //加载可用的菜单列表
            var listEnabledSysMenuEntity = new SysMenuBiz().GetEnabledSysMenuList();

            //根据可用的菜单列表，去掉没有起到作用的功能点
            for (int i = listEnabledSysRelation.Count - 1; i >= 0; i--)
            {
                var permisionEntity = listEnabledSysRelation[i];
                var list = listEnabledSysMenuEntity.Where(n => n.ModuleId == permisionEntity.ModuleId && n.FuncPointId == permisionEntity.FuncPointId).ToList();
                if (list.Count <= 0) listEnabledSysRelation.Remove(permisionEntity);
            }

            return listEnabledSysRelation;
        }

        private List<SysRelationEntity> SetSysRelationListCache()
        {
            var listSysRelationEntity = EntityExecution.ReadEntityList2<SysRelationEntity>(null);
            var listSysModuleEnumEntity = new SysModuleEnumBiz().GetSysModuleEnumList();
            var listSysFuncPointEnumEntity = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            var query = from sysRelationEntity in listSysRelationEntity
                        join sysModuleEnumEntity in listSysModuleEnumEntity on sysRelationEntity.ModuleId equals sysModuleEnumEntity.Name
                        join sysFuncPointEnumEntity in listSysFuncPointEnumEntity on sysRelationEntity.FuncPointId equals sysFuncPointEnumEntity.Name
                        select new SysRelationEntity()
                        {
                            ModuleId = sysRelationEntity.ModuleId,
                            FuncPointId = sysRelationEntity.FuncPointId,
                            Permissions = sysRelationEntity.Permissions,
                            ModuleName = sysModuleEnumEntity.Description,
                            FuncPointName = sysFuncPointEnumEntity.Description,
                        };
            listSysRelationEntity = query.ToList<SysRelationEntity>();
            foreach (SysRelationEntity sysRelationEntity in listSysRelationEntity)
            {
                if (!string.IsNullOrEmpty(sysRelationEntity.Permissions))
                {
                    sysRelationEntity.PermissionList = new List<string>();
                    sysRelationEntity.PermissionListName = new List<string>();
                    var list = sysRelationEntity.Permissions.Split(',').ToList();
                    foreach (string s in list)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            sysRelationEntity.PermissionList.Add(s);
                            sysRelationEntity.PermissionListName.Add(SysEnumNameExBiz.GetDescription<SysFuncPointEnum>(s));
                        }
                    }
                    if (sysRelationEntity.PermissionListName.Count > 0)
                        sysRelationEntity.PermissionsName = string.Join(",", sysRelationEntity.PermissionListName.ToArray());
                }
            }
            DataCache.SetCache(CacheKey, listSysRelationEntity);
            return listSysRelationEntity;
        }


        public object GetEnabledSysPermsPointEnumList(string[] permissionList)
        {
            throw new NotImplementedException();
        }
    }
}
