using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using stonefw.Utility.EntityToSql.Utilitys;
using stonefw.Utility.EntityToSql.Entity;
using stonefw.Utility.EntityToSql.GenSQL;

namespace stonefw.Utility.EntityToSql.Data
{
    /// <summary>
    /// 实体类的操作
    /// </summary>
    public static class EntityOperations
    {
        #region 实体类的增删改查

        #region 新增操作

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        public static void InsertEntity<T>(T entity, Database db)
        {
            using (DbCommand cmd = SqlCreator.CreateInsertCommand(entity, db))
            {
                int rowCount = db.ExecuteNonQuery(cmd);
                if (rowCount <= 0) throw new Exception("新增记录失败(DB)！");
            }
        }

        /// <summary>
        /// 插入实体，并返回标识列的值
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        /// <returns>标识列的值</returns>
        public static object InsertEntityWithIdentity<T>(T entity, Database db)
        {
            using (DbCommand cmd = SqlCreator.CreateInsertCommandWithIdentity(entity, db))
            {
                object retObj = db.ExecuteScalar(cmd);
                return retObj;
            }
        }

        #endregion

        #region 删除操作

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression">删除的条件</param>
        /// <param name="db"></param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity2<T>(Expression<Func<T, bool>> conditionExpression)
        {
            if (conditionExpression == null)
                throw new EntityExpressionsException("删除记录时，必须指定Where条件，否则将导致整个表的数据被删除！");

            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                whereEntity.Where(conditionExpression);
                return DeleteEntity(whereEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression">删除的条件</param>
        /// <param name="db"></param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity2<T>(Expression<Func<T, bool>> conditionExpression, Database db)
        {
            if (conditionExpression == null)
            {
                throw new EntityExpressionsException("删除记录时，必须指定Where条件，否则将导致整个表的数据被删除！");
            }

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            whereEntity.Where(conditionExpression);
            return DeleteEntity(whereEntity, db);
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity<T>(GenericWhereEntity<T> whereEntity, Database db)
        {
            //DELETE时应该禁用别名
            whereEntity.DisableTableAlias = true;

            Type entityType = typeof(T);
            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreatDeleteCommand(db, entityType, whereEntity);
                return db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity<T>(T entity, Database db)
        {
            //获取主键相关的字段
            Type entityType = typeof(T);
            List<string> dbPrimaryKeys = DbTableMapping.GetPrimaryKeyOfEntityField(entityType);
            if (dbPrimaryKeys == null || dbPrimaryKeys.Count == 0)
                throw new EntityExpressionsException(string.Format("实体类{0}未设置主键字段！", entityType.FullName));

            List<string> primaryKeyDBCloumns = DbTableMapping.GetDbColumnNames(entityType, dbPrimaryKeys);
            List<PropertyInfo> primaryKeyPropertys = DbTableMapping.GetEntityPropertyInfos(entityType, dbPrimaryKeys);

            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreatDeleteCommand(db, entityType, entity, dbPrimaryKeys, primaryKeyDBCloumns, primaryKeyPropertys);
                return db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        #endregion

        #region 修改操作

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        public static void UpdateEntity<T>(T entity, Database db)
        {
            //检索数据库相关的字段和属性
            Type entityType = typeof(T);
            List<string> entityFieldNames = DbTableMapping.GetEntityFieldNames(entityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType);

            //获取主键相关的字段
            List<string> dbPrimaryKeys = DbTableMapping.GetPrimaryKeyOfEntityField(entityType);
            if (dbPrimaryKeys == null || dbPrimaryKeys.Count == 0)
            {
                throw new EntityExpressionsException(string.Format("实体类{0}未设置主键字段！", entityType.FullName));
            }
            //获取不为空的字段列表和属性
            List<string> notNullEntityMembers = EntityInstanceTool.GetNotNullEntityMembers(entity, entityFieldNames, entityPropertyInfos);
            for (int i = 0; i < dbPrimaryKeys.Count; i++)
            {
                if (notNullEntityMembers == null || !notNullEntityMembers.Any(n => n == dbPrimaryKeys[i]))
                {
                    throw new EntityExpressionsException(string.Format("未给实体类{0}实例的主键字段{1}赋值！", entityType.FullName, dbPrimaryKeys[i]));
                }
            }
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertyInfos(entity, entityPropertyInfos);
            //检索对应的数据库字段
            List<string> notNullDBCloumns = DbTableMapping.GetDbColumnNames(entityType, notNullEntityMembers);
            List<string> primaryKeyDBCloumns = DbTableMapping.GetDbColumnNames(entityType, dbPrimaryKeys);

            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreateUpdateCommand(db, entityType, entity, notNullEntityMembers, notNullDBCloumns, notNullEntityPropertys, dbPrimaryKeys, primaryKeyDBCloumns);
                int rowCount = db.ExecuteNonQuery(cmd);
                if (rowCount != 1)
                {
                    throw new Exception("修改记录失败(DB)！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        public static void UpdateEntity2<T>(Expression<Func<T, bool>> conditionExpression, T entity, Database db)
        {
            if (conditionExpression == null)
            {
                throw new EntityExpressionsException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
            }

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            UpdateEntity(whereEntity, entity, db);
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="entity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        public static void UpdateEntity<T>(GenericWhereEntity<T> whereEntity, T entity, Database db)
        {
            //检索数据库相关的字段和属性
            Type entityType = typeof(T);
            List<string> entityFieldNames = DbTableMapping.GetEntityFieldNames(entityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType);

            //获取不为空的字段列表和属性
            List<string> notNullEntityMembers = EntityInstanceTool.GetNotNullEntityMembers(entity, entityFieldNames, entityPropertyInfos);
            List<PropertyInfo> notNullEntityPropertys = EntityInstanceTool.GetNotNullEntityPropertyInfos(entity, entityPropertyInfos);
            //检索对应的数据库字段
            List<string> notNullDBCloumns = DbTableMapping.GetDbColumnNames(entityType, notNullEntityMembers);

            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreateUpdateCommand(db, whereEntity, entityType, entity, notNullEntityMembers, notNullDBCloumns, notNullEntityPropertys);
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="conditionExpression"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        public static void SetMemberNull2<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db)
        {
            if (conditionExpression == null)
            {
                throw new EntityExpressionsException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
            }

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }
            SetMemberNull(whereEntity, keySelector, db);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionExpression"></param>
        /// <param name="db"></param>
        /// <param name="keySelectors"></param>
        public static void SetMembersNull2<T>(Expression<Func<T, bool>> conditionExpression, Database db, params Expression<Func<T, object>>[] keySelectors)
        {
            if (conditionExpression == null)
            {
                throw new EntityExpressionsException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");
            }

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }
            SetMembersNull(whereEntity, db, keySelectors);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereEntity"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        public static void SetMemberNull<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db)
        {
            Type entityType = typeof(T);
            MemberExpression m = keySelector.Body as MemberExpression;
            string memberName = m.Member.Name;

            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreateUpdateMemberToNULLCommand(db, whereEntity, entityType, memberName);
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereEntity"></param>
        /// <param name="db"></param>
        /// <param name="keySelectors"></param>
        public static void SetMembersNull<T>(GenericWhereEntity<T> whereEntity, Database db, params Expression<Func<T, object>>[] keySelectors)
        {
            Type entityType = typeof(T);
            //检索要UPDATE的成员列表
            List<string> memberNames = new List<string>(keySelectors.Length);
            for (int i = 0; i < keySelectors.Length; i++)
            {
                //对于引用类型的成员访问,使用如下转换
                MemberExpression m = keySelectors[i].Body as MemberExpression;
                if (m != null)
                {
                    memberNames.Add(m.Member.Name);
                    continue;
                }
                //对于值类型的成员访问需要使用如下转换
                var ue = keySelectors[i].Body as UnaryExpression;
                if (ue != null)
                {
                    MemberExpression me = ue.Operand as MemberExpression;
                    if (me != null)
                    {
                        memberNames.Add(me.Member.Name);
                        continue;
                    }
                }
            }

            DbCommand cmd = null;
            try
            {
                cmd = SqlCreator.CreateUpdateMemberToNULLCommand(db, whereEntity, entityType, memberNames);
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }

        #endregion

        #region 单表查询

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="T">实体的类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <returns></returns>
        public static T ReadEntity2<T>(Expression<Func<T, bool>> conditionExpression) where T : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                if (conditionExpression != null)
                {
                    whereEntity.Where(conditionExpression);
                }
                return ReadEntity(whereEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="T">实体的类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="db">数据库连接</param>
        /// <returns></returns>
        public static T ReadEntity2<T>(Expression<Func<T, bool>> conditionExpression, Database db) where T : class, new()
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            return ReadEntity(whereEntity, db);
        }

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>实体类的实例</returns>
        public static T ReadEntity<T>(GenericWhereEntity<T> whereEntity, Database db) where T : class, new()
        {
            T entity = null;
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(whereEntity.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(whereEntity.EntityType);
            string selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, dbColumnNames, 1);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                if (reader.Read())
                {
                    entity = EntityInstanceTool.FillOneEntity<T>(reader, entityPropertyInfos);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<T> ReadEntityList2<T>(Expression<Func<T, bool>> conditionExpression, params int[] maxRowCounts) where T : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                if (conditionExpression != null)
                {
                    whereEntity.Where(conditionExpression);
                }

                return ReadEntityList(whereEntity, db, maxRowCounts);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<T> ReadEntityList2<T>(Expression<Func<T, bool>> conditionExpression, Database db, params int[] maxRowCounts) where T : class, new()
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            return ReadEntityList(whereEntity, db, maxRowCounts);
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<T> ReadEntityList<T>(GenericWhereEntity<T> whereEntity, Database db, params int[] maxRowCounts) where T : class, new()
        {
            List<T> entitys = new List<T>();
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(whereEntity.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(whereEntity.EntityType);
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            string selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, dbColumnNames, rowCount);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                while (reader.Read())
                {
                    var entity = EntityInstanceTool.FillOneEntity<T>(reader, entityPropertyInfos);
                    entitys.Add(entity);
                }
                return entitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int GetEntityCount2<T>(Expression<Func<T, bool>> conditionExpression)
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                if (conditionExpression != null)
                {
                    whereEntity.Where(conditionExpression);
                }

                return GetEntityCount(whereEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int GetEntityCount2<T>(Expression<Func<T, bool>> conditionExpression, Database db)
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            return GetEntityCount(whereEntity, db);
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int GetEntityCount<T>(GenericWhereEntity<T> whereEntity, Database db)
        {
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(whereEntity.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(whereEntity.EntityType);
            string selectSQL = "SELECT COUNT(*) ";
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            try
            {
                var countObj = db.ExecuteScalar(cmd);
                return (int)countObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询是否存在指定条件的记录
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <returns>是否存在指定条件的记录</returns>
        public static bool ExistsRecord2<T>(Expression<Func<T, bool>> conditionExpression)
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                if (conditionExpression != null)
                {
                    whereEntity.Where(conditionExpression);
                }

                return ExistsRecord(whereEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 查询是否存在指定条件的记录
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>是否存在指定条件的记录</returns>
        public static bool ExistsRecord2<T>(Expression<Func<T, bool>> conditionExpression, Database db)
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            return ExistsRecord(whereEntity, db);
        }

        /// <summary>
        /// 查询是否存在指定条件的记录
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>是否存在指定条件的记录</returns>
        public static bool ExistsRecord<T>(GenericWhereEntity<T> whereEntity, Database db)
        {
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(whereEntity.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(whereEntity.EntityType);
            string selectSQL = "SELECT 1 WHERE EXISTS (SELECT 1";
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL + ")");
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            try
            {
                var countObj = db.ExecuteScalar(cmd);
                if (countObj == null || countObj is DBNull)
                    return false;
                return ((int)countObj > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        #endregion

        #region 双表连接查询

        /// <summary>
        /// 关联读取主表
        /// </summary>
        /// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        /// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        /// <param name="joinEntity">连接条件</param>
        /// <returns></returns>
        public static MainTable ReadMainEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where MainTable : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
                if (mainConditionExpression != null)
                {
                    mainWhereEntity.Where(mainConditionExpression);
                }
                GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
                if (subConditionExpression != null)
                {
                    subWhereEntity.Where(subConditionExpression);
                }
                GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
                joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

                return ReadMainEntity(joinEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 关联读取主表
        /// </summary>
        /// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        /// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        /// <param name="joinEntity">连接条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns></returns>
        public static MainTable ReadMainEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression,
            Database db) where MainTable : class, new()
        {
            GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
            if (mainConditionExpression != null)
            {
                mainWhereEntity.Where(mainConditionExpression);
            }
            GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
            if (subConditionExpression != null)
            {
                subWhereEntity.Where(subConditionExpression);
            }
            GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
            joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

            return ReadMainEntity(joinEntity, db);
        }

        /// <summary>
        /// 关联读取主表
        /// </summary>
        /// <typeparam name="MainTable"></typeparam>
        /// <typeparam name="SubTable"></typeparam>
        /// <param name="joinEntity"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static MainTable ReadMainEntity<MainTable, SubTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity,
            Database db) where MainTable : class, new()
        {
            MainTable entity = null;
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(joinEntity.MainEntity, joinEntity);

            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.MainEntity.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(joinEntity.MainEntity.EntityType);

            string selectSQL = SqlCreator.GetMemberSelectSql(joinEntity.MainEntity.TableName, dbColumnNames, 1);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                if (reader.Read())
                {
                    entity = EntityInstanceTool.FillOneEntity<MainTable>(reader, entityPropertyInfos);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 关联读取子表
        /// </summary>
        /// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        /// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="subConditionExpression">子表的查询条件</param>
        /// <param name="joinConditionExpression">连接条件</param>
        /// <returns></returns>
        public static SubTable ReadSubEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where SubTable : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
                if (mainConditionExpression != null)
                {
                    mainWhereEntity.Where(mainConditionExpression);
                }
                GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
                if (subConditionExpression != null)
                {
                    subWhereEntity.Where(subConditionExpression);
                }
                GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
                joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

                return ReadSubEntity(joinEntity, db);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 关联读取子表
        /// </summary>
        /// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        /// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="subConditionExpression">子表的查询条件</param>
        /// <param name="joinConditionExpression">连接条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns></returns>
        public static SubTable ReadSubEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression,
            Database db) where SubTable : class, new()
        {
            GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
            if (mainConditionExpression != null)
            {
                mainWhereEntity.Where(mainConditionExpression);
            }
            GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
            if (subConditionExpression != null)
            {
                subWhereEntity.Where(subConditionExpression);
            }
            GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
            joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

            return ReadSubEntity(joinEntity, db);
        }

        /// <summary>
        /// 关联读取子表
        /// </summary>
        /// <typeparam name="MainTable"></typeparam>
        /// <typeparam name="SubTable"></typeparam>
        /// <param name="joinEntity"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static SubTable ReadSubEntity<MainTable, SubTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity,
            Database db) where SubTable : class, new()
        {
            SubTable entity = null;
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(joinEntity.EntityToJoin, joinEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(joinEntity.EntityToJoin.EntityType);
            string selectSQL = SqlCreator.GetMemberSelectSql(joinEntity.EntityToJoin.TableName, dbColumnNames, 1);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, joinEntity.EntityToJoin);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                if (reader.Read())
                {
                    entity = EntityInstanceTool.FillOneEntity<SubTable>(reader, entityPropertyInfos);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="detailsConditionExpression">明细表的查询条件</param>
        /// <param name="joinConditionExpression">连接条件</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>明细表实体类的列表</returns>
        public static List<DetailsTable> ReadDetailsEntityList2<MainTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<DetailsTable, bool>> detailsConditionExpression,
            Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
                if (mainConditionExpression != null)
                {
                    mainWhereEntity.Where(mainConditionExpression);
                }
                GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
                if (detailsConditionExpression != null)
                {
                    detailsWhereEntity.Where(detailsConditionExpression);
                }
                GenericJoinEntity<MainTable, DetailsTable> joinEntity = new GenericJoinEntity<MainTable, DetailsTable>();
                joinEntity.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression);

                return ReadDetailsEntityList(joinEntity, db, maxRowCounts);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="detailsConditionExpression">明细表的查询条件</param>
        /// <param name="joinConditionExpression">连接条件</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>明细表实体类的列表</returns>
        public static List<DetailsTable> ReadDetailsEntityList2<MainTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<DetailsTable, bool>> detailsConditionExpression,
            Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression,
            Database db,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
            if (mainConditionExpression != null)
            {
                mainWhereEntity.Where(mainConditionExpression);
            }
            GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
            if (detailsConditionExpression != null)
            {
                detailsWhereEntity.Where(detailsConditionExpression);
            }
            GenericJoinEntity<MainTable, DetailsTable> joinEntity = new GenericJoinEntity<MainTable, DetailsTable>();
            joinEntity.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression);

            return ReadDetailsEntityList(joinEntity, db, maxRowCounts);
        }

        /// <summary>
        /// 读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="joinEntity">连接条件</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>明细表实体类的列表</returns>
        public static List<DetailsTable> ReadDetailsEntityList<MainTable, DetailsTable>(
            GenericJoinEntity<MainTable, DetailsTable> joinEntity,
            Database db,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            List<DetailsTable> entitys = new List<DetailsTable>();
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(joinEntity.MainEntity, joinEntity);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(joinEntity.EntityToJoin.EntityType);
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            string selectSQL = SqlCreator.GetMemberSelectSql(joinEntity.EntityToJoin.TableName, dbColumnNames, rowCount);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                while (reader.Read())
                {
                    var entity = EntityInstanceTool.FillOneEntity<DetailsTable>(reader, entityPropertyInfos);
                    entitys.Add(entity);
                }
                return entitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        #endregion

        #region 三表连接

        /// <summary>
        /// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="SubTable">子表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="subConditionExpression">子表的查询条件</param>
        /// <param name="detailsConditionExpression">明细表的查询条件</param>
        /// <param name="joinConditionExpression0">主表和子表的连接条件</param>
        /// <param name="joinConditionExpression1">主表和明细表的连接条件</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns></returns>
        public static List<DetailsTable> ReadDetailsEntityList2<MainTable, SubTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<DetailsTable, bool>> detailsConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression0,
            Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression1,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
                if (mainConditionExpression != null)
                {
                    mainWhereEntity.Where(mainConditionExpression);
                }
                GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
                if (subConditionExpression != null)
                {
                    subWhereEntity.Where(subConditionExpression);
                }
                GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
                if (detailsConditionExpression != null)
                {
                    detailsWhereEntity.Where(detailsConditionExpression);
                }
                GenericJoinEntity<MainTable, SubTable> joinEntity0 = new GenericJoinEntity<MainTable, SubTable>();
                joinEntity0.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression0);
                GenericJoinEntity<MainTable, DetailsTable> joinEntity1 = new GenericJoinEntity<MainTable, DetailsTable>();
                joinEntity1.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression1);

                return ReadDetailsEntityList(joinEntity0, joinEntity1, db, maxRowCounts);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="SubTable">子表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="mainConditionExpression">主表的查询条件</param>
        /// <param name="subConditionExpression">子表的查询条件</param>
        /// <param name="detailsConditionExpression">明细表的查询条件</param>
        /// <param name="joinConditionExpression0">主表和子表的连接条件</param>
        /// <param name="joinConditionExpression1">主表和明细表的连接条件</param>
        /// <param name="db"></param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns></returns>
        public static List<DetailsTable> ReadDetailsEntityList2<MainTable, SubTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<DetailsTable, bool>> detailsConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression0,
            Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression1,
            Database db,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
            if (mainConditionExpression != null)
            {
                mainWhereEntity.Where(mainConditionExpression);
            }
            GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
            if (subConditionExpression != null)
            {
                subWhereEntity.Where(subConditionExpression);
            }
            GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
            if (detailsConditionExpression != null)
            {
                detailsWhereEntity.Where(detailsConditionExpression);
            }
            GenericJoinEntity<MainTable, SubTable> joinEntity0 = new GenericJoinEntity<MainTable, SubTable>();
            joinEntity0.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression0);
            GenericJoinEntity<MainTable, DetailsTable> joinEntity1 = new GenericJoinEntity<MainTable, DetailsTable>();
            joinEntity1.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression1);

            return ReadDetailsEntityList(joinEntity0, joinEntity1, db, maxRowCounts);
        }

        /// <summary>
        /// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="SubTable">子表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="joinEntity0">连接条件(主表-子表)</param>
        /// <param name="joinEntity1">连接条件(主表-明细表)</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>明细表实体类的列表</returns>
        public static List<DetailsTable> ReadDetailsEntityList<MainTable, SubTable, DetailsTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity0,
            GenericJoinEntity<MainTable, DetailsTable> joinEntity1,
            Database db,
            params int[] maxRowCounts) where DetailsTable : class, new()
        {
            List<DetailsTable> entitys = new List<DetailsTable>();
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(joinEntity0.MainEntity, joinEntity0, joinEntity1);
            List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity1.EntityToJoin.EntityType);
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(joinEntity1.EntityToJoin.EntityType);
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            string selectSQL = SqlCreator.GetMemberSelectSql(joinEntity1.EntityToJoin.TableName, dbColumnNames, rowCount);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, joinEntity0.MainEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                while (reader.Read())
                {
                    var entity = EntityInstanceTool.FillOneEntity<DetailsTable>(reader, entityPropertyInfos);
                    entitys.Add(entity);
                }
                return entitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        #endregion

        #endregion

        #region 成员选择

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="conditionExpression"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static TKey SelectSingleMember2<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db)
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }
            return SelectSingleMember(whereEntity, keySelector, db);
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="conditionExpression"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<TKey> SelectSingleMemberList2<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db)
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }
            return SelectSingleMemberList(whereEntity, keySelector, db);
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereEntity"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static TKey SelectSingleMember<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db)
        {
            Type entityType = typeof(T);
            MemberExpression m = keySelector.Body as MemberExpression;
            string memberName = m.Member.Name;
            List<string> dbColumnNames = new List<string>(1);
            dbColumnNames.Add(DbTableMapping.GetDbColumnName(entityType, memberName));
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            string selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, dbColumnNames, 1);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            try
            {
                var tmpObj = db.ExecuteScalar(cmd);
                return (TKey)tmpObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereEntity"></param>
        /// <param name="keySelector"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<TKey> SelectSingleMemberList<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db)
        {
            Type entityType = typeof(T);
            MemberExpression m = keySelector.Body as MemberExpression;
            string memberName = m.Member.Name;
            List<string> dbColumnNames = new List<string>(1);
            dbColumnNames.Add(DbTableMapping.GetDbColumnName(entityType, memberName));
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            string selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, dbColumnNames, 1);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);
            IDataReader reader = null;
            List<TKey> retList = new List<TKey>();
            try
            {
                reader = db.ExecuteReader(cmd);
                while (reader.Read())
                {
                    if (reader.IsDBNull(0))
                        continue;
                    retList.Add((TKey)reader.GetValue(0));
                }
                return retList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询指定的成员,并以DataTable的形式返回
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="memberExpression">要查询的成员</param>
        /// <param name="maxRowCounts">要返回记录的数量</param>
        /// <returns>查询得到的成员</returns>
        public static DataTable SelectMembers2<T, TResult>(Expression<Func<T, bool>> conditionExpression, Expression<VisitMember<T, TResult>> memberExpression, params int[] maxRowCounts)
        {
            Database db = null;
            try
            {
                db = DatabaseFactory.CreateDatabase();
                GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
                if (conditionExpression != null)
                {
                    whereEntity.Where(conditionExpression);
                }
                return whereEntity.SelectMembers(memberExpression, db, maxRowCounts);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
            }
        }

        /// <summary>
        /// 查询指定的成员,并以DataTable的形式返回
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="memberExpression">要查询的成员</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">要返回记录的数量</param>
        /// <returns>查询得到的成员</returns>
        public static DataTable SelectMembers2<T, TResult>(Expression<Func<T, bool>> conditionExpression, Expression<VisitMember<T, TResult>> memberExpression, Database db, params int[] maxRowCounts)
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
            {
                whereEntity.Where(conditionExpression);
            }

            return whereEntity.SelectMembers(memberExpression, db, maxRowCounts);
        }

        /// <summary>
        /// 查询指定的成员,并以DataTable的形式返回
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="memberExpression">要查询的成员</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">要返回记录的数量</param>
        /// <returns>查询得到的成员</returns>
        public static DataTable SelectMembers<T, TResult>(this GenericWhereEntity<T> whereEntity, Expression<VisitMember<T, TResult>> memberExpression, Database db, params int[] maxRowCounts)
        {
            if (memberExpression == null || memberExpression.Body == null)
            {
                throw new EntityExpressionsException("必须指定要查询的成员!");
            }

            if (!(memberExpression.Body is NewExpression) && !(memberExpression.Body is MemberExpression))
            {
                throw new EntityExpressionsException("指定要查询的成员无效!");
            }

            Type entityType = typeof(T);
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(whereEntity);
            string selectSQL = null;
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            if (memberExpression.Body is NewExpression)
            {
                selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, entityType, memberExpression.Body as NewExpression, rowCount);
            }
            else
            {
                selectSQL = SqlCreator.GetMemberSelectSql(whereEntity.TableName, entityType, memberExpression.Body as MemberExpression, rowCount);
            }

            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, whereEntity);

            try
            {
                //DateTime t0 = DateTime.Now;
                DataTable dt = db.ExecuteDataTable(cmd);
                //DateTime t1 = DateTime.Now;
                //var ts = t1 - t0;
                //Console.WriteLine("数据库查询耗时：" + ts.TotalMilliseconds);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        #endregion

        #region 连接查询(返回多表成员)

        /// <summary>
        /// 连接查询(返回双表成员)
        /// </summary>
        /// <typeparam name="MainTable"></typeparam>
        /// <typeparam name="SubTable"></typeparam>
        /// <param name="joinEntity"></param>
        /// <param name="db"></param>
        /// <param name="maxRowCounts"></param>
        /// <returns></returns>
        public static List<GenericPairEntity<MainTable, SubTable>> JoinReadEntitys<MainTable, SubTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity,
            Database db,
            params int[] maxRowCounts)
            where MainTable : class, new()
            where SubTable : class, new()
        {
            List<GenericPairEntity<MainTable, SubTable>> pairsList = new List<GenericPairEntity<MainTable, SubTable>>();
            //构造查询条件
            string whereSQL = BuildDbQuerysGeneric.BuildCondition(joinEntity.MainEntity, joinEntity);
            List<string> dbColumnNamesA = DbTableMapping.GetDbColumnNames(joinEntity.MainEntity.EntityType);
            List<PropertyInfo> entityPropertysA = DbTableMapping.GetEntityPropertyInfos(joinEntity.MainEntity.EntityType);
            List<string> dbColumnNamesB = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
            List<PropertyInfo> entityPropertysB = DbTableMapping.GetEntityPropertyInfos(joinEntity.EntityToJoin.EntityType);
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            string selectSQL = SqlCreator.GetJoinMemberSelectSql(joinEntity.MainEntity.TableName, dbColumnNamesA,
                joinEntity.EntityToJoin.TableName, dbColumnNamesB,
                rowCount);
            var cmd = db.GetSqlStringCommand(selectSQL + whereSQL);
            SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(cmd);
                while (reader.Read())
                {
                    var pair = EntityInstanceTool.FillOneEntityPair<MainTable, SubTable>(reader, entityPropertysA, entityPropertysB);
                    pairsList.Add(pair);
                }
                return pairsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                cmd.Dispose();
            }
        }

        #endregion

        #region 特殊操作

        /// <summary>
        /// 从DataReader中加载实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T LoadEntityFromReader<T>(IDataReader reader) where T : class, new()
        {
            if (!reader.Read())
            {
                return null;
            }
            Type entityType = typeof(T);
            List<string> dbColumns = new List<string>(reader.FieldCount);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                dbColumns.Add(reader.GetName(i));
            }
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType, dbColumns);

            T entity = new T();
            for (int i = 0; i < entityPropertyInfos.Count; i++)
            {
                if (reader.IsDBNull(i) || entityPropertyInfos[i] == null)
                {
                    continue;
                }
                entityPropertyInfos[i].SetValue(entity, reader.GetValue(i), null);
            }
            return entity;
        }

        /// <summary>
        /// 从DataReader中加载实体类的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> LoadEntityListFromReader<T>(IDataReader reader) where T : class, new()
        {
            Type entityType = typeof(T);
            List<string> dbColumns = new List<string>(reader.FieldCount);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                dbColumns.Add(reader.GetName(i));
            }
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos(entityType, dbColumns);

            List<T> entityList = new List<T>();
            while (reader.Read())
            {
                T entity = new T();
                for (int i = 0; i < entityPropertyInfos.Count; i++)
                {
                    if (reader.IsDBNull(i) || entityPropertyInfos[i] == null)
                    {
                        continue;
                    }
                    entityPropertyInfos[i].SetValue(entity, reader.GetValue(i), null);
                }
                entityList.Add(entity);
            }
            return entityList;
        }

        #endregion
    }
}