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
    public class SysFuncPointBiz
    {
        const string CacheKey = "SysFuncPointBiz-GetSysFuncPointList";

        private SysFuncPointDao _dao;
        private SysFuncPointDao Dao
        {
            get { return _dao ?? (_dao = new SysFuncPointDao()); }
        }

        public List<SysFuncPointEntity> GetSysFuncPointList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysFuncPointListCache();
            return (List<SysFuncPointEntity>) list;
        }
        public ExcuteResult DeleteSysFuncPoint(string funcPointId)
        {
            if (EntityExecution.GetEntityCount2<SysMfpRelationEntity>(n => n.FuncPointId == funcPointId) > 0)
                return ExcuteResult.IsOccupied;

            if (EntityExecution.GetEntityCount2<SysMenuEntity>(n => n.FuncPointId == funcPointId && n.DeleteFlag == false) > 0)
                return ExcuteResult.IsOccupied;

            if (EntityExecution.GetEntityCount2<BcPermissionEntity>(n => n.FuncPointId == funcPointId) > 0)
                return ExcuteResult.IsOccupied;

            if (EntityExecution.GetEntityCount2<BcAutoCodeEntity>(n => n.FuncPointId == funcPointId) > 0)
                return ExcuteResult.IsOccupied;

            SysFuncPointEntity entity = new SysFuncPointEntity() { FuncPointId = funcPointId };
            EntityExecution.DeleteEntity(entity);
            SetSysFuncPointListCache();
            return ExcuteResult.Success;
        }
        public void AddNewSysFuncPoint(SysFuncPointEntity entity)
        {
            EntityExecution.InsertEntity(entity);
            SetSysFuncPointListCache();
        }
        public void UpdateSysFuncPoint(SysFuncPointEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
            SetSysFuncPointListCache();
        }
        public SysFuncPointEntity GetSingleSysFuncPoint(string funcPointId)
        {
            var list = GetSysFuncPointList().Where(n => n.FuncPointId == funcPointId).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        public string GetName(string funcPointId)
        {
            var entity = GetSingleSysFuncPoint(funcPointId);
            return entity != null ? entity.FuncPointName : string.Empty;
        }
        public List<SysFuncPointEntity> GetNotUsedSysFuncPointList()
        {
            return Dao.GetNotUsedSysFuncPointList();
        }

        private List<SysFuncPointEntity> SetSysFuncPointListCache()
        {
            List<SysFuncPointEntity> list = EntityExecution.ReadEntityList2<SysFuncPointEntity>(null);
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}
