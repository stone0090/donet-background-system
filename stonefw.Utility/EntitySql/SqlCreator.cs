using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntitySql.Entity;
using stonefw.Utility.EntitySql.ExpressionVisitor;

namespace stonefw.Utility.EntitySql
{
    /// <summary>
    /// Sql语句的创建器
    /// </summary>
    internal static class SqlCreator
    {
        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbColumnNames"></param>
        /// <returns></returns>
        public static string CreateSelectSql<T>(GenericWhereEntity<T> theWhereEntity, int topCount)
        {
            return CreateSelectSql<T>(theWhereEntity, null, topCount);
        }
        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string CreateSelectSql<T>(GenericWhereEntity<T> theWhereEntity, List<string> dbColumnNames = null, int topCount = 0)
        {

            string dbTableName = EntityMappingTool.GetDbTableName(theWhereEntity.EntityType);

            if (string.IsNullOrEmpty(dbTableName))
                throw new EntitySqlException(string.Format("未给类型{0}设置数据表信息!", theWhereEntity.EntityType.FullName));

            if (dbColumnNames == null)
                dbColumnNames = EntityMappingTool.GetDbColumnNames(typeof(T));

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");

                if (theWhereEntity.DisableTableAlias)
                    sqlBuilder.Append(string.Format("{0}.[{1}]", dbTableName, dbColumnNames[i]));
                else
                    sqlBuilder.Append(string.Format("{0}.[{1}]", theWhereEntity.TableName, dbColumnNames[i]));
            }

            return sqlBuilder.ToString();
        }
        /// <summary>
        /// 创建查询条件
        /// </summary>
        public static string CreateWhereSql<T>(GenericWhereEntity<T> theWhereEntity)
        {
            string dbTableName = EntityMappingTool.GetDbTableName(theWhereEntity.EntityType);

            if (string.IsNullOrEmpty(dbTableName))
                throw new EntitySqlException(string.Format("未给类型{0}设置数据表信息!", theWhereEntity.EntityType.FullName));

            StringBuilder tsqlBuffer = new StringBuilder(2048);

            if (theWhereEntity.DisableTableAlias)
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("]");
            else
                tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableName);

            if (theWhereEntity.WhereExpressions.Count > 0)
            {
                tsqlBuffer.Append(" WHERE ");
                //逐个语句查询，并合并参数
                for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
                {
                    ConditionBuilderGeneric<T> conditionBuilder = new ConditionBuilderGeneric<T>((theWhereEntity.DisableTableAlias ? dbTableName : theWhereEntity.TableName), theWhereEntity);
                    conditionBuilder.Build(theWhereEntity.WhereExpressions[i]);

                    if (i > 0)
                        tsqlBuffer.Append(" AND ");

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
        /// 生成用于插入的Sql命令
        /// </summary>
        public static DbCommand CreateInsertCommand<T>(Database db, T entity)
        {
            var entityType = typeof(T);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(EntityMappingTool.GetDbTableName(entity.GetType())).Append("] (");

            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append("[").Append(notNullDbCloumnNames[i]).Append("]");
            }

            sqlBuilder.Append(") VALUES (");

            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append("@").Append(notNullDbCloumnNames[i]);
            }

            sqlBuilder.Append(")");

            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDbCloumnNames[i], notNullDbColumnTypes[i], notNullEntityPropertys[i].GetValue(entity, null));
            }

            return cmd;
        }
        /// <summary>
        /// 生成用于插入的Sql命令(返回标识值)
        /// </summary>
        public static DbCommand CreateInsertCommandWithIdentity<T>(Database db, T entity)
        {
            var entityType = typeof(T);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(EntityMappingTool.GetDbTableName(entityType)).Append("] (");
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append("[").Append(notNullDbCloumnNames[i]).Append("]");
            }

            sqlBuilder.Append(") VALUES (");

            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append("@").Append(notNullDbCloumnNames[i]);
            }

            sqlBuilder.Append(") select @@identity");

            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDbCloumnNames[i], notNullDbColumnTypes[i], notNullEntityPropertys[i].GetValue(entity, null));
            }

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateCommand<T>(Database db, T entity)
        {
            Type entityType = typeof(T);

            List<string> primaryKeyEntityFieldNames = EntityMappingTool.GetPrimaryKeyOfEntityField(entityType);
            List<string> primaryKeyDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, primaryKeyEntityFieldNames);
            List<DbType> primaryKeyDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, primaryKeyEntityFieldNames);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            //生成Sql语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE [").Append(EntityMappingTool.GetDbTableName(entityType)).Append("] SET ");
            bool firstColumn = true;
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                string loopColumn = notNullDbCloumnNames[i];

                //当前模式主键不更新
                if (primaryKeyDbCloumnNames.Contains(loopColumn))
                    continue;

                sqlBuilder.Append(firstColumn ? "" : ",");
                firstColumn = false;
                sqlBuilder.AppendFormat("[{0}]=@{0}", loopColumn);
                parameterIndex.Add(loopColumn);
            }

            //WHERE
            sqlBuilder.Append(" WHERE ");
            for (int i = 0; i < primaryKeyDbCloumnNames.Count; i++)
            {
                sqlBuilder.Append((i > 0) ? " AND " : "");
                sqlBuilder.AppendFormat("([{0}]=@{0})", primaryKeyDbCloumnNames[i]);
                parameterIndex.Add(primaryKeyDbCloumnNames[i]);
            }

            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < parameterIndex.Count; i++)
            {
                int pIndex = notNullDbCloumnNames.IndexOf(parameterIndex[i]);
                db.AddInParameter(cmd, "@" + notNullDbCloumnNames[pIndex], notNullDbColumnTypes[pIndex], notNullEntityPropertys[pIndex].GetValue(entity, null));
            }

            return cmd;
        }
        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateCommand<T>(Database db, T entity, GenericWhereEntity<T> whereEntity)
        {
            Type entityType = typeof(T);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            //生成Sql语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET ", whereEntity.TableName);
            bool firstColumn = true;
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                string loopColumn = notNullDbCloumnNames[i];
                sqlBuilder.Append(firstColumn ? "" : ",");
                firstColumn = false;
                sqlBuilder.AppendFormat("{0}.[{1}]=@{1}", whereEntity.TableName, loopColumn);
                parameterIndex.Add(loopColumn);
            }

            //WHERE
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);
            sqlBuilder.Append(" ").Append(whereSql);

            //参数
            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDbCloumnNames.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDbCloumnNames[i], notNullDbColumnTypes[i], notNullEntityPropertys[i].GetValue(entity, null));
            }
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateMemberToNullCommand<T>(Database db, GenericWhereEntity<T> whereEntity, string memberName)
        {
            string dbColumnName = EntityMappingTool.GetDbColumnName(typeof(T), memberName);

            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET  {0}.[{1}]=null", whereEntity.TableName, dbColumnName);

            //WHERE
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);
            sqlBuilder.Append(" ").Append(whereSql);

            //参数
            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }
        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateMemberToNullCommand<T>(Database db, GenericWhereEntity<T> whereEntity, List<string> memberNames)
        {
            var dbColumnNames = EntityMappingTool.GetDbColumnNames(typeof(T), memberNames);

            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET", whereEntity.TableName);
            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                sqlBuilder.Append((i == 0) ? "" : ",");
                sqlBuilder.AppendFormat("{0}.[{1}]=null", whereEntity.TableName, dbColumnNames[i]);
            }

            //WHERE
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);
            sqlBuilder.Append(" ").Append(whereSql);

            //参数
            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 创建用于删除的Sql命令
        /// </summary>
        public static DbCommand CreatDeleteCommand<T>(Database db, T entity)
        {
            Type entityType = typeof(T);
            List<string> primaryKeyEntityFieldNames = EntityMappingTool.GetPrimaryKeyOfEntityField(entityType);
            List<string> primaryKeyDbCloumnNames = EntityMappingTool.GetDbColumnNames(entityType, primaryKeyEntityFieldNames);
            List<DbType> primaryKeyDbColumnTypes = EntityMappingTool.GetDbColumnTypes(entityType, primaryKeyEntityFieldNames);
            List<PropertyInfo> primaryKeyPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(entityType, primaryKeyEntityFieldNames);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", EntityMappingTool.GetDbTableName(entityType));
            for (int i = 0; i < primaryKeyEntityFieldNames.Count; i++)
            {
                sqlBuilder.Append((i > 0) ? " AND " : "");
                sqlBuilder.AppendFormat("([{0}]=@{0})", primaryKeyDbCloumnNames[i]);
            }

            //参数
            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < primaryKeyEntityFieldNames.Count; i++)
            {
                db.AddInParameter(cmd, "@" + primaryKeyDbCloumnNames[i], primaryKeyDbColumnTypes[i], primaryKeyPropertyInfos[i].GetValue(entity, null));
            }

            return cmd;
        }
        /// <summary>
        /// 创建用于删除的Sql命令
        /// </summary>
        public static DbCommand CreatDeleteCommand<T>(Database db, GenericWhereEntity<T> whereEntity)
        {
            //生成Sql语句            
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE ", EntityMappingTool.GetDbTableName(typeof(T)));
            sqlBuilder.Append(SqlCreator.CreateWhereSql(whereEntity));
            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);
            return cmd;
        }

        /// <summary>
        /// 填充Sql参数
        /// </summary>
        public static void FillSqlParameters<T>(Database db, DbCommand cmd, GenericWhereEntity<T> whereEntity)
        {
            for (int i = 0; i < whereEntity.WhereParameterNames.Count; i++)
            {
                db.AddInParameter(cmd, whereEntity.WhereParameterNames[i], whereEntity.WhereParameterTypes[i], whereEntity.WhereParameterValues[i]);
            }
        }


        #region 暂时不用的方法   
        ///// <summary>
        ///// 创建成员查询的Sql语句(连接查询)
        ///// </summary>
        //public static string GetJoinMemberSelectSql(
        //    string tableNameA, List<string> dbColumnNamesA,
        //    string tableNameB, List<string> dbColumnNamesB,
        //    int topCount)
        //{
        //    StringBuilder sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("SELECT ");

        //    if (topCount > 0) sqlBuilder.AppendFormat("TOP {0} ", topCount);

        //    for (int i = 0; i < dbColumnNamesA.Count; i++)
        //    {
        //        if (i > 0) sqlBuilder.Append(", ");
        //        sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameA, dbColumnNamesA[i]));
        //    }

        //    for (int i = 0; i < dbColumnNamesB.Count; i++)
        //    {
        //        if (dbColumnNamesA.Count > 0) sqlBuilder.Append(", ");
        //        sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameB, dbColumnNamesB[i]));
        //    }

        //    return sqlBuilder.ToString();
        //}

        ///// <summary>
        ///// 创建成员查询的Sql语句
        ///// </summary>
        //public static string CreateSelectSql<T>(MemberExpression expression, int topCount)
        //{
        //    if (expression == null)
        //        return "";

        //    string dbTableName = EntityMappingTool.GetDbTableName(typeof(T));
        //    string dbColumnName = EntityMappingTool.GetDbColumnName(typeof(T), expression.Member.Name);

        //    StringBuilder sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("SELECT ");

        //    if (topCount > 0)
        //        sqlBuilder.AppendFormat("TOP {0} ", topCount);

        //    if (string.IsNullOrEmpty(dbTableName))
        //        sqlBuilder.AppendFormat("[{0}] ", dbColumnName);
        //    else
        //        sqlBuilder.AppendFormat("{0}.[{1}] ", dbTableName, dbColumnName);

        //    return sqlBuilder.ToString();
        //}

        ///// <summary>
        ///// 创建成员查询的Sql语句
        ///// </summary>
        //public static string CreateSelectSql<T>(NewExpression expression, int topCount)
        //{
        //    if (expression == null)
        //        return "";

        //    string dbTableName = EntityMappingTool.GetDbTableName(typeof(T));

        //    StringBuilder sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("SELECT ");

        //    if (topCount > 0)
        //        sqlBuilder.AppendFormat("TOP {0} ", topCount);

        //    for (int i = 0; i < expression.Members.Count; i++)
        //    {
        //        if (i > 0)
        //            sqlBuilder.Append(", ");

        //        string memberName = expression.Members[i].Name;
        //        string dbColumnName = EntityMappingTool.GetDbColumnName(typeof(T), memberName);

        //        if (string.IsNullOrEmpty(dbTableName))
        //            sqlBuilder.Append(string.Format("[{0}]", dbColumnName));
        //        else
        //            sqlBuilder.Append(string.Format("{0}.[{1}]", dbTableName, dbColumnName));
        //    }

        //    sqlBuilder.Append(" ");
        //    return sqlBuilder.ToString();
        //}

        ///// <summary>
        ///// 创建查询条件
        ///// </summary>
        ///// <param name="theWhereEntity">查询条件的节点</param>
        ///// <param name="joinEntity">已连接的实体</param>
        ///// <returns></returns>
        //public static string BuildCondition<TA, TB>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        //{
        //    //重置表的别名
        //    joinEntity.MainEntity.ResetTableName(0);
        //    joinEntity.EntityToJoin.ResetTableName(1);

        //    //预生成查询条件
        //    BuildConditionForJoin(joinEntity.MainEntity);
        //    BuildConditionForJoin(joinEntity.EntityToJoin);

        //    //生成最终条件
        //    string dbTableName = DbTableMapping.GetDbTableName(joinEntity.MainEntity.EntityType);
        //    string joinDBTableName = DbTableMapping.GetDbTableName(joinEntity.EntityToJoin.EntityType);

        //    StringBuilder tsqlBuffer = new StringBuilder(2048);
        //    tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableName);
        //    if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" INNER JOIN [");

        //    else if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" LEFT JOIN [");

        //    tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableName);
        //    tsqlBuffer.Append(" ON ");

        //    string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity);
        //    tsqlBuffer.Append(joinCon);

        //    bool addWherePart = false;
        //    if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
        //    {
        //        tsqlBuffer.Append(" WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(theWhereEntity.WhereCondition);
        //    }

        //    if (!string.IsNullOrEmpty(joinEntity.EntityToJoin.WhereCondition))
        //    {
        //        tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(joinEntity.EntityToJoin.WhereCondition);
        //    }

        //    if (joinEntity.EntityToJoin.WhereParameterNames.Count > 0)
        //        theWhereEntity.WhereParameterNames.AddRange(joinEntity.EntityToJoin.WhereParameterNames);
        //    if (joinEntity.EntityToJoin.WhereParameterTypes.Count > 0)
        //        theWhereEntity.WhereParameterTypes.AddRange(joinEntity.EntityToJoin.WhereParameterTypes);
        //    if (joinEntity.EntityToJoin.WhereParameterValues.Count > 0)
        //        theWhereEntity.WhereParameterValues.AddRange(joinEntity.EntityToJoin.WhereParameterValues);

        //    return tsqlBuffer.ToString();
        //}

        ///// <summary>
        ///// 创建查询条件
        ///// </summary>
        ///// <param name="theWhereEntity">查询条件的节点</param>
        ///// <param name="joinEntity">已连接的实体</param>
        ///// <returns></returns>
        //public static string BuildCondition<TA, TB>(GenericWhereEntity<TB> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity)
        //{
        //    //重置表的别名
        //    joinEntity.MainEntity.ResetTableName(0);
        //    joinEntity.EntityToJoin.ResetTableName(1);

        //    //预生成查询条件
        //    BuildConditionForJoin(joinEntity.MainEntity);
        //    BuildConditionForJoin(joinEntity.EntityToJoin);

        //    //生成最终条件
        //    string dbTableName = DbTableMapping.GetDbTableName(joinEntity.MainEntity.EntityType);
        //    string joinDBTableName = DbTableMapping.GetDbTableName(joinEntity.EntityToJoin.EntityType);

        //    StringBuilder tsqlBuffer = new StringBuilder(2048);
        //    tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(joinEntity.MainEntity.TableName);

        //    if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" INNER JOIN [");

        //    else if (joinEntity.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" LEFT JOIN [");

        //    tsqlBuffer.Append(joinDBTableName).Append("] AS ").Append(joinEntity.EntityToJoin.TableName);
        //    tsqlBuffer.Append(" ON ");

        //    string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity);
        //    tsqlBuffer.Append(joinCon);

        //    bool addWherePart = false;
        //    if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
        //    {
        //        tsqlBuffer.Append(" WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(theWhereEntity.WhereCondition);
        //    }

        //    if (!string.IsNullOrEmpty(joinEntity.EntityToJoin.WhereCondition))
        //    {
        //        tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(joinEntity.EntityToJoin.WhereCondition);
        //    }

        //    if (joinEntity.EntityToJoin.WhereParameterNames.Count > 0)
        //        theWhereEntity.WhereParameterNames.AddRange(joinEntity.EntityToJoin.WhereParameterNames);
        //    if (joinEntity.EntityToJoin.WhereParameterTypes.Count > 0)
        //        theWhereEntity.WhereParameterTypes.AddRange(joinEntity.EntityToJoin.WhereParameterTypes);
        //    if (joinEntity.EntityToJoin.WhereParameterValues.Count > 0)
        //        theWhereEntity.WhereParameterValues.AddRange(joinEntity.EntityToJoin.WhereParameterValues);

        //    return tsqlBuffer.ToString();
        //}

        ///// <summary>
        ///// 创建查询条件
        ///// </summary>
        ///// <typeparam name="TA"></typeparam>
        ///// <typeparam name="TB"></typeparam>
        ///// <typeparam name="TC"></typeparam>
        ///// <param name="theWhereEntity">查询条件的节点</param>
        ///// <param name="joinEntity1">已连接的实体1</param>
        ///// <param name="joinEntity2">已连接的实体2</param>
        ///// <returns></returns>
        //public static string BuildCondition<TA, TB, TC>(GenericWhereEntity<TA> theWhereEntity, GenericJoinEntity<TA, TB> joinEntity1, GenericJoinEntity<TA, TC> joinEntity2)
        //{
        //    //重置表的别名
        //    joinEntity1.MainEntity.ResetTableName(0);
        //    joinEntity1.EntityToJoin.ResetTableName(1);
        //    joinEntity2.EntityToJoin.ResetTableName(2);

        //    //预生成查询条件
        //    BuildConditionForJoin(theWhereEntity);
        //    BuildConditionForJoin(joinEntity1.EntityToJoin);
        //    BuildConditionForJoin(joinEntity2.EntityToJoin);

        //    //生成最终条件
        //    string dbTableName = DbTableMapping.GetDbTableName(theWhereEntity.EntityType);
        //    string joinDBTableName1 = DbTableMapping.GetDbTableName(joinEntity1.EntityToJoin.EntityType);
        //    string joinDBTableName2 = DbTableMapping.GetDbTableName(joinEntity2.EntityToJoin.EntityType);

        //    StringBuilder tsqlBuffer = new StringBuilder(2048);
        //    tsqlBuffer.Append(" FROM [").Append(dbTableName).Append("] AS ").Append(theWhereEntity.TableName);

        //    if (joinEntity1.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" INNER JOIN [");

        //    else if (joinEntity1.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" LEFT JOIN [");

        //    tsqlBuffer.Append(joinDBTableName1).Append("] AS ").Append(joinEntity1.EntityToJoin.TableName);
        //    tsqlBuffer.Append(" ON ");

        //    string joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity1);
        //    tsqlBuffer.Append(joinCon);

        //    if (joinEntity2.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" INNER JOIN [");

        //    else if (joinEntity2.JoinMode == JoinModeEnum.InnerJoin)
        //        tsqlBuffer.Append(" LEFT JOIN [");

        //    tsqlBuffer.Append(joinDBTableName2).Append("] AS ").Append(joinEntity2.EntityToJoin.TableName);
        //    tsqlBuffer.Append(" ON ");

        //    joinCon = JoinConditionBuilderGeneric.GetJoinCondition(joinEntity2);
        //    tsqlBuffer.Append(joinCon);

        //    bool addWherePart = false;
        //    if (!string.IsNullOrEmpty(theWhereEntity.WhereCondition))
        //    {
        //        tsqlBuffer.Append(" WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(theWhereEntity.WhereCondition);
        //    }

        //    if (!string.IsNullOrEmpty(joinEntity1.EntityToJoin.WhereCondition))
        //    {
        //        tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(joinEntity1.EntityToJoin.WhereCondition);
        //    }

        //    if (!string.IsNullOrEmpty(joinEntity2.EntityToJoin.WhereCondition))
        //    {
        //        tsqlBuffer.Append(addWherePart ? " AND " : " WHERE ");
        //        addWherePart = true;
        //        tsqlBuffer.Append(joinEntity2.EntityToJoin.WhereCondition);
        //    }

        //    if (joinEntity1.EntityToJoin.WhereParameterNames.Count > 0)
        //        theWhereEntity.WhereParameterNames.AddRange(joinEntity1.EntityToJoin.WhereParameterNames);
        //    if (joinEntity1.EntityToJoin.WhereParameterTypes.Count > 0)
        //        theWhereEntity.WhereParameterTypes.AddRange(joinEntity1.EntityToJoin.WhereParameterTypes);
        //    if (joinEntity1.EntityToJoin.WhereParameterValues.Count > 0)
        //        theWhereEntity.WhereParameterValues.AddRange(joinEntity1.EntityToJoin.WhereParameterValues);

        //    if (joinEntity2.EntityToJoin.WhereParameterNames.Count > 0)
        //        theWhereEntity.WhereParameterNames.AddRange(joinEntity2.EntityToJoin.WhereParameterNames);
        //    if (joinEntity2.EntityToJoin.WhereParameterTypes.Count > 0)
        //        theWhereEntity.WhereParameterTypes.AddRange(joinEntity2.EntityToJoin.WhereParameterTypes);
        //    if (joinEntity2.EntityToJoin.WhereParameterValues.Count > 0)
        //        theWhereEntity.WhereParameterValues.AddRange(joinEntity2.EntityToJoin.WhereParameterValues);

        //    return tsqlBuffer.ToString();
        //}

        //#region 私有方法

        ///// <summary>
        ///// 为连接操作创建查询条件
        ///// </summary>
        ///// <param name="theWhereEntity">查询条件的节点</param>
        //private static void BuildConditionForJoin<T>(GenericWhereEntity<T> theWhereEntity)
        //{
        //    StringBuilder tsqlBuffer = new StringBuilder(2048);

        //    if (theWhereEntity.WhereExpressions.Count > 0)
        //    {
        //        //逐个语句查询，并合并参数
        //        for (int i = 0; i < theWhereEntity.WhereExpressions.Count; i++)
        //        {
        //            ConditionBuilderGeneric<T> conditionBuilder = new ConditionBuilderGeneric<T>(theWhereEntity.TableName, theWhereEntity);
        //            conditionBuilder.Build(theWhereEntity.WhereExpressions[i]);

        //            if (i > 0)
        //                tsqlBuffer.Append(" AND ");

        //            tsqlBuffer.Append(conditionBuilder.Condition);

        //            if (conditionBuilder.Arguments != null && conditionBuilder.Arguments.Length > 0)
        //                theWhereEntity.WhereParameterValues.AddRange(conditionBuilder.Arguments);
        //            if (conditionBuilder.ParameterNames != null && conditionBuilder.ParameterNames.Length > 0)
        //                theWhereEntity.WhereParameterNames.AddRange(conditionBuilder.ParameterNames);
        //            if (conditionBuilder.DbTypes != null && conditionBuilder.DbTypes.Length > 0)
        //                theWhereEntity.WhereParameterTypes.AddRange(conditionBuilder.DbTypes);
        //        }
        //    }
        //    theWhereEntity.WhereCondition = tsqlBuffer.ToString();
        //}
        //#endregion
        #endregion
    }
}
