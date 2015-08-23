using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using stonefw.Entity.CustomerModule;
using stonefw.Utility;
using stonefw.Utility.EntitySql;

namespace stonefw.Biz.CustomerModule
{
    public class CuContactPersonBiz
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<CuContactPersonEntity> GetCuContactPersonList()
        {
            return EntityExecution.SelectAll<CuContactPersonEntity>(n => n.DeleteFlag == false);
        }
        public List<CuContactPersonEntity> GetCuContactPersonList(string cuId)
        {
            return EntityExecution.SelectAll<CuContactPersonEntity>(n => n.CuId == cuId && n.DeleteFlag == false);
        }
        public void DeleteCuContactPerson(int cpId)
        {
            CuContactPersonEntity entity = new CuContactPersonEntity() { CpId = cpId, DeleteFlag = true };
            EntityExecution.Update(entity);
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
            EntityExecution.Insert(entity);
        }
        public void UpdateCuContactPerson(CuContactPersonEntity entity)
        {
            if (entity.IsDefault == true)
            {
                using (var ts = new TransactionScope())
                {
                    ClearDefault(entity.CuId);
                    EntityExecution.Update(entity);
                    ts.Complete();
                }
            }
            else
            {
                EntityExecution.Update(entity);
            }
        }
        public CuContactPersonEntity GetCuContactPersonEntity(int cpId)
        {
            return EntityExecution.SelectOne<CuContactPersonEntity>(n => n.CpId == cpId && n.DeleteFlag == false);
        }

        public void ClearDefault(string cuId)
        {
            string sql = "UPDATE Cu_ContactPerson SET IsDefault = 0 WHERE a.DeleteFlag = 0 ";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            Db.AddInParameter(dm, "@CuId", DbType.AnsiString, cuId);
            Db.ExecuteNonQuery(dm);
        }
    }
}
