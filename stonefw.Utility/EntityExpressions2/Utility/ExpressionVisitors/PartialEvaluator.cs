using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace stonefw.Utility.EntityExpressions2.Utility.ExpressionVisitors
{
    /// <summary>
    /// 预处理，执行部分求值
    /// </summary>
    internal class PartialEvaluator : ExpressionVisitor
    {
        /// <summary>
        /// 用于判断是否需要求值的委托
        /// </summary>
        private Func<Expression, bool> m_fnCanBeEvaluated;
        /// <summary>
        /// 需要进行求值的节点
        /// </summary>
        private HashSet<Expression> m_candidates;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PartialEvaluator()
            : this(CanBeEvaluatedLocally)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fnCanBeEvaluated">用于判断是否需要求值的委托</param>
        public PartialEvaluator(Func<Expression, bool> fnCanBeEvaluated)
        {
            this.m_fnCanBeEvaluated = fnCanBeEvaluated;
        }

        /// <summary>
        /// 执行预处理，针对需要的部分进行求值
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Expression Eval(Expression exp)
        {
            //调用分析器获取需要进行求值的节点
            this.m_candidates = new Nominator(this.m_fnCanBeEvaluated).Nominate(exp);
            //逐一访问相关节点
            return this.Visit(exp);
        }

        /// <summary>
        /// 访问相关节点
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            //如果是需要执行求值的节点，则执行求值方法
            if (this.m_candidates.Contains(exp))
            {
                return this.Evaluate(exp);
            }

            return base.Visit(exp);
        }

        /// <summary>
        /// 执行求值方法
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Expression Evaluate(Expression e)
        {
            //访问常量的话直接返回
            if (e.NodeType == ExpressionType.Constant)
            {
                return e;
            }

            //执行Lambda表达式
            LambdaExpression lambda = Expression.Lambda(e);
            Delegate fn = lambda.Compile();

            return Expression.Constant(fn.DynamicInvoke(null), e.Type);
        }

        /// <summary>
        /// 用于判断是否需要求值的函数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static bool CanBeEvaluatedLocally(Expression exp)
        {
            return exp.NodeType != ExpressionType.Parameter;
        }

        #region Nominator

        /// <summary>
        /// 分析器：评估有那些节点需要执行求值操作
        /// </summary>
        private class Nominator : ExpressionVisitor
        {
            /// <summary>
            /// 评估器的委托
            /// </summary>
            private Func<Expression, bool> m_fnCanBeEvaluated;
            /// <summary>
            /// 需要执行求值操作的节点
            /// </summary>
            private HashSet<Expression> m_candidates;
            /// <summary>
            /// 状态：是否不能执行求值操作
            /// </summary>
            private bool m_cannotBeEvaluated;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="fnCanBeEvaluated">评估器的委托</param>
            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.m_fnCanBeEvaluated = fnCanBeEvaluated;
            }

            /// <summary>
            /// 挑选需要执行求值操作的节点
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.m_candidates = new HashSet<Expression>();
                this.Visit(expression);
                return this.m_candidates;
            }

            /// <summary>
            /// 递归访问整个表达式，判断节点是否需要执行求值操作
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            protected override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = this.m_cannotBeEvaluated;
                    this.m_cannotBeEvaluated = false;

                    base.Visit(expression);

                    if (!this.m_cannotBeEvaluated)
                    {
                        if (this.m_fnCanBeEvaluated(expression))
                        {
                            this.m_candidates.Add(expression);
                        }
                        else
                        {
                            this.m_cannotBeEvaluated = true;
                        }
                    }

                    this.m_cannotBeEvaluated |= saveCannotBeEvaluated;
                }

                return expression;
            }
        }

        #endregion
    }
}
