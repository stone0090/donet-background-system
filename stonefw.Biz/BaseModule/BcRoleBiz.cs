using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using stonefw.Entity.Enum;


using stonefw.Entity.BaseModule;
using stonefw.Utility.EntitySql;

namespace stonefw.Biz.BaseModule
{
    public class BcRoleBiz
    {
        public List<BcRoleEntity> GetBcRoleList()
        {
            return EntityExecution.SelectAll<BcRoleEntity>();
        }
        public ExcuteResultEnum DeleteBcRole(int roleId)
        {
            if (EntityExecution.Count<BcUserRoleEntity>(n => n.RoleId == roleId) > 0)
                return ExcuteResultEnum.IsOccupied;

            BcRoleEntity entity = new BcRoleEntity() { RoleId = roleId };
            EntityExecution.Delete(entity);
            return ExcuteResultEnum.Success;
        }
        public void AddNewBcRole(BcRoleEntity entity)
        {
            entity.RoleId = null;
            EntityExecution.Insert(entity);
        }
        public void UpdateBcRole(BcRoleEntity entity) { EntityExecution.Update(entity); }
        public BcRoleEntity GetSingleBcRole(int roleId) { return EntityExecution.SelectOne<BcRoleEntity>(n => n.RoleId == roleId); }
    }
}
