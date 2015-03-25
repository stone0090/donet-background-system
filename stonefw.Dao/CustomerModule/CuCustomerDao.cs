using System.Data;
using System.Data.Common;
using stonefw.Utility;

namespace stonefw.Dao.CustomerModule
{
    public class CuCustomerDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

    }
}
