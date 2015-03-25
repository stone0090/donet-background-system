using System;
using System.Data;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Utility;

namespace stonefw.Dao.BaseModule
{
    public class BcLogErrorDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }
    }
}
