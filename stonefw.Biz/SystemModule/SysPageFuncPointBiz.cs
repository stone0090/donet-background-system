using System.Collections.Generic;
using System.Linq;

using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntitySql;
using System.Data.Common;
using System.Data;

namespace stonefw.Biz.SystemModule
{
    public class SysPageFuncPointBiz
    {
        const string CacheKey = "SysPageFuncPointBiz-GetSysPageFuncPointList";

        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<SysPageFuncPointEntity> GetSysPageFuncPointList()
        {
            object list = DataCache.GetCache(CacheKey) ?? SetSysPageFuncPointListCache();
            return (List<SysPageFuncPointEntity>)list;
        }

        public void DeleteSysPageFuncPoint(string pageUrl)
        {
            SysPageFuncPointEntity entity = new SysPageFuncPointEntity() { PageUrl = pageUrl };
            EntityExecution.Delete(entity);
            SetSysPageFuncPointListCache();
        }
        public void AddNewSysPageFuncPoint(SysPageFuncPointEntity entity)
        {
            EntityExecution.Insert(entity);
            SetSysPageFuncPointListCache();
        }
        public void UpdateSysPageFuncPoint(SysPageFuncPointEntity entity)
        {
            EntityExecution.Update(entity);
            SetSysPageFuncPointListCache();
        }
        public SysPageFuncPointEntity GetSingleSysPageFuncPoint(string pageUrl)
        {
            var list = GetSysPageFuncPointList().Where(n => n.PageUrl == pageUrl).ToList();
            return list.Count > 0 ? list[0] : null;
        }
        private List<SysPageFuncPointEntity> SetSysPageFuncPointListCache()
        {
            var listSysPageFuncPointEntity = EntityExecution.SelectAll<SysPageFuncPointEntity>().OrderBy(n => n.PageUrl);
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

        public List<SysPageFuncPointEntity> GetSysPageFuncPointList(string funcPointId)
        {
            var sql = @"select * FROM [Sys_PageFuncPoint] a
                        left join [dbo].[Sys_FuncPoint] b ON a.FuncPointId = b.FuncPointId
                        where b.FuncPointId = @FuncPointId
                        order by a.PageUrl";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            Db.AddInParameter(dm, "@FuncPointId", DbType.AnsiString, funcPointId);
            return DataTableHepler.DataTableToList<SysPageFuncPointEntity>(Db.ExecuteDataTable(dm));
        }
    }
}
