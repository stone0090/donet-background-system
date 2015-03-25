using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Utility;

namespace stonefw.Dao.BaseModule
{
    public class BcAutoCodeDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public void ClearDefault(string funcPointId)
        {
            string sql = "UPDATE Bc_AutoCode SET IsDefault = 0 WHERE FuncPointId = @FuncPointId ";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            Db.AddInParameter(dm, "@FuncPointId", DbType.AnsiString, funcPointId);
            Db.ExecuteNonQuery(dm);
        }
    }
}
