using System.Collections.Generic;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;

namespace stonefw.Dao.SystemModule
{
    public class SysFuncPointDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<SysFuncPointEntity> GetNotUsedSysFuncPointList()
        {
            string sql = @"SELECT * FROM Sys_FuncPoint WHERE FuncPointId NOT IN (SELECT FuncPointId FROM Sys_MfpRelation) ";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            return DataTableHepler.DataTableToList<SysFuncPointEntity>(Db.ExecuteDataTable(dm));
        }
    }
}
