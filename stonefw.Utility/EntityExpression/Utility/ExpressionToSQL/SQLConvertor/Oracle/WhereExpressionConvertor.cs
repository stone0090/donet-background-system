using stonefw.Utility.EntityExpression.Entity;

namespace stonefw.Utility.EntityExpression.Utility.ExpressionToSQL.SQLConvertor.Oracle
{
    /// <summary>
    /// 查询条件转换器(Oracle)
    /// </summary>
    internal class WhereExpressionConvertor : IWhereExpressionConvertor
    {
        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="WA"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <returns></returns>
        public string BuildCondition<WA>(GenericWhereEntity<WA> theWhereEntity)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public string BuildCondition<WA, WB>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public string BuildCondition<WA, WB>(GenericWhereEntity<WB> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
        {
            //TODO
            return null;
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
        public string BuildCondition<WA, WB, WC>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity1, GenericJoinEntity<WA, WC> joinEntity2)
        {
            //TODO
            return null;
        }
    }
}
