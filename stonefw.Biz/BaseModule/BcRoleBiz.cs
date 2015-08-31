using System.Collections.Generic;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
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

            BcRoleEntity entity = new BcRoleEntity() {RoleId = roleId};
            EntityExecution.Delete(entity);
            return ExcuteResultEnum.Success;
        }

        public void AddNewBcRole(BcRoleEntity entity)
        {
            entity.RoleId = null;
            entity.Insert();
        }

        public void UpdateBcRole(BcRoleEntity entity)
        {
            entity.Update();
        }

        public BcRoleEntity GetSingleBcRole(int roleId)
        {
            return EntityExecution.SelectOne<BcRoleEntity>(n => n.RoleId == roleId);
        }
    }
}