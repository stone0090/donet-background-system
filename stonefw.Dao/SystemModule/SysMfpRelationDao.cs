using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Entity.SystemModule;
using stonefw.Utility;

namespace stonefw.Dao.SystemModule
{
    public class SysMfpRelationDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<SysMfpRelationEntity> GetSysMfpRelationList()
        {
            const string sql = @"SELECT a.*,b.ModuleName,c.FuncPointName FROM Sys_MfpRelation a
                            LEFT JOIN Sys_Module b ON a.ModuleId = b.ModuleId
                            LEFT JOIN Sys_FuncPoint c ON a.FuncPointId = c.FuncPointId";
            return DataTableHepler.ConvertToEntityList<SysMfpRelationEntity>(Db.ExecuteDataTable(sql));
        }
    }
}
