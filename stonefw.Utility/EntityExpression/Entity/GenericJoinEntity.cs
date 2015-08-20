using System;
using System.Linq.Expressions;

namespace stonefw.Utility.EntityExpression.Entity
{
    /// <summary>
    /// 支持泛型的表连接的条件
    /// </summary>
    public class GenericJoinEntity<WA,WB>
    {
        public GenericJoinEntity()
        {
        }

        private string _LeftTableGUID = null;
        private string _RightTableGUID = null;
        public string LeftTableGUID
        {
            get
            {
                return _LeftTableGUID;
            }
        }
        public string RightTableGUID
        {
            get
            {
                return _RightTableGUID;
            }
        }
        public GenericWhereEntity<WA> MainEntity
        {
            get;
            set;
        }
        public GenericWhereEntity<WB> EntityToJoin
        {
            get;
            set;
        }

        /// <summary>
        /// 连接模式
        /// </summary>
        public JoinModeEnums JoinMode
        {
            get;
            set;
        }

        private string _JoinConditionFirstParameter = null;
        /// <summary>
        /// 连接条件的首参数，用于表达式的定位
        /// </summary>
        public string JoinConditionFirstParameter
        {
            get
            {
                return _JoinConditionFirstParameter;
            }
        }

        private Expression _JoinConditionExpression = null;
        /// <summary>
        /// 连接条件表达式
        /// </summary>
        public Expression JoinConditionExpression
        {
            get
            {
                return _JoinConditionExpression;
            }
        }

        private string _JoinCondition = null;
        /// <summary>
        /// 连接条件
        /// </summary>
        public string JoinCondition
        {
            get
            {
                return _JoinCondition;
            }
            set
            {
                _JoinCondition = value;
            }
        }

        /// <summary>
        /// 连接表
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="conditionExpression"></param>
        public void InnerJoin(GenericWhereEntity<WA> t0, GenericWhereEntity<WB> t1, Expression<Func<WA, WB, bool>> conditionExpression)
        {
            if (conditionExpression.Body == null)
                throw new Exception("未给定内连接指定连接条件！");
            if (!(conditionExpression.Body is BinaryExpression) || !CheckJoinCondition(conditionExpression.Body))
                throw new Exception("指定的连接条件无效！");

            JoinMode = JoinModeEnums.InnerJoin;
            MainEntity = t0;
            EntityToJoin = t1;
            _LeftTableGUID = t0.GUID;
            _RightTableGUID = t1.GUID;
            _JoinConditionExpression = conditionExpression.Body;
            _JoinConditionFirstParameter = conditionExpression.Parameters[0].Name;
        }

        /// <summary>
        /// 连接表
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="conditionExpression"></param>
        public void LeftJoin(GenericWhereEntity<WA> t0, GenericWhereEntity<WB> t1, Expression<Func<WA, WA, bool>> conditionExpression)
        {
            if (conditionExpression.Body == null)
                throw new Exception("未给定左连接指定连接条件！");
            if (!(conditionExpression.Body is BinaryExpression) || !CheckJoinCondition(conditionExpression.Body))
                throw new Exception("指定的连接条件无效！");

            JoinMode = JoinModeEnums.LeftJoin;
            MainEntity = t0;
            EntityToJoin = t1;
            _LeftTableGUID = t0.GUID;
            _RightTableGUID = t1.GUID;
            _JoinConditionExpression = conditionExpression.Body;
            _JoinConditionFirstParameter = conditionExpression.Parameters[0].Name;
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