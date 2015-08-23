using System.Collections.Generic;
using stonefw.Entity.BaseModule;
using stonefw.Entity.Enum;
using stonefw.Utility;
using stonefw.Utility.EntitySql;


namespace stonefw.Biz.BaseModule
{
    public class BcGroupBiz
    {
        public List<BcGroupEntity> GetBcGroupList()
        { return EntityExecution.SelectAll<BcGroupEntity>(); }
        public ExcuteResultEnum DeleteBcGroup(int groupId)
        {
            if (EntityExecution.Count<BcUserInfoEntity>(n => n.GroupId == groupId && n.DeleteFlag == false) > 0)
                return ExcuteResultEnum.IsOccupied;

            BcGroupEntity entity = new BcGroupEntity() { GroupId = groupId };
            EntityExecution.Delete(entity);
            return ExcuteResultEnum.Success;
        }
        public void AddNewBcGroup(BcGroupEntity entity)
        {
            entity.GroupId = null;
            EntityExecution.Insert(entity);
        }
        public void UpdateBcGroup(BcGroupEntity entity) { EntityExecution.Update(entity); }
        public BcGroupEntity GetSingleBcGroup(int groupId) { return EntityExecution.SelectOne<BcGroupEntity>(n => n.GroupId == groupId); }
    }
}
