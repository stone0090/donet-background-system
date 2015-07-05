using System.Collections.Generic;
using System.Linq;
using stonefw.Entity.SystemModule;
using stonefw.Utility;
using stonefw.Utility.EntityExpressions;

namespace stonefw.Dao.SystemModule
{
    public class SysPageFuncPointDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<SysPageFuncPointEntity> GetSysPageFuncPointList()
        {
            var sql = @"select * FROM [Sys_PageFuncPoint] a
                        left join [dbo].[Sys_FuncPoint] b ON a.FuncPointId = b.FuncPointId
                        ORDER BY a.PageUrl";
            return DataTableHepler.DataTableToList<SysPageFuncPointEntity>(Db.ExecuteDataTable(sql));
        }
    }
}
