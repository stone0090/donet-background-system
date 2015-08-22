using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityToSql.Utilitys;
using stonefw.Utility.EntityToSql.Entity;

namespace stonefw.Utility.EntityToSql.Data
{
    /// <summary>
    /// Sql语句的创建器
    /// </summary>
    internal static class SqlCreator
    {
        /// <summary>
        /// 创建成员查询的Sql语句(连接查询)
        /// </summary>
        public static string GetJoinMemberSelectSql(
            string tableNameA, List<string> dbColumnNamesA,
            string tableNameB, List<string> dbColumnNamesB,
            int topCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0) sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < dbColumnNamesA.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameA, dbColumnNamesA[i]));
            }

            for (int i = 0; i < dbColumnNamesB.Count; i++)
            {
                if (dbColumnNamesA.Count > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameB, dbColumnNamesB[i]));
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbColumnNames"></param>
        /// <returns></returns>
        public static string GetMemberSelectSql<T>(int topCount)
        {
            List<string> dbColumnNames = null;
            return GetMemberSelectSql<T>(dbColumnNames, topCount);
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql<T>(List<string> dbColumnNames = null, int topCount = 0)
        {
            string tableName = DbTableMapping.GetDbTableName(typeof(T));

            if (dbColumnNames == null)
                dbColumnNames = DbTableMapping.GetDbColumnNames(typeof(T));

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                if (i > 0) sqlBuilder.Append(", ");
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableName, dbColumnNames[i]));
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql<T>(MemberExpression expression, int topCount)
        {
            if (expression == null)
                return "";

            string dbTableName = DbTableMapping.GetDbTableName(typeof(T));
            string dbColumnName = DbTableMapping.GetDbColumnName(typeof(T), expression.Member.Name);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            if (string.IsNullOrEmpty(dbTableName))
                sqlBuilder.AppendFormat("[{0}] ", dbColumnName);
            else
                sqlBuilder.AppendFormat("{0}.[{1}] ", dbTableName, dbColumnName);

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql<T>(NewExpression expression, int topCount)
        {
            if (expression == null)
                return "";

            string dbTableName = DbTableMapping.GetDbTableName(typeof(T));

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < expression.Members.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");

                string memberName = expression.Members[i].Name;
                string dbColumnName = DbTableMapping.GetDbColumnName(typeof(T), memberName);

                if (string.IsNullOrEmpty(dbTableName))
                    sqlBuilder.Append(string.Format("[{0}]", dbColumnName));
                else
                    sqlBuilder.Append(string.Format("{0}.[{1}]", dbTableName, dbColumnName));
            }

            sqlBuilder.Append(" ");
            return sqlBuilder.ToString();
        }


        /// <summary>
        /// 生成用于插入的Sql命令
        /// </summary>
        public static DbCommand CreateInsertCommand<T>(Database db, T entity)
        {
            var entityType = typeof(T);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(DbTableMapping.GetDbTableName(entity.GetType())).Append("] (");

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
            List<string> notNullDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(DbTableMapping.GetDbTableName(entityType)).Append("] (");
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

            List<string> primaryKeyEntityFieldNames = DbTableMapping.GetPrimaryKeyOfEntityField(entityType);
            List<string> primaryKeyDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, primaryKeyEntityFieldNames);
            List<DbType> primaryKeyDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, primaryKeyEntityFieldNames);

            List<string> notNullEntityFields = EntityInstanceTool.GetNotNullFields(entity);
            List<string> notNullDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityFields);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertys(entity);

            //生成Sql语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE [").Append(DbTableMapping.GetDbTableName(entityType)).Append("] SET ");
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
            List<string> notNullDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, notNullEntityFields);
            List<DbType> notNullDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityFields);
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
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
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
            string dbColumnName = DbTableMapping.GetDbColumnName(typeof(T), memberName);

            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET  {0}.[{1}]=null", whereEntity.TableName, dbColumnName);

            //WHERE
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
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
            var dbColumnNames = DbTableMapping.GetDbColumnNames(typeof(T), memberNames);

            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET", whereEntity.TableName);
            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                sqlBuilder.Append((i == 0) ? "" : ",");
                sqlBuilder.AppendFormat("{0}.[{1}]=null", whereEntity.TableName, dbColumnNames[i]);
            }

            //WHERE
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
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
            List<string> primaryKeyEntityFieldNames = DbTableMapping.GetPrimaryKeyOfEntityField(entityType);
            List<string> primaryKeyDbCloumnNames = DbTableMapping.GetDbColumnNames(entityType, primaryKeyEntityFieldNames);
            List<DbType> primaryKeyDbColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, primaryKeyEntityFieldNames);
            List<PropertyInfo> primaryKeyPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType, primaryKeyEntityFieldNames);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", DbTableMapping.GetDbTableName(entityType));
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
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", DbTableMapping.GetDbTableName(typeof(T)));
            sqlBuilder.Append(BuildDbQuerysGeneric.BuildCondition(whereEntity));
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
    }
}
