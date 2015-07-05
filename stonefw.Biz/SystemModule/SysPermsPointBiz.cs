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
    public class SysPermsPointBiz
    {
        const string CacheKey = "SysPermsPointBiz-GetSysPermsPointList";

        private SysPermsPointDao _dao;
        private SysPermsPointDao Dao
        {
            get { return _dao ?? (_dao = new SysPermsPointDao()); }
        }

        public List<SysPermsPointEntity> GetSysPermsPointList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysPermsPointListCache();
            return (List<SysPermsPointEntity>) list;
        }
        public ExcuteResultEnum DeleteSysPermsPoint(string permsPointId)
        {
            if (EntityExecution.GetEntityCount2<SysMfpRelationEntity>(n => n.Permissions.Contains(permsPointId)) > 0)
                return ExcuteResultEnum.IsOccupied;

            if (EntityExecution.GetEntityCount2<BcPermissionEntity>(n => n.Permissions.Contains(permsPointId)) > 0)
                return ExcuteResultEnum.IsOccupied;

            SysPermsPointEntity entity = new SysPermsPointEntity() { PermsPointId = permsPointId };
            EntityExecution.DeleteEntity(entity);
            SetSysPermsPointListCache();
            return ExcuteResultEnum.Success;
        }
        public void AddNewSysPermsPoint(SysPermsPointEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysPermsPointListCache();
        }
        public void UpdateSysPermsPoint(SysPermsPointEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysPermsPointListCache();
        }
        public SysPermsPointEntity GetSingleSysPermsPoint(string permissionId)
        {
            var list = GetSysPermsPointList().Where(n => n.PermsPointId == permissionId).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        public string GetName(string permissionId)
        {
            var entity = GetSingleSysPermsPoint(permissionId);
            return entity != null ? entity.PermsPointName : string.Empty;
        }
        public List<SysPermsPointEntity> GetEnabledSysPermsPointList(string[] permissions)
        {
            var list = new List<SysPermsPointEntity>();
            foreach (string s in permissions)
            {
                list.AddRange(GetSysPermsPointList().Where(n => n.PermsPointId == s));
            }
            return list;
        }

        private List<SysPermsPointEntity> SetSysPermsPointListCache()
        {
            List<SysPermsPointEntity> list = EntityExecution.ReadEntityList2<SysPermsPointEntity>(null);
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}
