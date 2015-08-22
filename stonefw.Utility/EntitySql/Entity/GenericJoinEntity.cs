using System;
using System.Linq.Expressions;

namespace stonefw.Utility.EntitySql.Entity
{
    /// <summary>
    /// 支持泛型的表连接的条件
    /// </summary>
    public class GenericJoinEntity<TA, TB>
    {
        public GenericJoinEntity()
        {
        }

        private string _leftTableGuid = null;
        private string _rightTableGuid = null;

        public string LeftTableGuid { get { return _leftTableGuid; } }
        public string RightTableGuid { get { return _rightTableGuid; } }

        public GenericWhereEntity<TA> MainEntity { get; set; }
        public GenericWhereEntity<TB> EntityToJoin { get; set; }

        /// <summary>
        /// 连接模式
        /// </summary>
        public JoinModeEnum JoinMode { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        public string JoinCondition { get; set; }

        private string _joinConditionFirstParameter = null;
        /// <summary>
        /// 连接条件的首参数，用于表达式的定位
        /// </summary>
        public string JoinConditionFirstParameter { get { return _joinConditionFirstParameter; } }

        private Expression _joinConditionExpression = null;
        /// <summary>
        /// 连接条件表达式
        /// </summary>
        public Expression JoinConditionExpression { get { return _joinConditionExpression; } }

        public void InnerJoin(GenericWhereEntity<TA> TA, GenericWhereEntity<TB> TB, Expression<Func<TA, TB, bool>> conditionExpression)
        {
            Join(TA, TB, conditionExpression, JoinModeEnum.InnerJoin);
        }

        public void LeftJoin(GenericWhereEntity<TA> TA, GenericWhereEntity<TB> TB, Expression<Func<TA, TB, bool>> conditionExpression)
        {
            Join(TA, TB, conditionExpression, JoinModeEnum.LeftJoin);
        }

        private void Join(GenericWhereEntity<TA> TA, GenericWhereEntity<TB> TB, Expression<Func<TA, TB, bool>> conditionExpression, JoinModeEnum joinMode)
        {
            if (conditionExpression.Body == null)
                throw new EntitySqlException("未指定连接条件！");
            if (!(conditionExpression.Body is BinaryExpression) || !CheckJoinCondition(conditionExpression.Body))
                throw new EntitySqlException("指定的连接条件无效！");

            JoinMode = joinMode;
            MainEntity = TA;
            EntityToJoin = TB;
            _leftTableGuid = TA.Guid;
            _rightTableGuid = TB.Guid;
            _joinConditionExpression = conditionExpression.Body;
            _joinConditionFirstParameter = conditionExpression.Parameters[0].Name;
        }

        /// <summary>
        /// 判断连接条件是否符合要求
        /// </summary>
        /// <param name="joinExpression">连接条件的表达式</param>
        /// <returns></returns>
        private static bool CheckJoinCondition(Expression joinExpression)
        {
            if (joinExpression is BinaryExpression)
            {
                BinaryExpression b = (BinaryExpression)joinExpression;
                return CheckJoinCondition(b.Left) && CheckJoinCondition(b.Right);
            }
            else if (joinExpression is MemberExpression)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}