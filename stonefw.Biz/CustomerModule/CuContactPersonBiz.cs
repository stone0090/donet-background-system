using System.Collections.Generic;
using System.Transactions;
using stonefw.Dao.CustomerModule;
using stonefw.Entity.CustomerModule;
using stonefw.Utility.EntitySql.Data;

namespace stonefw.Biz.CustomerModule
{
    public class CuContactPersonBiz
    {
        private CuContactPersonDao _dao;
        private CuContactPersonDao Dao
        {
            get { return _dao ?? (_dao = new CuContactPersonDao()); }
        }

        public List<CuContactPersonEntity> GetCuContactPersonList()
        {
            return EntityExecution.ReadEntityList<CuContactPersonEntity>(n => n.DeleteFlag == false);
        }
        public List<CuContactPersonEntity> GetCuContactPersonList(string cuId)
        {
            return EntityExecution.ReadEntityList<CuContactPersonEntity>(n => n.CuId == cuId && n.DeleteFlag == false);
        }
        public void DeleteCuContactPerson(int cpId)
        {
            CuContactPersonEntity entity = new CuContactPersonEntity() { CpId = cpId, DeleteFlag = true };
            EntityExecution.ExecUpdate(entity);
        }
        public void AddNewCuContactPerson(CuContactPersonEntity entity)
        {
            entity.CpId = null;
            entity.QQ = string.Empty;
            entity.WeChat = string.Empty;
            entity.Weibo = string.Empty;
            entity.Email = string.Empty;
            entity.Other = string.Empty;
            entity.Remark = string.Empty;
            entity.DeleteFlag = false;
            EntityExecution.ExecInsert(entity);
        }
        public void UpdateCuContactPerson(CuContactPersonEntity entity)
        {
            if (entity.IsDefault == true)
            {
                using (var ts = new TransactionScope())
                {
                    Dao.ClearDefault(entity.CuId);
                    EntityExecution.ExecUpdate(entity);
                    ts.Complete();
                }
            }
            else
            {
                EntityExecution.ExecUpdate(entity);
            }
        }
        public CuContactPersonEntity GetCuContactPersonEntity(int cpId)
        {
            return EntityExecution.ReadEntity<CuContactPersonEntity>(n => n.CpId == cpId && n.DeleteFlag == false);
        }
    }
}
