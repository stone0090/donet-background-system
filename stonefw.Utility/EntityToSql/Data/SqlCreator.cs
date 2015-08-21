using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityToSql.Utilitys;
using stonefw.Utility.EntityToSql.Entity;
using stonefw.Utility.EntityToSql.GenSQL;

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

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < dbColumnNamesA.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameA, dbColumnNamesA[i]));
            }

            for (int i = 0; i < dbColumnNamesB.Count; i++)
            {
                if (dbColumnNamesA.Count > 0)
                    sqlBuilder.Append(", ");
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
        public static string GetMemberSelectSql(string tableName, List<string> dbColumnNames)
        {
            return GetMemberSelectSql(tableName, dbColumnNames, 0);
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql(string tableName, List<string> dbColumnNames, int topCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableName, dbColumnNames[i]));
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql(string tableName, Type entityType, MemberExpression expression, int topCount)
        {
            if (expression == null)
                return "";

            string dbColumnName = DbTableMapping.GetDbColumnName(entityType, expression.Member.Name);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            if (string.IsNullOrEmpty(tableName))
                sqlBuilder.AppendFormat("[{0}] ", dbColumnName);
            else
                sqlBuilder.AppendFormat("{0}.[{1}] ", tableName, dbColumnName);

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的Sql语句
        /// </summary>
        public static string GetMemberSelectSql(string tableName, Type entityType, NewExpression expression, int topCount)
        {
            if (expression == null)
                return "";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            if (topCount > 0)
                sqlBuilder.AppendFormat("TOP {0} ", topCount);

            for (int i = 0; i < expression.Members.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");

                string memberName = expression.Members[i].Name;
                string dbColumnName = DbTableMapping.GetDbColumnName(entityType, memberName);

                if (string.IsNullOrEmpty(tableName))
                    sqlBuilder.Append(string.Format("[{0}]", dbColumnName));
                else
                    sqlBuilder.Append(string.Format("{0}.[{1}]", tableName, dbColumnName));
            }
            sqlBuilder.Append(" ");
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 填充Sql参数
        /// </summary>
        public static void FillSqlParameters<E>(Database db, DbCommand cmd, GenericWhereEntity<E> whereEntity)
        {
            for (int i = 0; i < whereEntity.WhereParameterNames.Count; i++)
            {
                db.AddInParameter(cmd, whereEntity.WhereParameterNames[i], whereEntity.WhereParameterTypes[i], whereEntity.WhereParameterValues[i]);
            }
        }

        /// <summary>
        /// 生成用于插入的Sql命令
        /// </summary>
        public static DbCommand CreateInsertCommand<T>(T entity, Database db)
        {
            var entityType = typeof(T);
            List<string> entityFieldNames = DbTableMapping.GetEntityFieldNames(entityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType);

            List<string> notNullEntityMembers = EntityInstanceTool.GetNotNullEntityMembers(entity, entityFieldNames, entityPropertyInfos);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertyInfos(entity, entityPropertyInfos);

            List<string> notNullDBCloumns = DbTableMapping.GetDbColumnNames(entityType, notNullEntityMembers);
            List<DbType> notNullColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(DbTableMapping.GetDbTableName(entity.GetType())).Append("] (");

            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append("[").Append(notNullDBCloumns[i]).Append("]");
            }

            sqlBuilder.Append(") VALUES (");

            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append("@").Append(notNullDBCloumns[i]);
            }

            sqlBuilder.Append(")");

            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entity, null));
            }

            return cmd;
        }

        /// <summary>
        /// 生成用于插入的Sql命令(返回标识值)
        /// </summary>
        public static DbCommand CreateInsertCommandWithIdentity<T>(T entity, Database db)
        {
            var entityType = typeof(T);
            List<string> entityFieldNames = DbTableMapping.GetEntityFieldNames(entityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType);

            List<string> notNullEntityMembers = EntityInstanceTool.GetNotNullEntityMembers(entity, entityFieldNames, entityPropertyInfos);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertyInfos(entity, entityPropertyInfos);

            List<string> notNullDBCloumns = DbTableMapping.GetDbColumnNames(entityType, notNullEntityMembers);
            List<DbType> notNullColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(DbTableMapping.GetDbTableName(entityType)).Append("] (");
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append("[").Append(notNullDBCloumns[i]).Append("]");
            }

            sqlBuilder.Append(") VALUES (");

            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                    sqlBuilder.Append(", ");
                sqlBuilder.Append("@").Append(notNullDBCloumns[i]);
            }

            sqlBuilder.Append(") select @@identity");

            DbCommand cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entity, null));
            }

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateCommand(Database db, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys, List<string> dbPrimaryKeys, List<string> primaryKeyDBCloumns)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityMembers);
            List<DbType> primaryKeyColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, dbPrimaryKeys);

            //生成Sql语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE [").Append(DbTableMapping.GetDbTableName(entityType)).Append("] SET ");
            bool firstColumn = true;
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                string loopColumn = notNullDBCloumns[i];
                //当前模式主键不更新
                if (primaryKeyDBCloumns.Contains(loopColumn))
                {
                    continue;
                }

                sqlBuilder.Append(firstColumn ? "" : ",");
                firstColumn = false;
                sqlBuilder.AppendFormat("[{0}]=@{0}", loopColumn);
                parameterIndex.Add(loopColumn);
            }

            //WHERE
            sqlBuilder.Append(" WHERE ");
            for (int i = 0; i < primaryKeyDBCloumns.Count; i++)
            {
                sqlBuilder.Append((i > 0) ? " AND " : "");
                sqlBuilder.AppendFormat("([{0}]=@{0})", primaryKeyDBCloumns[i]);
                parameterIndex.Add(primaryKeyDBCloumns[i]);
            }

            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < parameterIndex.Count; i++)
            {
                int pIndex = notNullDBCloumns.IndexOf(parameterIndex[i]);
                db.AddInParameter(cmd, "@" + notNullDBCloumns[pIndex], notNullColumnTypes[pIndex], notNullEntityPropertys[pIndex].GetValue(entityInstance, null));
            }
            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, notNullEntityMembers);

            //生成Sql语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET ", whereEntity.TableName);
            bool firstColumn = true;
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                string loopColumn = notNullDBCloumns[i];
                sqlBuilder.Append(firstColumn ? "" : ",");
                firstColumn = false;
                sqlBuilder.AppendFormat("{0}.[{1}]=@{1}", whereEntity.TableName, loopColumn);
                parameterIndex.Add(loopColumn);
            }

            //WHERE
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(" ").Append(whereSql);

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entityInstance, null));
            }
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateMemberToNULLCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, string memberName)
        {
            DbCommand cmd = null;
            string dbColumnName = DbTableMapping.GetDbColumnName(entityType, memberName);
            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET  {0}.[{1}]=null", whereEntity.TableName, dbColumnName);

            //WHERE
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(" ").Append(whereSql);

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的Sql命令
        /// </summary>
        public static DbCommand CreateUpdateMemberToNULLCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, List<string> memberNames)
        {
            DbCommand cmd = null;
            var dbColumnNames = DbTableMapping.GetDbColumnNames(entityType, memberNames);
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
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 创建用于删除的Sql命令
        /// </summary>
        public static DbCommand CreatDeleteCommand(Database db, Type entityType, object entityInstance, List<string> conditionMembers, List<string> conditionDBCloumns, List<PropertyInfo> conditionEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> conditionColumnTypes = DbTableMapping.GetDbColumnTypes(entityType, conditionMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", DbTableMapping.GetDbTableName(entityType));

            for (int i = 0; i < conditionMembers.Count; i++)
            {
                sqlBuilder.Append((i > 0) ? " AND " : "");
                sqlBuilder.AppendFormat("([{0}]=@{0})", conditionDBCloumns[i]);
            }

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < conditionMembers.Count; i++)
            {
                db.AddInParameter(cmd, "@" + conditionDBCloumns[i], conditionColumnTypes[i], conditionEntityPropertys[i].GetValue(entityInstance, null));
            }

            return cmd;
        }

        /// <summary>
        /// 创建用于删除的Sql命令
        /// </summary>
        public static DbCommand CreatDeleteCommand<E>(Database db, Type entityType, GenericWhereEntity<E> whereEntity)
        {
            DbCommand cmd = null;
            //生成Sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE ");
            string whereSql = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(whereSql);
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSqlParameters(db, cmd, whereEntity);
            return cmd;
        }
    }
}
