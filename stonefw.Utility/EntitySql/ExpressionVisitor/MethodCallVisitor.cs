using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Stonefw.Utility.EntitySql.Entity;
using Stonefw.Utility.EntitySql.ExpressionVisitor.MethodCall;

namespace Stonefw.Utility.EntitySql.ExpressionVisitor
{
    /// <summary>
    /// 方法调用的访问器
    /// </summary>
    internal static class MethodCallVisitor
    {
        /// <summary>
        /// 访问指定的方法
        /// </summary>
        /// <param name="theEntityType">实体的类型</param>
        /// <param name="m">访问方法调用相关的表达式</param>
        /// <param name="tableAlias">表的别名</param>
        /// <param name="colConditionParts">存储条件节点的栈</param>
        /// <param name="colParameterNames">存储参数名称的列表</param>
        /// <param name="colDbTypes">存储数据库字段类型的列表</param>
        /// <param name="colArguments">存储条件值的列表</param>
        public static void Visit(Type theEntityType, MethodCallExpression m, string tableAlias,
            Stack<string> colConditionParts, List<string> colParameterNames, List<DbType> colDbTypes,
            List<object> colArguments)
        {
            if (m.Object is MemberExpression)
            {
                //类似n.Name.StartsWith("吴")这样的调用
                if (m.Object.Type == typeof (string))
                {
                    StringMethodCallVisitor.Visit(theEntityType, m, tableAlias, colConditionParts, colParameterNames,
                        colDbTypes, colArguments);
                }
                else
                {
                    throw new EntitySqlException("暂不支持{" + m.ToString() + "}的调用!");
                }
            }
            else if (m.Object is ConstantExpression)
            {
                //类似"ABCD".Contains(n.Name)这样的调用
                var cons = m.Object as ConstantExpression;
                if (cons.Type == typeof (string))
                {
                    StringMethodCallVisitor.Visit(theEntityType, m, tableAlias, colConditionParts, colParameterNames,
                        colDbTypes, colArguments);
                }
                else
                {
                    throw new EntitySqlException("暂不支持{" + m.ToString() + "}的调用!");
                }
            }
            else
            {
                throw new EntitySqlException("暂不支持{" + m.ToString() + "}的调用!");
            }
        }
    }
}