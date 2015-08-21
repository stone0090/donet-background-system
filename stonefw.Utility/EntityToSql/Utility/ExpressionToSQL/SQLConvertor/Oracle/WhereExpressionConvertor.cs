using stonefw.Utility.EntityToSql.Entity;

namespace stonefw.Utility.EntityToSql.Utility.ExpressionToSQL.SQLConvertor.Oracle
{
    /// <summary>
    /// 查询条件转换器(Oracle)
    /// </summary>
    internal class WhereExpressionConvertor : IWhereExpressionConvertor
    {
        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <returns></returns>
        public string BuildCondition<TA>(GenericWhereEntity<TA> theWhereEntity)
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
        public string BuildCondition<TA, TB>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
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
        public string BuildCondition<TA, TB>(GenericWhereEntity<TB> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        {
            //TODO
            return null;
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
        public string BuildCondition<TA, TB, TC>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity1, GenericJoinEntity<TA, TC> joinEntity2)
        {
            //TODO
            return null;
        }
    }
}
