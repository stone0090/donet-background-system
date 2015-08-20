using System;
using stonefw.Utility.EntityExpression.Entity;
using stonefw.Utility.EntityExpression.Utility.ExpressionToSQL.SQLConvertor;
using stonefw.Utility.EntityExpression.Utility.ExpressionToSQL.SQLConvertor.MSSQL;

namespace stonefw.Utility.EntityExpression.Utility.ExpressionToSQL
{
    /// <summary>
    /// 查询条件构造器
    /// </summary>
    public static class ConditionBuilder
    {        
        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="WA"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <returns></returns>
        public static string BuildCondition<WA>(GenericWhereEntity<WA> theWhereEntity)
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
        public static string BuildCondition<WA, WB>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
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
        public static string BuildCondition<WA, WB>(GenericWhereEntity<WB> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
        {
            var convertor = GetWhereExpressionConvertor(theWhereEntity);
            return convertor.BuildCondition(theWhereEntity, joinEntity);
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="WA"></typeparam>
        /// <typeparam name="WB"></typeparam>
        /// <typeparam name="WC"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity1">已连接的实体1</param>
        /// <param name="joinEntity2">已连接的实体2</param>
        /// <returns></returns>
        public static string BuildCondition<WA, WB, WC>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity1, GenericJoinEntity<WA, WC> joinEntity2)
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
            string providerName = theWhereEntity.DBProviderName.Trim().ToLower();
            switch (providerName)
            {
                case "system.data.sqlclient":
                    return new WhereExpressionConvertor();
                default:
                    throw new Exception("暂不支持数据库：" + providerName);
            }
        }
        #endregion
    }
}
