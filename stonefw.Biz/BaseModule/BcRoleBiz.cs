using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using stonefw.Entity.Enum;
using stonefw.Utility.EntityExpressions;
using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;


namespace stonefw.Biz.BaseModule
{
    public class BcRoleBiz
    {
        private BcRoleDao _dao;
        private BcRoleDao Dao
        {
            get { return _dao ?? (_dao = new BcRoleDao()); }
        }

        public List<BcRoleEntity> GetBcRoleList()
        {
            return EntityExecution.ReadEntityList2<BcRoleEntity>(null);
        }
        public ExcuteResult DeleteBcRole(int roleId)
        {
            if (EntityExecution.GetEntityCount2<BcUserRoleEntity>(n => n.RoleId == roleId) > 0)
                return ExcuteResult.IsOccupied;

            BcRoleEntity entity = new BcRoleEntity() { RoleId = roleId };
            EntityExecution.DeleteEntity(entity);
            return ExcuteResult.Success;
        }
        public void AddNewBcRole(BcRoleEntity entity)
        {
            entity.RoleId = null;
            EntityExecution.InsertEntity(entity);
        }
        public void UpdateBcRole(BcRoleEntity entity) { EntityExecution.UpdateEntity(entity); }
        public BcRoleEntity GetSingleBcRole(int roleId) { return EntityExecution.ReadEntity2<BcRoleEntity>(n => n.RoleId == roleId); }
    }
}
