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
    public class BcGroupBiz
    {
        private BcGroupDao _dao;
        private BcGroupDao Dao
        {
            get { return _dao ?? (_dao = new BcGroupDao()); }
        }
        public List<BcGroupEntity> GetBcGroupList()
        { return EntityExecution.ReadEntityList<BcGroupEntity>(); }
        public ExcuteResultEnum DeleteBcGroup(int groupId)
        {
            if (EntityExecution.GetEntityCount<BcUserInfoEntity>(n => n.GroupId == groupId && n.DeleteFlag == false) > 0)
                return ExcuteResultEnum.IsOccupied;

            BcGroupEntity entity = new BcGroupEntity() { GroupId = groupId };
            EntityExecution.ExecDelete(entity);
            return ExcuteResultEnum.Success;
        }
        public void AddNewBcGroup(BcGroupEntity entity)
        {
            entity.GroupId = null;
            EntityExecution.ExecInsert(entity);
        }
        public void UpdateBcGroup(BcGroupEntity entity) { EntityExecution.ExecUpdate(entity); }
        public BcGroupEntity GetSingleBcGroup(int groupId) { return EntityExecution.ReadEntity<BcGroupEntity>(n => n.GroupId == groupId); }
    }
}
