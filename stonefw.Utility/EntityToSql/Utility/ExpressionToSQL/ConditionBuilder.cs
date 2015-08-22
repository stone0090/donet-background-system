using System;
using stonefw.Utility.EntityToSql.Entity;
using stonefw.Utility.EntityToSql.Utility.ExpressionToSQL.SQLConvertor;
using stonefw.Utility.EntityToSql.Utility.ExpressionToSQL.SQLConvertor.MSSQL;

namespace stonefw.Utility.EntityToSql.Utility.ExpressionToSQL
{
    /// <summary>
    /// 查询条件构造器
    /// </summary>
    public static class ConditionBuilder
    {        
        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <returns></returns>
        public static string BuildCondition<TA>(GenericWhereEntity<TA> theWhereEntity)
        {
            var convertor = GetWhereExpressionConvertor(theWhereEntity);
            return convertor.BuildCondition(theWhereEntity);
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public static string BuildCondition<TA, TB>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        {
            var convertor = GetWhereExpressionConvertor(theWhereEntity);
            return convertor.BuildCondition(theWhereEntity, joinEntity);
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public static string BuildCondition<TA, TB>(GenericWhereEntity<TB> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        {
            var convertor = GetWhereExpressionConvertor(theWhereEntity);
            return convertor.BuildCondition(theWhereEntity, joinEntity);
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <typeparam name="TB"></typeparam>
        /// <typeparam name="TC"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity1">已连接的实体1</param>
        /// <param name="joinEntity2">已连接的实体2</param>
        /// <returns></returns>
        public static string BuildCondition<TA, TB, TC>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity1, GenericJoinEntity<TA, TC> joinEntity2)
        {
            var convertor = GetWhereExpressionConvertor(theWhereEntity);
            return convertor.BuildCondition(theWhereEntity, joinEntity1, joinEntity2);
        }

        #region 获取查询条件加载器
        /// <summary>
        /// 获取查询条件加载器
        /// </summary>
        /// <typeparam name="W"></typeparam>
        /// <param name="theWhereEntity"></param>
        /// <returns></returns>
        private static IWhereExpressionConvertor GetWhereExpressionConvertor<W>(GenericWhereEntity<W> theWhereEntity)
        {
            string providerName = theWhereEntity.DbProviderName.Trim().ToLower();
            switch (providerName)
            {
                case "system.data.sqlclient":
                    return new WhereExpressionConvertor();
                default:
                    throw new EntityToSqlException("暂不支持数据库：" + providerName);
            }
        }
        #endregion
    }
}
