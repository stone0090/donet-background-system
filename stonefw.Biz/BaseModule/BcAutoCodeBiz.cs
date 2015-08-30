using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using Stonefw.Entity.BaseModule;
using Stonefw.Utility;
using Stonefw.Utility.EntitySql;

namespace Stonefw.Biz.BaseModule
{
    public class BcAutoCodeBiz
    {
        private Database _db;

        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<BcAutoCodeEntity> GetBcAutoCodeList()
        {
            return EntityExecution.SelectAll<BcAutoCodeEntity>();
        }

        public void DeleteBcAutoCode(int id)
        {
            BcAutoCodeEntity entity = new BcAutoCodeEntity() {Id = id};
            EntityExecution.Delete(entity);
        }

        public void AddNewBcAutoCode(BcAutoCodeEntity entity)
        {
            entity.Id = null;
            entity.CurrentDate = DateTime.Now;
            entity.CurrentCode = 0;
            using (var ts = new TransactionScope())
            {
                ClearDefault(entity.FuncPointId);
                entity.Insert();
                ts.Complete();
            }
        }

        public void UpdateBcAutoCode(BcAutoCodeEntity entity)
        {
            if (entity.IsDefault == true)
            {
                using (var ts = new TransactionScope())
                {
                    ClearDefault(entity.FuncPointId);
                    entity.Update();
                    ts.Complete();
                }
            }
            else
            {
                entity.Update();
            }
        }

        public BcAutoCodeEntity GetSingleBcAutoCode(int id)
        {
            return EntityExecution.SelectOne<BcAutoCodeEntity>(n => n.Id == id);
        }

        public string GetCode(string funcPointId)
        {
            var list = GetBcAutoCodeList().Where(n => n.FuncPointId == funcPointId && n.IsDefault == true).ToList();
            if (list == null || list.Count <= 0)
            {
                throw new Exception("请先设置好自动编号规则！");
            }
            var entity = list[0];
            if (entity.CurrentDate != DateTime.Now.Date)
            {
                entity.CurrentDate = DateTime.Now.Date;
                entity.CurrentCode = 0;
            }
            entity.CurrentCode += 1;
            var prefix = entity.Prefix;
            var date = DateTime.Parse(entity.CurrentDate.ToString()).ToString(entity.DateFormat);
            var code = entity.CurrentCode.ToString().PadLeft((int) entity.Digit, '0');
            var result = string.Format("{0}{1}{2}", prefix, date, code);
            UpdateBcAutoCode(entity);
            return result;
        }

        private void ClearDefault(string funcPointId)
        {
            string sql = "UPDATE Bc_AutoCode SET IsDefault = 0 WHERE FuncPointId = @FuncPointId ";
            using (DbCommand dm = Db.GetSqlStringCommand(sql))
            {
                Db.AddInParameter(dm, "@FuncPointId", DbType.AnsiString, funcPointId);
                Db.ExecuteNonQuery(dm);
            }
        }
    }
}