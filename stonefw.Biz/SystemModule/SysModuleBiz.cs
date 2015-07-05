using System.Collections.Generic;
using System.Linq;
using stonefw.Dao.BaseModule;
using stonefw.Dao.SystemModule;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;

namespace stonefw.Biz.SystemModule
{
    public class SysModuleBiz
    {
        const string CacheKey = "SysModuleBiz-GetSysModuleList";

        private SysModuleDao _dao;
        private SysModuleDao Dao
        {
            get { return _dao ?? (_dao = new SysModuleDao()); }
        }

        public List<SysModuleEntity> GetSysModuleList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysModuleListCache();
            return list != null ? (List<SysModuleEntity>)list : null;
        }
        public ExcuteResultEnum DeleteSysModule(string moduleId)
        {
            if (EntityExecution.GetEntityCount2<SysMfpRelationEntity>(n => n.ModuleId == moduleId) > 0)
                return ExcuteResultEnum.IsOccupied;

            if (EntityExecution.GetEntityCount2<SysMenuEntity>(n => n.ModuleId == moduleId && n.DeleteFlag == false) > 0)
                return ExcuteResultEnum.IsOccupied;

            if (EntityExecution.GetEntityCount2<BcPermissionEntity>(n => n.ModuleId == moduleId) > 0)
                return ExcuteResultEnum.IsOccupied;

            SysModuleEntity entity = new SysModuleEntity() { ModuleId = moduleId };
            EntityExecution.DeleteEntity(entity);
            SetSysModuleListCache();
            return ExcuteResultEnum.Success;
        }
        public void AddNewSysModule(SysModuleEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysModuleListCache();
        }
        public void UpdateSysModule(SysModuleEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysModuleListCache();
        }
        public SysModuleEntity GetSingleSysModule(string moduleId)
        {
            var list = GetSysModuleList().Where(n => n.ModuleId == moduleId).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        public string GetName(string moduleId)
        {
            var entity = GetSingleSysModule(moduleId);
            return entity != null ? entity.ModuleName : string.Empty;
        }

        private List<SysModuleEntity> SetSysModuleListCache()
        {
            List<SysModuleEntity> list = EntityExecution.ReadEntityList2<SysModuleEntity>(null);
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}
