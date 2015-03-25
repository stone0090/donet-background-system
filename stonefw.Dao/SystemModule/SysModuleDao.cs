using stonefw.Utility;

namespace stonefw.Dao.SystemModule 
{
    public class SysModuleDao 
    {
     private Database _db;
     private Database Db
     {
     get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
     }
    }
}
