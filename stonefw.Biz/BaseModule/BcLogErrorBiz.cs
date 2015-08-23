using System.Collections.Generic;
using System.Data;
using System.Linq;
using stonefw.Entity.BaseModule;
using stonefw.Utility;
using stonefw.Utility.EntitySql;

namespace stonefw.Biz.BaseModule
{
    public class BcLogErrorBiz
    {
        public List<BcLogErrorEntity> GetBcLogErrorList()
        { return EntityExecution.SelectAll<BcLogErrorEntity>().OrderByDescending(n => n.OpTime).ToList(); }
        public void DeleteBcLogError(int id)
        {
            BcLogErrorEntity entity = new BcLogErrorEntity() { Id = id };
            EntityExecution.Delete(entity);
        }
        public void AddNewBcLogError(BcLogErrorEntity entity)
        {
            entity.Id = null;
            EntityExecution.Insert(entity);
        }
        public void UpdateBcLogError(BcLogErrorEntity entity) { EntityExecution.Update(entity); }
        public BcLogErrorEntity GetSingleBcLogError(int id) { return EntityExecution.SelectOne<BcLogErrorEntity>(n => n.Id == id); }
    }
}
