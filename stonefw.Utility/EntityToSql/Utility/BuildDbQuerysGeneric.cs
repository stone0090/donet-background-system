using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stonefw.Utility.EntityToSql.Utility.ExpressionVisitors;
using stonefw.Utility.EntityToSql.Entity;
using stonefw.Utility.EntityToSql.GenSQL;

namespace stonefw.Utility.EntityToSql.Utilitys
{
    /// <summary>
    /// 创建数据库查询
    /// </summary>
    public static class BuildDbQuerysGeneric
    {
        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <returns></returns>
        public static string BuildCondition<TA>(GenericWhereEntity<TA> theWhereEntity)
        {
            string dbTableName = DbTableMapping.GetDbTableName(theWhereEntity.EntityType);
            if (string.IsNullOrEmpty(dbTableName))
            {
                throw new Exception(string.Format("未给类型{0}设置数据表信息!", typeof(TA).FullName));
            }
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            if (theWhereEntity.DisableTableAlias)
            {
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("]");
            }
            else
            {
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableName);
            }

            if (theWhereEntity.WhereExpressions.Count > 0)
            {
                tsqlBuffer.Append(" WHERE ");
                //逐个语句查询，并合并参数
                for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
                {
                    ConditionBuilderGeneric<TA> conditionBuilder = new ConditionBuilderGeneric<TA>((theWhereEntity.DisableTableAlias ? dbTableName : theWhereEntity.TableName), theWhereEntity);
                    conditionBuilder.Build(theWhereEntity.WhereExpressions[i]);
                    if (i > 0)
                    {
                        tsqlBuffer.Append(" AND ");
                    }
                    tsqlBuffer.Append(conditionBuilder.Condition);

                    if (conditionBuilder.Arguments != null && conditionBuilder.Arguments.Length > 0)
                        theWhereEntity.WhereParameterValues.AddRange(conditionBuilder.Arguments);
                    if (conditionBuilder.ParameterNames != null && conditionBuilder.ParameterNames.Length > 0)
                        theWhereEntity.WhereParameterNames.AddRange(conditionBuilder.ParameterNames);
                    if (conditionBuilder.DbTypes != null && conditionBuilder.DbTypes.Length > 0)
                        theWhereEntity.WhereParameterTypes.AddRange(conditionBuilder.DbTypes);
                }
            }

            return tsqlBuffer.ToString();
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public static string BuildCondition<TA, TB>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        {
            //重置表的别名
            joinEntity.MainEntity.ResetTableName(0);
            joinEntity.EntityToJoin.ResetTableName(1);
            //预生成查询条件
            BuildConditionForJoin(joinEntity.MainEntity);
            BuildConditionForJoin(joinEntity.EntityToJoin);

            //生成最终条件
            string dbTableName = DbTableMapping.GetDbTableName(joinEntity.MainEntity.EntityType);
            string joinDBTableName = DbTableMapping.GetDbTableName(joinEntity.EntityToJoin.EntityType);
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableName);
            if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableName);
            tsqlBuffer.Append(" ON ");
            string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity);
            tsqlBuffer.Append(joinCon);

            bool addWherePart = false;
            if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
            {
                tsqlBuffer.Append(" WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(theWhereEntity.WhereCondition);
            }
            if (!string.IsNullOrEmpty(joinEntity.EntityToJoin.WhereCondition))
            {
                tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(joinEntity.EntityToJoin.WhereCondition);
            }

            if (joinEntity.EntityToJoin.WhereParameterNames.Count > 0)
                theWhereEntity.WhereParameterNames.AddRange(joinEntity.EntityToJoin.WhereParameterNames);
            if (joinEntity.EntityToJoin.WhereParameterTypes.Count > 0)
                theWhereEntity.WhereParameterTypes.AddRange(joinEntity.EntityToJoin.WhereParameterTypes);
            if (joinEntity.EntityToJoin.WhereParameterValues.Count > 0)
                theWhereEntity.WhereParameterValues.AddRange(joinEntity.EntityToJoin.WhereParameterValues);

            return tsqlBuffer.ToString();
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity">已连接的实体</param>
        /// <returns></returns>
        public static string BuildCondition<TA, TB>(GenericWhereEntity<TB> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        {
            //重置表的别名
            joinEntity.MainEntity.ResetTableName(0);
            joinEntity.EntityToJoin.ResetTableName(1);
            //预生成查询条件
            BuildConditionForJoin(joinEntity.MainEntity);
            BuildConditionForJoin(joinEntity.EntityToJoin);

            //生成最终条件
            string dbTableName = DbTableMapping.GetDbTableName(joinEntity.MainEntity.EntityType);
            string joinDBTableName = DbTableMapping.GetDbTableName(joinEntity.EntityToJoin.EntityType);
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableName);
            if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableName);
            tsqlBuffer.Append(" ON ");
            string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity);
            tsqlBuffer.Append(joinCon);

            bool addWherePart = false;
            if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
            {
                tsqlBuffer.Append(" WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(theWhereEntity.WhereCondition);
            }
            if (!string.IsNullOrEmpty(joinEntity.EntityToJoin.WhereCondition))
            {
                tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(joinEntity.EntityToJoin.WhereCondition);
            }

            if (joinEntity.EntityToJoin.WhereParameterNames.Count > 0)
                theWhereEntity.WhereParameterNames.AddRange(joinEntity.EntityToJoin.WhereParameterNames);
            if (joinEntity.EntityToJoin.WhereParameterTypes.Count > 0)
                theWhereEntity.WhereParameterTypes.AddRange(joinEntity.EntityToJoin.WhereParameterTypes);
            if (joinEntity.EntityToJoin.WhereParameterValues.Count > 0)
                theWhereEntity.WhereParameterValues.AddRange(joinEntity.EntityToJoin.WhereParameterValues);

            return tsqlBuffer.ToString();
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
            //重置表的别名
            joinEntity1.MainEntity.ResetTableName(0);
            joinEntity1.EntityToJoin.ResetTableName(1);
            joinEntity2.EntityToJoin.ResetTableName(2);
            //预生成查询条件
            BuildConditionForJoin(theWhereEntity);
            BuildConditionForJoin(joinEntity1.EntityToJoin);
            BuildConditionForJoin(joinEntity2.EntityToJoin);

            //生成最终条件
            string dbTableName = DbTableMapping.GetDbTableName(theWhereEntity.EntityType);
            string joinDBTableName1 = DbTableMapping.GetDbTableName(joinEntity1.EntityToJoin.EntityType);
            string joinDBTableName2 = DbTableMapping.GetDbTableName(joinEntity2.EntityToJoin.EntityType);
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableName);

            if (joinEntity1.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity1.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName1).Append("] AS ").Append(joinEntity1.EntityToJoin.TableName);
            tsqlBuffer.Append(" ON ");
            string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity1);
            tsqlBuffer.Append(joinCon);

            if (joinEntity2.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity2.JoinMode == JoinModeEnum.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName2).Append("] AS ").Append(joinEntity2.EntityToJoin.TableName);
            tsqlBuffer.Append(" ON ");
            joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity2);
            tsqlBuffer.Append(joinCon);

            bool addWherePart = false;
            if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
            {
                tsqlBuffer.Append(" WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(theWhereEntity.WhereCondition);
            }
            if (!string.IsNullOrEmpty(joinEntity1.EntityToJoin.WhereCondition))
            {
                tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(joinEntity1.EntityToJoin.WhereCondition);
            }
            if (!string.IsNullOrEmpty(joinEntity2.EntityToJoin.WhereCondition))
            {
                tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
                addWherePart = true;
                tsqlBuffer.Append(joinEntity2.EntityToJoin.WhereCondition);
            }

            if (joinEntity1.EntityToJoin.WhereParameterNames.Count > 0)
                theWhereEntity.WhereParameterNames.AddRange(joinEntity1.EntityToJoin.WhereParameterNames);
            if (joinEntity1.EntityToJoin.WhereParameterTypes.Count > 0)
                theWhereEntity.WhereParameterTypes.AddRange(joinEntity1.EntityToJoin.WhereParameterTypes);
            if (joinEntity1.EntityToJoin.WhereParameterValues.Count > 0)
                theWhereEntity.WhereParameterValues.AddRange(joinEntity1.EntityToJoin.WhereParameterValues);

            if (joinEntity2.EntityToJoin.WhereParameterNames.Count > 0)
                theWhereEntity.WhereParameterNames.AddRange(joinEntity2.EntityToJoin.WhereParameterNames);
            if (joinEntity2.EntityToJoin.WhereParameterTypes.Count > 0)
                theWhereEntity.WhereParameterTypes.AddRange(joinEntity2.EntityToJoin.WhereParameterTypes);
            if (joinEntity2.EntityToJoin.WhereParameterValues.Count > 0)
                theWhereEntity.WhereParameterValues.AddRange(joinEntity2.EntityToJoin.WhereParameterValues);

            return tsqlBuffer.ToString();
        }

        #region 私有方法

        /// <summary>
        /// 为连接操作创建查询条件
        /// </summary>
        /// <param name="theWhereEntity">查询条件的节点</param>
        private static void BuildConditionForJoin<TA>(GenericWhereEntity<TA> theWhereEntity)
        {
            StringBuilder tsqlBuffer = new StringBuilder(2048);

            if (theWhereEntity.WhereExpressions.Count > 0)
            {
                //逐个语句查询，并合并参数
                for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
                {
                    ConditionBuilderGeneric<TA> conditionBuilder = new ConditionBuilderGeneric<TA>(theWhereEntity.TableName, theWhereEntity);
                    conditionBuilder.Build(theWhereEntity.WhereExpressions[i]);
                    if (i > 0)
                    {
                        tsqlBuffer.Append(" AND ");
                    }
                    tsqlBuffer.Append(conditionBuilder.Condition);

                    if (conditionBuilder.Arguments != null && conditionBuilder.Arguments.Length > 0)
                        theWhereEntity.WhereParameterValues.AddRange(conditionBuilder.Arguments);
                    if (conditionBuilder.ParameterNames != null && conditionBuilder.ParameterNames.Length > 0)
                        theWhereEntity.WhereParameterNames.AddRange(conditionBuilder.ParameterNames);
                    if (conditionBuilder.DbTypes != null && conditionBuilder.DbTypes.Length > 0)
                        theWhereEntity.WhereParameterTypes.AddRange(conditionBuilder.DbTypes);
                }
            }
            theWhereEntity.WhereCondition = tsqlBuffer.ToString();
        }

        #endregion
    }
}
