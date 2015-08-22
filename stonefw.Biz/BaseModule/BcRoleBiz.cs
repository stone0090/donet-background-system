using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using stonefw.Entity.Enum;

using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Utility.EntitySql.Data;

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
            return EntityExecution.ReadEntityList<BcRoleEntity>();
        }
        public ExcuteResultEnum DeleteBcRole(int roleId)
        {
            if (EntityExecution.GetEntityCount<BcUserRoleEntity>(n => n.RoleId == roleId) > 0)
                return ExcuteResultEnum.IsOccupied;

            BcRoleEntity entity = new BcRoleEntity() { RoleId = roleId };
            EntityExecution.ExecDelete(entity);
            return ExcuteResultEnum.Success;
        }
        public void AddNewBcRole(BcRoleEntity entity)
        {
            entity.RoleId = null;
            EntityExecution.ExecInsert(entity);
        }
        public void UpdateBcRole(BcRoleEntity entity) { EntityExecution.ExecUpdate(entity); }
        public BcRoleEntity GetSingleBcRole(int roleId) { return EntityExecution.ReadEntity<BcRoleEntity>(n => n.RoleId == roleId); }
    }
}
