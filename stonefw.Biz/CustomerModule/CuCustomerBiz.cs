using System.Collections.Generic;
using Stonefw.Entity.CustomerModule;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.CustomerModule
{
    public class CuCustomerBiz
    {
        public List<CuCustomerEntity> GetCuCustomerList()
        {
            return EntityExecution.SelectAll<CuCustomerEntity>(n => n.DeleteFlag == false);
        }

        public void DeleteCuCustomer(string cuId)
        {
            CuCustomerEntity entity = new CuCustomerEntity() {CuId = cuId, DeleteFlag = true};
            entity.Update();
        }

        public void AddNewCuCustomer(CuCustomerEntity entity)
        {
            entity.DeleteFlag = false;
            entity.Insert();
        }

        public void UpdateCuCustomer(CuCustomerEntity entity)
        {
            entity.Update();
        }

        public CuCustomerEntity GetCuCustomerEntity(string cuId)
        {
            return EntityExecution.SelectOne<CuCustomerEntity>(n => n.CuId == cuId && n.DeleteFlag == false);
        }
    }
}