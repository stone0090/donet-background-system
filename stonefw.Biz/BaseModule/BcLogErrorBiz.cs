using System.Collections.Generic;
using System.Linq;
using Stonefw.Entity.BaseModule;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
{
    public class BcLogErrorBiz
    {
        public List<BcLogErrorEntity> GetBcLogErrorList()
        {
            return EntityExecution.SelectAll<BcLogErrorEntity>().OrderByDescending(n => n.OpTime).ToList();
        }

        public void DeleteBcLogError(int id)
        {
            BcLogErrorEntity entity = new BcLogErrorEntity() {Id = id};
            EntityExecution.Delete(entity);
        }

        public void AddNewBcLogError(BcLogErrorEntity entity)
        {
            entity.Id = null;
            entity.Insert();
        }

        public void UpdateBcLogError(BcLogErrorEntity entity)
        {
            entity.Update();
        }

        public BcLogErrorEntity GetSingleBcLogError(int id)
        {
            return EntityExecution.SelectOne<BcLogErrorEntity>(n => n.Id == id);
        }
    }
}