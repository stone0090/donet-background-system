using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using stonefw.Entity.Enum;


using stonefw.Entity.BaseModule;
using stonefw.Utility.EntitySql;
using stonefw.Utility;

namespace stonefw.Biz.BaseModule
{
    public class BcUserRoleBiz
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public void DeleteBcUserRole(int userId, int roleId)
        {
            BcUserRoleEntity entity = new BcUserRoleEntity() { UserId = userId, RoleId = roleId };
            EntityExecution.Delete(entity);
        }
        public ExcuteResultEnum AddNewBcUserRole(BcUserRoleEntity entity)
        {
            if (EntityExecution.Count<BcUserRoleEntity>(n => n.RoleId == entity.RoleId && n.UserId == entity.UserId) > 0)
                return ExcuteResultEnum.IsExist;
            EntityExecution.Insert(entity);
            return ExcuteResultEnum.Success;
        }
        public void UpdateBcUserRole(BcUserRoleEntity entity) { EntityExecution.Update(entity); }
        public BcUserRoleEntity GetSingleBcUserRole(int userId, int roleId) { return EntityExecution.SelectOne<BcUserRoleEntity>(n => n.UserId == userId && n.RoleId == roleId); }

        public List<BcUserRoleEntity> GetBcUserRoleList(int? userId = 0, int? roleId = 0)
        {
            string sql = @"SELECT a.*,b.UserName,c.RoleName FROM Bc_UserRole a
                            LEFT JOIN Bc_UserInfo b ON a.UserId = b.UserId
                            LEFT JOIN Bc_Role c ON a.RoleId = c.RoleId WHERE b.DeleteFlag = 0 ";
            if (userId != 0) sql += " AND a.UserId = @UserId ";
            if (roleId != 0) sql += " AND a.RoleId = @RoleId ";
            var dm = Db.GetSqlStringCommand(sql);
            if (userId != 0) Db.AddInParameter(dm, "@UserId", DbType.Int32, userId);
            if (roleId != 0) Db.AddInParameter(dm, "@RoleId", DbType.Int32, roleId);
            return DataTableHepler.DataTableToList<BcUserRoleEntity>(Db.ExecuteDataTable(dm));
        }
    }
}
