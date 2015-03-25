using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using stonefw.Entity.Enum;
using stonefw.Utility.EntityExpressions;
using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;

namespace stonefw.Biz.BaseModule
{
    public class BcUserRoleBiz
    {
        private BcUserRoleDao _dao;
        private BcUserRoleDao Dao
        {
            get { return _dao ?? (_dao = new BcUserRoleDao()); }
        }

        public List<BcUserRoleEntity> GetBcUserRoleList(int? userId = 0, int? roleId = 0)
        {
            return Dao.GetBcUserRoleList(userId, roleId);
        }
        public void DeleteBcUserRole(int userId, int roleId)
        {
            BcUserRoleEntity entity = new BcUserRoleEntity() { UserId = userId, RoleId = roleId };
            EntityExecution.DeleteEntity(entity);
        }
        public ExcuteResult AddNewBcUserRole(BcUserRoleEntity entity)
        {
            if (EntityExecution.GetEntityCount2<BcUserRoleEntity>(n => n.RoleId == entity.RoleId && n.UserId == entity.UserId) > 0)
                return ExcuteResult.IsExist;
            EntityExecution.InsertEntity(entity);
            return ExcuteResult.Success;
        }
        public void UpdateBcUserRole(BcUserRoleEntity entity) { EntityExecution.UpdateEntity(entity); }
        public BcUserRoleEntity GetSingleBcUserRole(int userId, int roleId) { return EntityExecution.ReadEntity2<BcUserRoleEntity>(n => n.UserId == userId && n.RoleId == roleId); }
    }
}
