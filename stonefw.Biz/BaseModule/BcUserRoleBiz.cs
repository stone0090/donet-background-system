using System.Collections.Generic;
using System.Data;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
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
            BcUserRoleEntity entity = new BcUserRoleEntity() {UserId = userId, RoleId = roleId};
            EntityExecution.Delete(entity);
        }

        public ExcuteResultEnum AddNewBcUserRole(BcUserRoleEntity entity)
        {
            if (EntityExecution.Count<BcUserRoleEntity>(n => n.RoleId == entity.RoleId && n.UserId == entity.UserId) > 0)
                return ExcuteResultEnum.IsExist;
            entity.Insert();
            return ExcuteResultEnum.Success;
        }

        public void UpdateBcUserRole(BcUserRoleEntity entity)
        {
            entity.Update();
        }

        public BcUserRoleEntity GetSingleBcUserRole(int userId, int roleId)
        {
            return EntityExecution.SelectOne<BcUserRoleEntity>(n => n.UserId == userId && n.RoleId == roleId);
        }

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