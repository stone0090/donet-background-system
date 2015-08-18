using System.Text;
using stonefw.Utility.EntityExpressions2.Entity;
using stonefw.Utility.EntityExpressions2.Utility.ExpressionToSQL.SQLConvertor.MSSQL;

namespace stonefw.Utility.EntityExpressions2.Utility.DiagnoseUtilitys
{
    /// <summary>
    /// 预生成SQL语句，用于诊断
    /// </summary>
    public static class PreGenerateSQL
    {
        /// <summary>
        /// 生成查询条件的语句
        /// </summary>
        /// <typeparam name="W"></typeparam>
        /// <param name="whereEntity"></param>
        /// <returns></returns>
        public static string GenerateWhereSQL<W>(GenericWhereEntity<W> whereEntity)
        {
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" WHERE ");
            //逐个语句查询，并合并参数
            for (int i = 0; i < whereEntity.WhereExpressions.Count; i++)
            {
                VisitorForMSSQLWhereExpression<W> conditionBuilder = new VisitorForMSSQLWhereExpression<W>("tDoc", whereEntity);
                conditionBuilder.Build(whereEntity.WhereExpressions[i]);
                if (i > 0)
                {
                    tsqlBuffer.Append(" AND ");
                }
                tsqlBuffer.Append(conditionBuilder.Condition);
            }
            return tsqlBuffer.ToString();
        }
    }
}
