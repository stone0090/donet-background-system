using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using stonefw.Entity.BaseModule;
using stonefw.Utility;

namespace stonefw.Dao.BaseModule
{
    public class BcUserInfoDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<BcUserInfoEntity> GetBcUserInfoList(int? roleId, int? groupId, string userName)
        {
            string sql = @"SELECT DISTINCT a.*,d.GroupName FROM Bc_UserInfo a
                            LEFT JOIN Bc_UserRole b ON a.UserId = b.UserId
                            LEFT JOIN Bc_Role c ON b.RoleId = c.RoleId 
                            LEFT JOIN Bc_Group d ON a.GroupId = d.GroupId
                            WHERE a.DeleteFlag = 0 ";
            if (roleId != 0) sql += " AND c.RoleId = @RoleId ";
            if (groupId != 0) sql += " AND a.GroupId = @GroupId ";
            if (!string.IsNullOrEmpty(userName)) sql += " AND a.UserName like @UserName ";
            using (DbCommand dm = Db.GetSqlStringCommand(sql))
            {
                if (roleId != 0) Db.AddInParameter(dm, "@RoleId", DbType.Int32, roleId);
                if (groupId != 0) Db.AddInParameter(dm, "@GroupId", DbType.Int32, groupId);
                if (!string.IsNullOrEmpty(userName)) Db.AddInParameter(dm, "@UserName", DbType.AnsiString, "%" + userName + "%");
                return DataTableHepler.ConvertToEntityList<BcUserInfoEntity>(Db.ExecuteDataTable(dm));
            }
        }

    }
}
