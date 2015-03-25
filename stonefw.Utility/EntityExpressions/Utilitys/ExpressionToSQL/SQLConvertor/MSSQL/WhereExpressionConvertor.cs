using System;
using System.Text;
using stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys;
using stonefw.Utility.EntityExpressions.Entitys;

namespace stonefw.Utility.EntityExpressions.Utilitys.ExpressionToSQL.SQLConvertor.MSSQL
{
    /// <summary>
    /// 查询条件转换器(MSSQL)
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
            string dbTableName = theWhereEntity.EntityType.GetDBTableName();
            if (string.IsNullOrEmpty(dbTableName))
            {
                throw new Exception(string.Format("未给类型{0}设置数据表信息!", typeof(WA).FullName));
            }
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            if (theWhereEntity.DisableTableAlias)
            {
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("]");
            }
            else
            {
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableNameDeclare);
            }

            if (theWhereEntity.WhereExpressions.Count > 0)
            {
                tsqlBuffer.Append(" WHERE ");
                //逐个语句查询，并合并参数
                for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
                {
                    VisitorForMSSQLWhereExpression<WA> conditionBuilder = new VisitorForMSSQLWhereExpression<WA>((theWhereEntity.DisableTableAlias ? dbTableName : theWhereEntity.TableNameDeclare), theWhereEntity);
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
        public string BuildCondition<WA, WB>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
        {
            //重置表的别名
            joinEntity.MainEntity.ResetTableNameDeclare(0);
            joinEntity.EntityToJoin.ResetTableNameDeclare(1);
            //预生成查询条件
            BuildConditionForJoin(joinEntity.MainEntity);
            BuildConditionForJoin(joinEntity.EntityToJoin);

            //生成最终条件
            string dbTableName = joinEntity.MainEntity.EntityType.GetDBTableName();
            string joinDBTableName = joinEntity.EntityToJoin.EntityType.GetDBTableName();
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableNameDeclare);
            if (joinEntity.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableNameDeclare);
            tsqlBuffer.Append(" ON ");
            string joinCon = MSSQLJoinConditionBuilder.GetJoinCondition(joinEntity);
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
        public string BuildCondition<WA, WB>(GenericWhereEntity<WB> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity)
        {
            //重置表的别名
            joinEntity.MainEntity.ResetTableNameDeclare(0);
            joinEntity.EntityToJoin.ResetTableNameDeclare(1);
            //预生成查询条件
            BuildConditionForJoin(joinEntity.MainEntity);
            BuildConditionForJoin(joinEntity.EntityToJoin);

            //生成最终条件
            string dbTableName = joinEntity.MainEntity.EntityType.GetDBTableName();
            string joinDBTableName = joinEntity.EntityToJoin.EntityType.GetDBTableName();
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableNameDeclare);
            if (joinEntity.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableNameDeclare);
            tsqlBuffer.Append(" ON ");
            string joinCon = MSSQLJoinConditionBuilder.GetJoinCondition(joinEntity);
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
        /// <typeparam name="WA"></typeparam>
        /// <typeparam name="WB"></typeparam>
        /// <typeparam name="WC"></typeparam>
        /// <param name="theWhereEntity">查询条件的节点</param>
        /// <param name="joinEntity1">已连接的实体1</param>
        /// <param name="joinEntity2">已连接的实体2</param>
        /// <returns></returns>
        public string BuildCondition<WA, WB, WC>(GenericWhereEntity<WA> theWhereEntity, GenericJoinEntity<WA, WB> joinEntity1, GenericJoinEntity<WA, WC> joinEntity2)
        {
            //重置表的别名
            joinEntity1.MainEntity.ResetTableNameDeclare(0);
            joinEntity1.EntityToJoin.ResetTableNameDeclare(1);
            joinEntity2.EntityToJoin.ResetTableNameDeclare(2);
            //预生成查询条件
            BuildConditionForJoin(theWhereEntity);
            BuildConditionForJoin(joinEntity1.EntityToJoin);
            BuildConditionForJoin(joinEntity2.EntityToJoin);

            //生成最终条件
            string dbTableName = theWhereEntity.EntityType.GetDBTableName();
            string joinDBTableName1 = joinEntity1.EntityToJoin.EntityType.GetDBTableName();
            string joinDBTableName2 = joinEntity2.EntityToJoin.EntityType.GetDBTableName();
            StringBuilder tsqlBuffer = new StringBuilder(2048);
            tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableNameDeclare);

            if (joinEntity1.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity1.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName1).Append("] AS ").Append(joinEntity1.EntityToJoin.TableNameDeclare);
            tsqlBuffer.Append(" ON ");
            string joinCon = MSSQLJoinConditionBuilder.GetJoinCondition(joinEntity1);
            tsqlBuffer.Append(joinCon);

            if (joinEntity2.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" INNER JOIN [");
            }
            else if (joinEntity2.JoinMode == JoinModeEnums.InnerJoin)
            {
                tsqlBuffer.Append(" LEFT JOIN [");
            }
            tsqlBuffer.Append(joinDBTableName2).Append("] AS ").Append(joinEntity2.EntityToJoin.TableNameDeclare);
            tsqlBuffer.Append(" ON ");
            joinCon = MSSQLJoinConditionBuilder.GetJoinCondition(joinEntity2);
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
        private static void BuildConditionForJoin<WA>(GenericWhereEntity<WA> theWhereEntity)
        {
            StringBuilder tsqlBuffer = new StringBuilder(2048);

            if (theWhereEntity.WhereExpressions.Count > 0)
            {
                //逐个语句查询，并合并参数
                for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
                {
                    VisitorForMSSQLWhereExpression<WA> conditionBuilder = new VisitorForMSSQLWhereExpression<WA>(theWhereEntity.TableNameDeclare, theWhereEntity);
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
