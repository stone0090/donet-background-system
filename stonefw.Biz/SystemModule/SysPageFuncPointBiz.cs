using System.Collections.Generic;
using System.Linq;
using stonefw.Dao.SystemModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntitySql.Data;

namespace stonefw.Biz.SystemModule
{
    public class SysPageFuncPointBiz
    {
        const string CacheKey = "SysPageFuncPointBiz-GetSysPageFuncPointList";

        private SysPageFuncPointDao _dao;
        private SysPageFuncPointDao Dao
        {
            get { return _dao ?? (_dao = new SysPageFuncPointDao()); }
        }

        public List<SysPageFuncPointEntity> GetSysPageFuncPointList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysPageFuncPointListCache();
            return (List<SysPageFuncPointEntity>)list;
        }
        public List<SysPageFuncPointEntity> GetSysPageFuncPointList(string funcPointId)
        {
            return GetSysPageFuncPointList().Where(n => n.FuncPointId == funcPointId).ToList();
        }
        public void DeleteSysPageFuncPoint(string pageUrl)
        {
            SysPageFuncPointEntity entity = new SysPageFuncPointEntity() { PageUrl = pageUrl };
            EntityExecution.ExecDelete(entity);
            SetSysPageFuncPointListCache();
        }
        public void AddNewSysPageFuncPoint(SysPageFuncPointEntity entity)
        {
            EntityExecution.ExecInsert(entity);
            SetSysPageFuncPointListCache();
        }
        public void UpdateSysPageFuncPoint(SysPageFuncPointEntity entity)
        {
            EntityExecution.ExecUpdate(entity);
            SetSysPageFuncPointListCache();
        }
        public SysPageFuncPointEntity GetSingleSysPageFuncPoint(string pageUrl)
        {
            var list = GetSysPageFuncPointList().Where(n => n.PageUrl == pageUrl).ToList();
            return list.Count > 0 ? list[0] : null;
        }

        private List<SysPageFuncPointEntity> SetSysPageFuncPointListCache()
        {
            var listSysPageFuncPointEntity = EntityExecution.ReadEntityList<SysPageFuncPointEntity>().OrderBy(n => n.PageUrl);
            var listSysFuncPointEnumEntity = new SysFuncPointEnumBiz().GetSysFuncPointEnumList();
            var query = from sysPageFuncPointEntity in listSysPageFuncPointEntity
                        join sysFuncPointEnumEntity in listSysFuncPointEnumEntity on sysPageFuncPointEntity.FuncPointId equals sysFuncPointEnumEntity.Name
                        select new SysPageFuncPointEntity()
                        {
                            PageUrl = sysPageFuncPointEntity.PageUrl,
                            FuncPointId = sysPageFuncPointEntity.FuncPointId,
                            FuncPointName = sysFuncPointEnumEntity.Description,
                        };
            var list = query.ToList<SysPageFuncPointEntity>();
            DataCache.SetCache(CacheKey, list);
            return list;
        }
    }
}
