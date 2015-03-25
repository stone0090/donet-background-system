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
    public class BcGroupBiz
    {
        private BcGroupDao _dao;
        private BcGroupDao Dao
        {
            get { return _dao ?? (_dao = new BcGroupDao()); }
        }
        public List<BcGroupEntity> GetBcGroupList()
        { return EntityExecution.ReadEntityList2<BcGroupEntity>(null); }
        public ExcuteResult DeleteBcGroup(int groupId)
        {
            if (EntityExecution.GetEntityCount2<BcUserInfoEntity>(n => n.GroupId == groupId && n.DeleteFlag == false) > 0)
                return ExcuteResult.IsOccupied;

            BcGroupEntity entity = new BcGroupEntity() { GroupId = groupId };
            EntityExecution.DeleteEntity(entity);
            return ExcuteResult.Success;
        }
        public void AddNewBcGroup(BcGroupEntity entity)
        {
            entity.GroupId = null;
            EntityExecution.InsertEntity(entity);
        }
        public void UpdateBcGroup(BcGroupEntity entity) { EntityExecution.UpdateEntity(entity); }
        public BcGroupEntity GetSingleBcGroup(int groupId) { return EntityExecution.ReadEntity2<BcGroupEntity>(n => n.GroupId == groupId); }
    }
}
