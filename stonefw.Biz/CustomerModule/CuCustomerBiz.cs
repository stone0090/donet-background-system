using System.Collections.Generic;

using stonefw.Entity.CustomerModule;
using stonefw.Utility.EntitySql;

namespace stonefw.Biz.CustomerModule
{
    public class CuCustomerBiz
    {
        public List<CuCustomerEntity> GetCuCustomerList()
        {
            return EntityExecution.SelectAll<CuCustomerEntity>(n => n.DeleteFlag == false);
        }
        public void DeleteCuCustomer(string cuId)
        {
            CuCustomerEntity entity = new CuCustomerEntity() { CuId = cuId, DeleteFlag = true };
            EntityExecution.Update(entity);
        }
        public void AddNewCuCustomer(CuCustomerEntity entity)
        {
            entity.DeleteFlag = false;
            EntityExecution.Insert(entity);
        }
        public void UpdateCuCustomer(CuCustomerEntity entity)
        {
            EntityExecution.Update(entity);
        }
        public CuCustomerEntity GetCuCustomerEntity(string cuId)
        {
            return EntityExecution.SelectOne<CuCustomerEntity>(n => n.CuId == cuId && n.DeleteFlag == false);
        }
    }
}
