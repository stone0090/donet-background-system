/*
 * First creat by wukea[2013/09/09]
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys;

namespace stonefw.Utility.EntityExpressions.Utilitys.ExpressionVisitors.MethodCalls
{
    /// <summary>
    /// 对字符串函数调用的访问器
    /// </summary>
    internal static class StringMethodCallVisitor
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
        public static void Visit(Type theEntityType, MethodCallExpression m, string tableAlias, Stack<string> colConditionParts, List<string> colParameterNames, List<DbType> colDbTypes, List<object> colArguments)
        {
            string methodName = m.Method.Name;
            if (m.Object is MemberExpression)
            {
                string condition = null;
                //string memberName = GetMemberName(m.Object as MemberExpression, out theDbType);
                string memberName = (m.Object as MemberExpression).GetDBColumnName(theEntityType);
                DbType theDbType = (m.Object as MemberExpression).GetDBColumnType(theEntityType);
                string parameterName = GetParameterName(colParameterNames, memberName, tableAlias);
                switch (methodName)
                {
                    case "Contains":
                        condition = string.Format("({0}.[{1}] like {2})", tableAlias, memberName, parameterName);
                        colConditionParts.Push(condition);
                        colParameterNames.Add(parameterName);
                        colDbTypes.Add(theDbType);
                        colArguments.Add("%" + GetArgumentValue(m.Arguments[0] as ConstantExpression) + "%");
                        break;
                    case "StartsWith":
                        condition = string.Format("({0}.[{1}] like {2})", tableAlias, memberName, parameterName);
                        colConditionParts.Push(condition);
                        colParameterNames.Add(parameterName);
                        colDbTypes.Add(theDbType);
                        colArguments.Add(GetArgumentValue(m.Arguments[0] as ConstantExpression) + "%");
                        break;
                    case "EndsWith":
                        condition = string.Format("({0}.[{1}] like {2})", tableAlias, memberName, parameterName);
                        colConditionParts.Push(condition);
                        colParameterNames.Add(parameterName);
                        colDbTypes.Add(theDbType);
                        colArguments.Add("%" + GetArgumentValue(m.Arguments[0] as ConstantExpression));
                        break;
                    default:
                        throw new Exception("暂不支持{" + m.ToString() + "}的调用！");
                }
            }
            else if (m.Object is ConstantExpression)
            {
                string condition = null;
                string memberName = (m.Arguments[0] as MemberExpression).GetDBColumnName(theEntityType);
                DbType theDbType = (m.Arguments[0] as MemberExpression).GetDBColumnType(theEntityType);
                string parameterName = GetParameterName(colParameterNames, memberName, tableAlias);
                switch (methodName)
                {
                    case "Contains":
                        condition = string.Format("(CHARINDEX({0},{1}.[{2}])>0)", parameterName, tableAlias, memberName);
                        colConditionParts.Push(condition);
                        colParameterNames.Add(parameterName);
                        colDbTypes.Add(theDbType);
                        colArguments.Add(GetArgumentValue(m.Object as ConstantExpression));
                        break;
                    case "StartsWith":
                        condition = string.Format("(CHARINDEX({0},{1}.[{2}])=1)", parameterName, tableAlias, memberName);
                        colConditionParts.Push(condition);
                        colParameterNames.Add(parameterName);
                        colDbTypes.Add(theDbType);
                        colArguments.Add(GetArgumentValue(m.Object as ConstantExpression));
                        break;
                    default:
                        throw new Exception("暂不支持{" + m.ToString() + "}的调用！");
                }
            }
            else
            {
                throw new Exception("暂不支持{" + m.ToString() + "}的调用！");
            }
        }

        /// <summary>
        /// 获取参数名称
        /// </summary>
        /// <param name="colParameterNames"></param>
        /// <param name="memberName"></param>
        /// <param name="tableAlias">表的别名</param>
        /// <returns></returns>
        private static string GetParameterName(List<string> colParameterNames, string memberName, string tableAlias)
        {
            string tmpMemberName = "@" + tableAlias + "_" + memberName;
            if (!colParameterNames.Contains(tmpMemberName))
                return tmpMemberName;
            int index = 1;
            while (colParameterNames.Contains(tmpMemberName + index))
            {
                index++;
            }
            return tmpMemberName + index;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetArgumentValue(ConstantExpression c)
        {
            return c.Value.ToString();
        }
    }
}
