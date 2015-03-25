using System.Data;
using System.Data.Common;
using stonefw.Utility;

namespace stonefw.Dao.CustomerModule
{
    public class CuContactPersonDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public void ClearDefault(string cuId)
        {
            string sql = "UPDATE Cu_ContactPerson SET IsDefault = 0 WHERE a.DeleteFlag = 0 ";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            Db.AddInParameter(dm, "@CuId", DbType.AnsiString, cuId);
            Db.ExecuteNonQuery(dm);
        }
    }
}
