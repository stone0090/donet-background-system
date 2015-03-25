using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions.Utilitys;
using stonefw.Utility.EntityExpressions.Entitys;

namespace stonefw.Utility.EntityExpressions.Data
{
    /// <summary>
    /// SQL语句的创建器
    /// </summary>
    internal static class SqlCreator
    {
        /// <summary>
        /// 创建成员查询的SQL语句(连接查询)
        /// </summary>
        /// <param name="tableNameDeclareA"></param>
        /// <param name="dbColumnNamesA"></param>
        /// <param name="tableNameDeclareB"></param>
        /// <param name="dbColumnNamesB"></param>
        /// <param name="selectCount">检索的记录的数量</param>
        /// <returns></returns>
        public static string GetJoinMemberSelectSQL(string tableNameDeclareA, List<string> dbColumnNamesA, 
            string tableNameDeclareB, List<string> dbColumnNamesB, 
            int selectCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            if (selectCount > 0)
            {
                sqlBuilder.AppendFormat("TOP {0} ", selectCount);
            }
            for (int i = 0; i < dbColumnNamesA.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameDeclareA, dbColumnNamesA[i]));
            }
            for (int i = 0; i < dbColumnNamesB.Count; i++)
            {
                if (dbColumnNamesA.Count > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameDeclareB, dbColumnNamesB[i]));
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的SQL语句
        /// </summary>
        /// <param name="tableNameDeclare"></param>
        /// <param name="dbColumnNames"></param>
        /// <returns></returns>
        public static string GetMemberSelectSQL(string tableNameDeclare, List<string> dbColumnNames)
        {
            return GetMemberSelectSQL(tableNameDeclare, dbColumnNames, 0);
        }

        /// <summary>
        /// 创建成员查询的SQL语句
        /// </summary>
        /// <param name="tableNameDeclare"></param>
        /// <param name="dbColumnNames"></param>
        /// <param name="selectCount">检索的记录的数量</param>
        /// <returns></returns>
        public static string GetMemberSelectSQL(string tableNameDeclare, List<string> dbColumnNames, int selectCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            if (selectCount > 0)
            {
                sqlBuilder.AppendFormat("TOP {0} ", selectCount);
            }
            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameDeclare, dbColumnNames[i]));
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的SQL语句
        /// </summary>
        /// <param name="tableNameDeclare">表的别名</param>
        /// <param name="entityType">实体的类别</param>
        /// <param name="expression">访问单个成员的表达式</param>
        /// <param name="selectCount">检索的记录的数量</param>
        /// <returns>SQL语句中的查询部分</returns>
        public static string GetMemberSelectSQL(string tableNameDeclare, Type entityType,  MemberExpression expression, int selectCount)
        {
            if (expression == null)
            {
                return "";
            }
            string dbColumnName = EntityUtilitys.GetDbColumnName(entityType, expression.Member.Name);
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            if (selectCount > 0)
            {
                sqlBuilder.AppendFormat("TOP {0} ", selectCount);
            }

            if (string.IsNullOrEmpty(tableNameDeclare))
            {
                sqlBuilder.AppendFormat("[{0}] ", dbColumnName);
            }
            else
            {
                sqlBuilder.AppendFormat("{0}.[{1}] ", tableNameDeclare, dbColumnName);
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 创建成员查询的SQL语句
        /// </summary>
        /// <param name="tableNameDeclare"></param>
        /// <param name="entityType"></param>
        /// <param name="expression"></param>
        /// <param name="selectCount">检索的记录的数量</param>
        /// <returns></returns>
        public static string GetMemberSelectSQL(string tableNameDeclare, Type entityType, NewExpression expression, int selectCount)
        {
            if (expression == null)
            {
                return "";
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            if (selectCount > 0)
            {
                sqlBuilder.AppendFormat("TOP {0} ", selectCount);
            }
            for (int i = 0; i < expression.Members.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                string memberName = expression.Members[i].Name;
                string dbColumnName = EntityUtilitys.GetDbColumnName(entityType, memberName);
                if (string.IsNullOrEmpty(tableNameDeclare))
                {
                    sqlBuilder.Append(string.Format("[{0}]", dbColumnName));
                }
                else
                {
                    sqlBuilder.Append(string.Format("{0}.[{1}]", tableNameDeclare, dbColumnName));
                }
            }
            sqlBuilder.Append(" ");
            return sqlBuilder.ToString();
        }        

        /// <summary>
        /// 填充SQL参数
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmd"></param>
        /// <param name="whereEntity"></param>
        public static void FillSQLParameters<E>(Database db, DbCommand cmd, GenericWhereEntity<E> whereEntity)
        {
            for (int i = 0; i < whereEntity.WhereParameterNames.Count; i++)
            {
                db.AddInParameter(cmd, whereEntity.WhereParameterNames[i], whereEntity.WhereParameterTypes[i], whereEntity.WhereParameterValues[i]);
            }
        }

        /// <summary>
        /// 生成用于插入的SQL命令
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityType"></param>
        /// <param name="entityInstance"></param>
        /// <param name="notNullEntityMembers"></param>
        /// <param name="notNullDBCloumns"></param>
        /// <param name="notNullEntityPropertys"></param>
        /// <returns></returns>
        public static DbCommand CreateInsertCommand(Database db, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, notNullEntityMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(EntityUtilitys.GetTableNameOfEntity(entityType)).Append("] (");
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append("[").Append(notNullDBCloumns[i]).Append("]");
            }
            sqlBuilder.Append(") VALUES (");
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append("@").Append(notNullDBCloumns[i]);
            }
            sqlBuilder.Append(")");
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entityInstance, null));
            }
            return cmd;
        }

        /// <summary>
        /// 生成用于插入的SQL命令(返回标识值)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityType"></param>
        /// <param name="entityInstance"></param>
        /// <param name="notNullEntityMembers"></param>
        /// <param name="notNullDBCloumns"></param>
        /// <param name="notNullEntityPropertys"></param>
        /// <returns></returns>
        public static DbCommand CreateInsertCommandWithIdentity(Database db, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, notNullEntityMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO [").Append(EntityUtilitys.GetTableNameOfEntity(entityType)).Append("] (");
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append("[").Append(notNullDBCloumns[i]).Append("]");
            }
            sqlBuilder.Append(") VALUES (");
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(", ");
                }
                sqlBuilder.Append("@").Append(notNullDBCloumns[i]);
            }
            sqlBuilder.Append(") select @@identity");
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entityInstance, null));
            }
            return cmd;
        }

        /// <summary>
        /// 生成用于更新的SQL命令
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityType"></param>
        /// <param name="entityInstance"></param>
        /// <param name="notNullEntityMembers"></param>
        /// <param name="notNullDBCloumns"></param>
        /// <param name="notNullEntityPropertys"></param>
        /// <param name="entityMembersOfPrimaryKey"></param>
        /// <param name="primaryKeyDBCloumns"></param>
        /// <returns></returns>
        public static DbCommand CreateUpdateCommand(Database db, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys, List<string> entityMembersOfPrimaryKey, List<string> primaryKeyDBCloumns)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, notNullEntityMembers);
            List<DbType> primaryKeyColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, entityMembersOfPrimaryKey);

            //生成SQL语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE [").Append(EntityUtilitys.GetTableNameOfEntity(entityType)).Append("] SET ");
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
        /// 生成用于更新的SQL命令
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereEntity"></param>
        /// <param name="entityType"></param>
        /// <param name="entityInstance"></param>
        /// <param name="notNullEntityMembers"></param>
        /// <param name="notNullDBCloumns"></param>
        /// <param name="notNullEntityPropertys"></param>
        /// <returns></returns>
        public static DbCommand CreateUpdateCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, object entityInstance, List<string> notNullEntityMembers, List<string> notNullDBCloumns, List<PropertyInfo> notNullEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> notNullColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, notNullEntityMembers);

            //生成SQL语句
            List<string> parameterIndex = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET ", whereEntity.TableNameDeclare);
            bool firstColumn = true;
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                string loopColumn = notNullDBCloumns[i];
                sqlBuilder.Append(firstColumn ? "" : ",");
                firstColumn = false;
                sqlBuilder.AppendFormat("{0}.[{1}]=@{1}", whereEntity.TableNameDeclare, loopColumn);
                parameterIndex.Add(loopColumn);
            }

            //WHERE
            string whereSQL = BuildDBQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(" ").Append(whereSQL);

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            for (int i = 0; i < notNullDBCloumns.Count; i++)
            {
                db.AddInParameter(cmd, "@" + notNullDBCloumns[i], notNullColumnTypes[i], notNullEntityPropertys[i].GetValue(entityInstance, null));
            }
            FillSQLParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的SQL命令
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereEntity"></param>
        /// <param name="entityType"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static DbCommand CreateUpdateMemberToNULLCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, string memberName)
        {
            DbCommand cmd = null;
            string dbColumnName = EntityUtilitys.GetDbColumnName(entityType, memberName);
            //生成SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET  {0}.[{1}]=null", whereEntity.TableNameDeclare, dbColumnName);

            //WHERE
            string whereSQL = BuildDBQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(" ").Append(whereSQL);

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSQLParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 生成用于更新的SQL命令
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereEntity"></param>
        /// <param name="entityType"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static DbCommand CreateUpdateMemberToNULLCommand<E>(Database db, GenericWhereEntity<E> whereEntity, Type entityType, List<string> memberNames)
        {
            DbCommand cmd = null;
            var dbColumnNames = EntityUtilitys.GetDBColumnNameOfAppointMembers(entityType, memberNames);
            //生成SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE {0} SET", whereEntity.TableNameDeclare);
            for (int i = 0; i < dbColumnNames.Count; i++)
            {
                sqlBuilder.Append((i == 0) ? "" : ",");
                sqlBuilder.AppendFormat("{0}.[{1}]=null", whereEntity.TableNameDeclare, dbColumnNames[i]);
            }

            //WHERE
            string whereSQL = BuildDBQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(" ").Append(whereSQL);

            //参数
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSQLParameters(db, cmd, whereEntity);

            return cmd;
        }

        /// <summary>
        /// 创建用于删除的SQL命令
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityType"></param>
        /// <param name="entityInstance"></param>
        /// <param name="conditionMembers"></param>
        /// <param name="conditionDBCloumns"></param>
        /// <param name="conditionEntityPropertys"></param>
        /// <returns></returns>
        public static DbCommand CreatDeleteCommand(Database db, Type entityType, object entityInstance, List<string> conditionMembers, List<string> conditionDBCloumns, List<PropertyInfo> conditionEntityPropertys)
        {
            DbCommand cmd = null;
            List<DbType> conditionColumnTypes = EntityUtilitys.GetDBColumnTypeOfAppointMembers(entityType, conditionMembers);

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", EntityUtilitys.GetTableNameOfEntity(entityType));

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
        /// 创建用于删除的SQL命令
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="db"></param>
        /// <param name="entityType"></param>
        /// <param name="whereEntity"></param>
        /// <returns></returns>
        public static DbCommand CreatDeleteCommand<E>(Database db, Type entityType, GenericWhereEntity<E> whereEntity)
        {
            DbCommand cmd = null;
            //生成SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE ");
            string whereSQL = BuildDBQuerysGeneric.BuildCondition(whereEntity);
            sqlBuilder.Append(whereSQL);
            cmd = db.GetSqlStringCommand(sqlBuilder.ToString());
            FillSQLParameters(db, cmd, whereEntity);
            return cmd;
        }
    }
}
