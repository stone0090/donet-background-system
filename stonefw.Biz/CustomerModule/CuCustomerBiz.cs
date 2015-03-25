using System.Collections.Generic;
using stonefw.Dao.CustomerModule;
using stonefw.Entity.CustomerModule;
using stonefw.Utility.EntityExpressions;

namespace stonefw.Biz.CustomerModule
{
    public class CuCustomerBiz
    {
        private CuCustomerDao _dao;
        private CuCustomerDao Dao
        {
            get { return _dao ?? (_dao = new CuCustomerDao()); }
        }

        public List<CuCustomerEntity> GetCuCustomerList()
        {
            return EntityExecution.ReadEntityList2<CuCustomerEntity>(n => n.DeleteFlag == false);
        }
        public void DeleteCuCustomer(string cuId)
        {
            CuCustomerEntity entity = new CuCustomerEntity() { CuId = cuId, DeleteFlag = true };
            EntityExecution.UpdateEntity(entity);
        }
        public void AddNewCuCustomer(CuCustomerEntity entity)
        {
            entity.DeleteFlag = false;
            EntityExecution.InsertEntity(entity);
        }
        public void UpdateCuCustomer(CuCustomerEntity entity)
        {
            EntityExecution.UpdateEntity(entity);
        }
        public CuCustomerEntity GetCuCustomerEntity(string cuId)
        {
            return EntityExecution.ReadEntity2<CuCustomerEntity>(n => n.CuId == cuId && n.DeleteFlag == false);
        }
    }
}
