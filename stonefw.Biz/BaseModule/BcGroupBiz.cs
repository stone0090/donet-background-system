using System.Collections.Generic;
using Stonefw.Entity.BaseModule;
using Stonefw.Entity.Enum;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
{
    public class BcGroupBiz
    {
        public List<BcGroupEntity> GetBcGroupList()
        {
            return EntityExecution.SelectAll<BcGroupEntity>();
        }

        public ExcuteResultEnum DeleteBcGroup(int groupId)
        {
            if (EntityExecution.Count<BcUserInfoEntity>(n => n.GroupId == groupId && n.DeleteFlag == false) > 0)
                return ExcuteResultEnum.IsOccupied;

            BcGroupEntity entity = new BcGroupEntity() {GroupId = groupId};
            EntityExecution.Delete(entity);
            return ExcuteResultEnum.Success;
        }

        public void AddNewBcGroup(BcGroupEntity entity)
        {
            entity.GroupId = null;
            entity.Insert();
        }

        public void UpdateBcGroup(BcGroupEntity entity)
        {
            entity.Update();
        }

        public BcGroupEntity GetSingleBcGroup(int groupId)
        {
            return EntityExecution.SelectOne<BcGroupEntity>(n => n.GroupId == groupId);
        }
    }
}