using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;
using stonefw.Utility.EntitySql.Data;

namespace stonefw.Biz.BaseModule
{
    public class BcLogErrorBiz
    {
        private BcLogErrorDao _dao;
        private BcLogErrorDao Dao
        {
            get { return _dao ?? (_dao = new BcLogErrorDao()); }
        }
        public List<BcLogErrorEntity> GetBcLogErrorList()
        { return EntityExecution.ReadEntityList<BcLogErrorEntity>().OrderByDescending(n => n.OpTime).ToList(); }
        public void DeleteBcLogError(int id)
        {
            BcLogErrorEntity entity = new BcLogErrorEntity() { Id = id };
            EntityExecution.ExecDelete(entity);
        }
        public void AddNewBcLogError(BcLogErrorEntity entity)
        {
            entity.Id = null;
            EntityExecution.ExecInsert(entity);
        }
        public void UpdateBcLogError(BcLogErrorEntity entity) { EntityExecution.ExecUpdate(entity); }
        public BcLogErrorEntity GetSingleBcLogError(int id) { return EntityExecution.ReadEntity<BcLogErrorEntity>(n => n.Id == id); }
    }
}
