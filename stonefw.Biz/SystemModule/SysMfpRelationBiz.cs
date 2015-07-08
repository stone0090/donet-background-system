using System.Collections.Generic;
using System.Linq;
using stonefw.Biz.BaseModule;
using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;

namespace stonefw.Biz.SystemModule
{
    public class SysMfpRelationBiz
    {
        const string CacheKey = "SysMfpRelationBiz-GetSysMfpRelationList";

        private SysMfpRelationDao _dao;
        private SysMfpRelationDao Dao
        {
            get { return _dao ?? (_dao = new SysMfpRelationDao()); }
        }

        public List<SysMfpRelationEntity> GetSysMfpRelationList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysMfpRelationListCache();
            return (List<SysMfpRelationEntity>)list;
        }
        public List<SysMfpRelationEntity> GetSysMfpRelationList(string moduleId)
        {
            return GetSysMfpRelationList().Where(n => n.ModuleId == moduleId).ToList();
        }
        public void DeleteSysMfpRelation(string moduleId, string funcPointId)
        {
            SysMfpRelationEntity entity = new SysMfpRelationEntity() { ModuleId = moduleId, FuncPointId = funcPointId };
            EntityExecution.DeleteEntity(entity);
            SetSysMfpRelationListCache();
        }
        public void AddNewSysMfpRelation(SysMfpRelationEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysMfpRelationListCache();
        }
        public void UpdateSysMfpRelation(SysMfpRelationEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysMfpRelationListCache();
        }
        public SysMfpRelationEntity GetSingleSysMfpRelation(string moduleId, string funcPointId)
        {
            var list = GetSysMfpRelationList().Where(n => n.ModuleId == moduleId && n.FuncPointId == funcPointId).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        public List<SysMfpRelationEntity> GetEnabledSysMfpRelationList()
        {
            var listEnabledSysMfpRelation = GetSysMfpRelationList();

            //加载可用的菜单列表
            var listEnabledSysMenuEntity = new SysMenuBiz().GetEnabledSysMenuList();

            //根据可用的菜单列表，去掉没有起到作用的功能点
            for (int i = listEnabledSysMfpRelation.Count - 1; i >= 0; i--)
            {
                var permisionEntity = listEnabledSysMfpRelation[i];
                var list = listEnabledSysMenuEntity.Where(n => n.ModuleId == permisionEntity.ModuleId && n.FuncPointId == permisionEntity.FuncPointId).ToList();
                if (list.Count <= 0) listEnabledSysMfpRelation.Remove(permisionEntity);
            }

            return listEnabledSysMfpRelation;
        }

        private List<SysMfpRelationEntity> SetSysMfpRelationListCache()
        {
            var sysMfpRelationList = Dao.GetSysMfpRelationList();
            foreach (SysMfpRelationEntity sysMfpRelationEntity in sysMfpRelationList)
            {
                if (!string.IsNullOrEmpty(sysMfpRelationEntity.Permissions))
                {
                    sysMfpRelationEntity.PermissionList = new List<string>();
                    sysMfpRelationEntity.PermissionListName = new List<string>();
                    var list = sysMfpRelationEntity.Permissions.Split(',').ToList();
                    foreach (string s in list)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            sysMfpRelationEntity.PermissionList.Add(s);
                            sysMfpRelationEntity.PermissionListName.Add(new SysPermsPointEnumBiz().GetDescription(s));
                        }
                    }
                    if (sysMfpRelationEntity.PermissionListName.Count > 0)
                        sysMfpRelationEntity.PermissionsName = string.Join(",", sysMfpRelationEntity.PermissionListName.ToArray());
                }
            }
            DataCache.SetCache(CacheKey, sysMfpRelationList);
            return sysMfpRelationList;
        }
    }
}
