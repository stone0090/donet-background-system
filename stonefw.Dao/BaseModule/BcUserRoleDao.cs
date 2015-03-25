using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Utility;

namespace stonefw.Dao.BaseModule
{
    public class BcUserRoleDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<BcUserRoleEntity> GetBcUserRoleList(int? userId, int? roleId)
        {
            string sql = @"SELECT a.*,b.UserName,c.RoleName FROM Bc_UserRole a
                            LEFT JOIN Bc_UserInfo b ON a.UserId = b.UserId
                            LEFT JOIN Bc_Role c ON a.RoleId = c.RoleId WHERE b.DeleteFlag = 0 ";
            if (userId != 0) sql += " AND a.UserId = @UserId ";
            if (roleId != 0) sql += " AND a.RoleId = @RoleId ";
            var dm = Db.GetSqlStringCommand(sql);
            if (userId != 0) Db.AddInParameter(dm, "@UserId", DbType.Int32, userId);
            if (roleId != 0) Db.AddInParameter(dm, "@RoleId", DbType.Int32, roleId);
            return DataTableHepler.ConvertToEntityList<BcUserRoleEntity>(Db.ExecuteDataTable(dm));
        }
    }
}
