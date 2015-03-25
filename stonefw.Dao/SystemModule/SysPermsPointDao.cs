using stonefw.Utility;

namespace stonefw.Dao.SystemModule 
{
    public class SysPermsPointDao 
    {
     private Database _db;
     private Database Db
     {
     get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
     }
    }
}
