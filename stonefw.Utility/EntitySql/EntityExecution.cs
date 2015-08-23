using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using stonefw.Utility.EntitySql.Entity;


namespace stonefw.Utility.EntitySql
{
    /// <summary>
    /// 实体类的操作
    /// </summary>
    public static class EntityExecution
    {
        #region 成员委托
        /// <summary>
        /// 访问成员的委托
        /// </summary>
        public delegate TResult VisitMember<T, TResult>(T arg);
        #endregion        

        #region 新增操作

        /// <summary>
        /// 插入实体
        /// </summary>
        public static void Insert<T>(this T entity, Database db = null)
            where T : BaseEntity
        {
            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateInsertCommand(db, entity))
            {
                int rowCount = db.ExecuteNonQuery(cmd);
                if (rowCount <= 0) throw new EntitySqlException("新增记录失败(DB)！");
            }
        }

        /// <summary>
        /// 插入实体，并返回标识列的值
        /// </summary>
        public static object InsertWithIdentity<T>(this T entity, Database db = null)
            where T : BaseEntity
        {
            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateInsertCommandWithIdentity(db, entity))
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
        public static int Delete<T>(Expression<Func<T, bool>> conditionExpression, Database db = null)
            where T : BaseEntity
        {
            if (conditionExpression == null)
                throw new EntitySqlException("删除记录时，必须指定Where条件，否则将导致整个表的数据被删除！");

            if (db == null) db = DatabaseFactory.CreateDatabase();
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            whereEntity.Where(conditionExpression);
            return Delete(whereEntity, db);
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        public static int Delete<T>(GenericWhereEntity<T> whereEntity, Database db = null)
            where T : BaseEntity
        {
            //DELETE时应该禁用别名
            whereEntity.DisableTableAlias = true;

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreatDeleteCommand(db, whereEntity))
            {
                return db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        public static int Delete<T>(this T entity, Database db = null)
            where T : BaseEntity
        {
            //获取主键相关的字段
            Type entityType = typeof(T);
            if (!EntityMappingTool.HasPrimaryKey(entityType))
                throw new EntitySqlException(string.Format("实体类{0}未设置主键字段！", entityType.FullName));

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreatDeleteCommand(db, entity))
            {
                return db.ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region 修改操作

        /// <summary>
        /// 更新实体类
        /// </summary>
        public static void Update<T>(this T entity, Database db = null)
            where T : BaseEntity
        {
            //检索数据库相关的字段和属性
            Type entityType = typeof(T);

            //获取主键相关的字段
            if (!EntityMappingTool.HasPrimaryKey(entityType))
                throw new EntitySqlException(string.Format("实体类{0}未设置主键字段！", entityType.FullName));

            if (!EntityInstanceTool.HasPrimaryKeyValue(entity))
                throw new EntitySqlException(string.Format("未给实体类{0}实例的主键字段赋值！", entityType.FullName));

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateUpdateCommand(db, entity))
            {
                int rowCount = db.ExecuteNonQuery(cmd);
                if (rowCount <= 0) throw new EntitySqlException("修改记录失败(DB)！");
            }
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        public static void Update<T>(this T entity, Expression<Func<T, bool>> conditionExpression, Database db = null)
            where T : BaseEntity
        {
            if (conditionExpression == null)
                throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            Update(entity, whereEntity, db);
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        public static void Update<T>(this T entity, GenericWhereEntity<T> whereEntity, Database db = null)
            where T : BaseEntity
        {
            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateUpdateCommand(db, entity, whereEntity))
            {
                db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 将指定的字段设置为Null
        /// </summary>
        public static void SetMemberNull<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db = null)
            where T : BaseEntity
        {
            if (conditionExpression == null)
                throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            SetMemberNull(whereEntity, keySelector, db);
        }

        /// <summary>
        /// 将指定的字段设置为Null
        /// </summary>
        public static void SetMembersNull<T>(Expression<Func<T, bool>> conditionExpression, Database db = null, params Expression<Func<T, object>>[] keySelectors)
            where T : BaseEntity
        {
            if (conditionExpression == null)
                throw new EntitySqlException("修改记录时，必须指定Where条件，否则将导致整个表的数据被修改！");

            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            SetMembersNull(whereEntity, db, keySelectors);
        }

        /// <summary>
        /// 将指定的字段设置为Null
        /// </summary>
        public static void SetMemberNull<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db = null)
            where T : BaseEntity
        {
            Type entityType = typeof(T);
            MemberExpression m = keySelector.Body as MemberExpression;
            string memberName = m.Member.Name;

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateUpdateMemberToNullCommand(db, whereEntity, memberName))
            {
                db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 将指定的字段设置为Null
        /// </summary>
        public static void SetMembersNull<T>(GenericWhereEntity<T> whereEntity, Database db = null, params Expression<Func<T, object>>[] keySelectors)
            where T : BaseEntity
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

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (DbCommand cmd = SqlCreator.CreateUpdateMemberToNullCommand(db, whereEntity, memberNames))
            {
                db.ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region 查询操作

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="T">实体的类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="db">数据库连接</param>
        /// <returns></returns>
        public static T SelectOne<T>(Expression<Func<T, bool>> conditionExpression, Database db = null)
            where T : BaseEntity, new()
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            return SelectOne(whereEntity, db);
        }

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>实体类的实例</returns>
        public static T SelectOne<T>(GenericWhereEntity<T> whereEntity, Database db = null)
            where T : BaseEntity, new()
        {
            string selectSql = SqlCreator.CreateSelectSql<T>(whereEntity, 1);
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
            {
                SqlCreator.FillSqlParameters(db, cmd, whereEntity);
                using (var reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                        return EntityInstanceTool.FillOneEntity<T>(reader);
                    return null;
                }
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
        public static List<T> SelectAll<T>(Expression<Func<T, bool>> conditionExpression = null, Database db = null, params int[] maxRowCounts)
            where T : BaseEntity, new()
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            return SelectAll(whereEntity, db, maxRowCounts);
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<T> SelectAll<T>(GenericWhereEntity<T> whereEntity, Database db = null, params int[] maxRowCounts)
            where T : BaseEntity, new()
        {
            int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
            string selectSql = SqlCreator.CreateSelectSql<T>(whereEntity, rowCount);
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
            {
                SqlCreator.FillSqlParameters(db, cmd, whereEntity);
                using (var reader = db.ExecuteReader(cmd))
                {
                    List<T> entitys = new List<T>();
                    while (reader.Read())
                    {
                        entitys.Add(EntityInstanceTool.FillOneEntity<T>(reader));
                    }
                    return entitys;
                }
            }
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int Count<T>(Expression<Func<T, bool>> conditionExpression, Database db = null)
            where T : BaseEntity
        {
            GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
            if (conditionExpression != null)
                whereEntity.Where(conditionExpression);

            return Count(whereEntity, db);
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="T">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="db">数据库连接</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int Count<T>(GenericWhereEntity<T> whereEntity, Database db = null)
            where T : BaseEntity
        {
            string whereSql = SqlCreator.CreateWhereSql(whereEntity);
            string selectSql = "SELECT COUNT(*) ";

            if (db == null) db = DatabaseFactory.CreateDatabase();
            using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
            {
                SqlCreator.FillSqlParameters(db, cmd, whereEntity);
                var countObj = db.ExecuteScalar(cmd);
                return (int)countObj;
            }
        }

        ///// <summary>
        ///// 查询是否存在指定条件的记录
        ///// </summary>
        ///// <typeparam name="T">实体类别</typeparam>
        ///// <param name="conditionExpression">查询条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <returns>是否存在指定条件的记录</returns>
        //public static bool ExistsRecord2<T>(Expression<Func<T, bool>> conditionExpression, Database db = null)
        //{
        //    GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
        //    if (conditionExpression != null)            
        //        whereEntity.Where(conditionExpression);            

        //    return ExistsRecord(whereEntity, db);
        //}

        ///// <summary>
        ///// 查询是否存在指定条件的记录
        ///// </summary>
        ///// <typeparam name="T">实体类别</typeparam>
        ///// <param name="whereEntity">查询条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <returns>是否存在指定条件的记录</returns>
        //public static bool ExistsRecord<T>(GenericWhereEntity<T> whereEntity, Database db = null)
        //{
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(whereEntity);
        //    string selectSql = "SELECT 1 WHERE EXISTS (SELECT 1";
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql + ")");
        //    SqlCreator.FillSqlParameters(db, cmd, whereEntity);
        //    try
        //    {
        //        var countObj = db.ExecuteScalar(cmd);
        //        if (countObj == null || countObj is DBNull)
        //            return false;
        //        return ((int)countObj > 0);
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //    }
        //}

        #endregion


        #region 暂时不用的方法       

        #region 成员选择

        ///// <summary>
        ///// 检索指定的字段
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="conditionExpression"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static TKey SelectSingleMember<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db = null)
        //    where T : BaseEntity
        //{
        //    GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
        //    if (conditionExpression != null)
        //        whereEntity.Where(conditionExpression);

        //    return SelectSingleMember(whereEntity, keySelector, db);
        //}

        ///// <summary>
        ///// 检索指定的字段
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="whereEntity"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static TKey SelectSingleMember<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db = null)
        //    where T : BaseEntity
        //{
        //    MemberExpression m = keySelector.Body as MemberExpression;
        //    List<string> dbColumnNames = new List<string>(1) { EntityMappingTool.GetDbColumnName(typeof(T), m.Member.Name) };
        //    string whereSql = SqlCreator.CreateWhereSql(whereEntity);
        //    string selectSql = SqlCreator.CreateSelectSql<T>(dbColumnNames, 1);

        //    if (db == null) db = DatabaseFactory.CreateDatabase();
        //    using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
        //    {
        //        SqlCreator.FillSqlParameters(db, cmd, whereEntity);
        //        var tmpObj = db.ExecuteScalar(cmd);
        //        return (TKey)tmpObj;
        //    }
        //}

        ///// <summary>
        ///// 检索指定的字段
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="conditionExpression"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static List<TKey> SelectSingleMemberList<T, TKey>(Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TKey>> keySelector, Database db = null)
        //    where T : BaseEntity
        //{
        //    GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
        //    if (conditionExpression != null)
        //        whereEntity.Where(conditionExpression);

        //    return SelectSingleMemberList(whereEntity, keySelector, db);
        //}

        ///// <summary>
        ///// 检索指定的字段
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="whereEntity"></param>
        ///// <param name="keySelector"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static List<TKey> SelectSingleMemberList<T, TKey>(GenericWhereEntity<T> whereEntity, Expression<Func<T, TKey>> keySelector, Database db = null)
        //    where T : BaseEntity
        //{
        //    MemberExpression m = keySelector.Body as MemberExpression;
        //    List<string> dbColumnNames = new List<string>(1) { EntityMappingTool.GetDbColumnName(typeof(T), m.Member.Name) };
        //    string whereSql = SqlCreator.CreateWhereSql(whereEntity);
        //    string selectSql = SqlCreator.CreateSelectSql<T>(dbColumnNames, 1);

        //    if (db == null) db = DatabaseFactory.CreateDatabase();
        //    using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
        //    {
        //        SqlCreator.FillSqlParameters(db, cmd, whereEntity);
        //        using (var reader = db.ExecuteReader(cmd))
        //        {
        //            List<TKey> retList = new List<TKey>();
        //            while (reader.Read())
        //            {
        //                if (reader.IsDBNull(0))
        //                    continue;
        //                retList.Add((TKey)reader.GetValue(0));
        //            }
        //            return retList;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 查询指定的成员,并以DataTable的形式返回
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="conditionExpression">查询条件的表达式</param>
        ///// <param name="memberExpression">要查询的成员</param>
        ///// <param name="db">数据库连接</param>
        ///// <param name="maxRowCounts">要返回记录的数量</param>
        ///// <returns>查询得到的成员</returns>
        //public static DataTable SelectMembers<T, TResult>(Expression<Func<T, bool>> conditionExpression, Expression<VisitMember<T, TResult>> memberExpression, Database db = null, params int[] maxRowCounts)
        //    where T : BaseEntity
        //{
        //    GenericWhereEntity<T> whereEntity = new GenericWhereEntity<T>();
        //    if (conditionExpression != null)
        //        whereEntity.Where(conditionExpression);

        //    return whereEntity.SelectMembers(memberExpression, db, maxRowCounts);
        //}

        ///// <summary>
        ///// 查询指定的成员,并以DataTable的形式返回
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="whereEntity">查询条件</param>
        ///// <param name="memberExpression">要查询的成员</param>
        ///// <param name="db">数据库连接</param>
        ///// <param name="maxRowCounts">要返回记录的数量</param>
        ///// <returns>查询得到的成员</returns>
        //public static DataTable SelectMembers<T, TResult>(this GenericWhereEntity<T> whereEntity, Expression<VisitMember<T, TResult>> memberExpression, Database db = null, params int[] maxRowCounts)
        //    where T : BaseEntity
        //{
        //    if (memberExpression == null || memberExpression.Body == null)
        //        throw new EntitySqlException("必须指定要查询的成员!");

        //    if (!(memberExpression.Body is NewExpression) && !(memberExpression.Body is MemberExpression))
        //        throw new EntitySqlException("指定要查询的成员无效!");

        //    Type entityType = typeof(T);
        //    string whereSql = SqlCreator.CreateWhereSql(whereEntity);
        //    string selectSql = null;
        //    int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
        //    if (memberExpression.Body is NewExpression)
        //        selectSql = SqlCreator.CreateSelectSql<T>(memberExpression.Body as NewExpression, rowCount);
        //    else
        //        selectSql = SqlCreator.CreateSelectSql<T>(memberExpression.Body as MemberExpression, rowCount);

        //    if (db == null) db = DatabaseFactory.CreateDatabase();
        //    using (var cmd = db.GetSqlStringCommand(selectSql + whereSql))
        //    {
        //        SqlCreator.FillSqlParameters(db, cmd, whereEntity);
        //        return db.ExecuteDataTable(cmd);
        //    }
        //}

        #endregion 

        #region 双表连接

        ///// <summary>
        ///// 关联读取主表
        ///// </summary>
        ///// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        ///// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        ///// <param name="joinEntity">连接条件</param>
        ///// <returns></returns>
        //public static MainTable ReadMainEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where MainTable : class, new()
        //{
        //    Database db = null;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase();
        //        GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //        if (mainConditionExpression != null)
        //        {
        //            mainWhereEntity.Where(mainConditionExpression);
        //        }
        //        GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //        if (subConditionExpression != null)
        //        {
        //            subWhereEntity.Where(subConditionExpression);
        //        }
        //        GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
        //        joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

        //        return ReadMainEntity(joinEntity, db);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //}

        ///// <summary>
        ///// 关联读取主表
        ///// </summary>
        ///// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        ///// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        ///// <param name="joinEntity">连接条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <returns></returns>
        //public static MainTable ReadMainEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression,
        //    Database db) where MainTable : class, new()
        //{
        //    GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //    if (mainConditionExpression != null)
        //    {
        //        mainWhereEntity.Where(mainConditionExpression);
        //    }
        //    GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //    if (subConditionExpression != null)
        //    {
        //        subWhereEntity.Where(subConditionExpression);
        //    }
        //    GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
        //    joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

        //    return ReadMainEntity(joinEntity, db);
        //}

        ///// <summary>
        ///// 关联读取主表
        ///// </summary>
        ///// <typeparam name="MainTable"></typeparam>
        ///// <typeparam name="SubTable"></typeparam>
        ///// <param name="joinEntity"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static MainTable ReadMainEntity<MainTable, SubTable>(
        //    GenericJoinEntity<MainTable, SubTable> joinEntity,
        //    Database db) where MainTable : class, new()
        //{
        //    MainTable entity = null;
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(joinEntity.MainEntity, joinEntity);
        //    List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.MainEntity.EntityType);

        //    string selectSql = SqlCreator.CreateSelectSql(joinEntity.MainEntity.TableName, dbColumnNames, 1);
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql);
        //    SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
        //    IDataReader reader = null;
        //    try
        //    {
        //        reader = db.ExecuteReader(cmd);
        //        if (reader.Read())
        //        {
        //            entity = EntityInstanceTool.FillOneEntity<MainTable>(reader);
        //        }
        //        return entity;
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        cmd.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 关联读取子表
        ///// </summary>
        ///// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        ///// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="subConditionExpression">子表的查询条件</param>
        ///// <param name="joinConditionExpression">连接条件</param>
        ///// <returns></returns>
        //public static SubTable ReadSubEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where SubTable : class, new()
        //{
        //    Database db = null;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase();
        //        GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //        if (mainConditionExpression != null)
        //        {
        //            mainWhereEntity.Where(mainConditionExpression);
        //        }
        //        GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //        if (subConditionExpression != null)
        //        {
        //            subWhereEntity.Where(subConditionExpression);
        //        }
        //        GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
        //        joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

        //        return ReadSubEntity(joinEntity, db);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //}

        ///// <summary>
        ///// 关联读取子表
        ///// </summary>
        ///// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        ///// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="subConditionExpression">子表的查询条件</param>
        ///// <param name="joinConditionExpression">连接条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <returns></returns>
        //public static SubTable ReadSubEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression,
        //    Database db) where SubTable : class, new()
        //{
        //    GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //    if (mainConditionExpression != null)
        //    {
        //        mainWhereEntity.Where(mainConditionExpression);
        //    }
        //    GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //    if (subConditionExpression != null)
        //    {
        //        subWhereEntity.Where(subConditionExpression);
        //    }
        //    GenericJoinEntity<MainTable, SubTable> joinEntity = new GenericJoinEntity<MainTable, SubTable>();
        //    joinEntity.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression);

        //    return ReadSubEntity(joinEntity, db);
        //}

        ///// <summary>
        ///// 关联读取子表
        ///// </summary>
        ///// <typeparam name="MainTable"></typeparam>
        ///// <typeparam name="SubTable"></typeparam>
        ///// <param name="joinEntity"></param>
        ///// <param name="db"></param>
        ///// <returns></returns>
        //public static SubTable ReadSubEntity<MainTable, SubTable>(
        //    GenericJoinEntity<MainTable, SubTable> joinEntity,
        //    Database db) where SubTable : class, new()
        //{
        //    SubTable entity = null;
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(joinEntity.EntityToJoin, joinEntity);
        //    List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
        //    string selectSql = SqlCreator.CreateSelectSql(joinEntity.EntityToJoin.TableName, dbColumnNames, 1);
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql);
        //    SqlCreator.FillSqlParameters(db, cmd, joinEntity.EntityToJoin);
        //    IDataReader reader = null;
        //    try
        //    {
        //        reader = db.ExecuteReader(cmd);
        //        if (reader.Read())
        //        {
        //            entity = EntityInstanceTool.FillOneEntity<SubTable>(reader);
        //        }
        //        return entity;
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        cmd.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="detailsConditionExpression">明细表的查询条件</param>
        ///// <param name="joinConditionExpression">连接条件</param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns>明细表实体类的列表</returns>
        //public static List<DetailsTable> ReadDetailsEntityList2<MainTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<DetailsTable, bool>> detailsConditionExpression,
        //    Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    Database db = null;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase();
        //        GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //        if (mainConditionExpression != null)
        //        {
        //            mainWhereEntity.Where(mainConditionExpression);
        //        }
        //        GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
        //        if (detailsConditionExpression != null)
        //        {
        //            detailsWhereEntity.Where(detailsConditionExpression);
        //        }
        //        GenericJoinEntity<MainTable, DetailsTable> joinEntity = new GenericJoinEntity<MainTable, DetailsTable>();
        //        joinEntity.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression);

        //        return ReadDetailsEntityList(joinEntity, db, maxRowCounts);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //}

        ///// <summary>
        ///// 读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="detailsConditionExpression">明细表的查询条件</param>
        ///// <param name="joinConditionExpression">连接条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns>明细表实体类的列表</returns>
        //public static List<DetailsTable> ReadDetailsEntityList2<MainTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<DetailsTable, bool>> detailsConditionExpression,
        //    Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression,
        //    Database db,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //    if (mainConditionExpression != null)
        //    {
        //        mainWhereEntity.Where(mainConditionExpression);
        //    }
        //    GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
        //    if (detailsConditionExpression != null)
        //    {
        //        detailsWhereEntity.Where(detailsConditionExpression);
        //    }
        //    GenericJoinEntity<MainTable, DetailsTable> joinEntity = new GenericJoinEntity<MainTable, DetailsTable>();
        //    joinEntity.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression);

        //    return ReadDetailsEntityList(joinEntity, db, maxRowCounts);
        //}

        ///// <summary>
        ///// 读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="joinEntity">连接条件</param>
        ///// <param name="db">数据库连接</param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns>明细表实体类的列表</returns>
        //public static List<DetailsTable> ReadDetailsEntityList<MainTable, DetailsTable>(
        //    GenericJoinEntity<MainTable, DetailsTable> joinEntity,
        //    Database db,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    List<DetailsTable> entitys = new List<DetailsTable>();
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(joinEntity.MainEntity, joinEntity);
        //    List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
        //    int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
        //    string selectSql = SqlCreator.CreateSelectSql(joinEntity.EntityToJoin.TableName, dbColumnNames, rowCount);
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql);
        //    SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
        //    IDataReader reader = null;
        //    try
        //    {
        //        reader = db.ExecuteReader(cmd);
        //        while (reader.Read())
        //        {
        //            var entity = EntityInstanceTool.FillOneEntity<DetailsTable>(reader);
        //            entitys.Add(entity);
        //        }
        //        return entitys;
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        cmd.Dispose();
        //    }
        //}

        #endregion

        #region 三表连接

        ///// <summary>
        ///// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="SubTable">子表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="subConditionExpression">子表的查询条件</param>
        ///// <param name="detailsConditionExpression">明细表的查询条件</param>
        ///// <param name="joinConditionExpression0">主表和子表的连接条件</param>
        ///// <param name="joinConditionExpression1">主表和明细表的连接条件</param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns></returns>
        //public static List<DetailsTable> ReadDetailsEntityList2<MainTable, SubTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<DetailsTable, bool>> detailsConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression0,
        //    Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression1,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    Database db = null;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase();
        //        GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //        if (mainConditionExpression != null)
        //        {
        //            mainWhereEntity.Where(mainConditionExpression);
        //        }
        //        GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //        if (subConditionExpression != null)
        //        {
        //            subWhereEntity.Where(subConditionExpression);
        //        }
        //        GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
        //        if (detailsConditionExpression != null)
        //        {
        //            detailsWhereEntity.Where(detailsConditionExpression);
        //        }
        //        GenericJoinEntity<MainTable, SubTable> joinEntity0 = new GenericJoinEntity<MainTable, SubTable>();
        //        joinEntity0.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression0);
        //        GenericJoinEntity<MainTable, DetailsTable> joinEntity1 = new GenericJoinEntity<MainTable, DetailsTable>();
        //        joinEntity1.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression1);

        //        return ReadDetailsEntityList(joinEntity0, joinEntity1, db, maxRowCounts);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //}

        ///// <summary>
        ///// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="SubTable">子表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="mainConditionExpression">主表的查询条件</param>
        ///// <param name="subConditionExpression">子表的查询条件</param>
        ///// <param name="detailsConditionExpression">明细表的查询条件</param>
        ///// <param name="joinConditionExpression0">主表和子表的连接条件</param>
        ///// <param name="joinConditionExpression1">主表和明细表的连接条件</param>
        ///// <param name="db"></param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns></returns>
        //public static List<DetailsTable> ReadDetailsEntityList2<MainTable, SubTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
        //    Expression<Func<SubTable, bool>> subConditionExpression,
        //    Expression<Func<DetailsTable, bool>> detailsConditionExpression,
        //    Expression<Func<MainTable, SubTable, bool>> joinConditionExpression0,
        //    Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression1,
        //    Database db,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    GenericWhereEntity<MainTable> mainWhereEntity = new GenericWhereEntity<MainTable>();
        //    if (mainConditionExpression != null)
        //    {
        //        mainWhereEntity.Where(mainConditionExpression);
        //    }
        //    GenericWhereEntity<SubTable> subWhereEntity = new GenericWhereEntity<SubTable>();
        //    if (subConditionExpression != null)
        //    {
        //        subWhereEntity.Where(subConditionExpression);
        //    }
        //    GenericWhereEntity<DetailsTable> detailsWhereEntity = new GenericWhereEntity<DetailsTable>();
        //    if (detailsConditionExpression != null)
        //    {
        //        detailsWhereEntity.Where(detailsConditionExpression);
        //    }
        //    GenericJoinEntity<MainTable, SubTable> joinEntity0 = new GenericJoinEntity<MainTable, SubTable>();
        //    joinEntity0.InnerJoin(mainWhereEntity, subWhereEntity, joinConditionExpression0);
        //    GenericJoinEntity<MainTable, DetailsTable> joinEntity1 = new GenericJoinEntity<MainTable, DetailsTable>();
        //    joinEntity1.InnerJoin(mainWhereEntity, detailsWhereEntity, joinConditionExpression1);

        //    return ReadDetailsEntityList(joinEntity0, joinEntity1, db, maxRowCounts);
        //}

        ///// <summary>
        ///// 通过关联主表-子表-明细表读取一个明细表的实体类的列表
        ///// </summary>
        ///// <typeparam name="MainTable">主表的类型</typeparam>
        ///// <typeparam name="SubTable">子表的类型</typeparam>
        ///// <typeparam name="DetailsTable">明细表的类型</typeparam>
        ///// <param name="joinEntity0">连接条件(主表-子表)</param>
        ///// <param name="joinEntity1">连接条件(主表-明细表)</param>
        ///// <param name="db">数据库连接</param>
        ///// <param name="maxRowCounts">读取的记录数量</param>
        ///// <returns>明细表实体类的列表</returns>
        //public static List<DetailsTable> ReadDetailsEntityList<MainTable, SubTable, DetailsTable>(
        //    GenericJoinEntity<MainTable, SubTable> joinEntity0,
        //    GenericJoinEntity<MainTable, DetailsTable> joinEntity1,
        //    Database db,
        //    params int[] maxRowCounts) where DetailsTable : class, new()
        //{
        //    List<DetailsTable> entitys = new List<DetailsTable>();
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(joinEntity0.MainEntity, joinEntity0, joinEntity1);
        //    List<string> dbColumnNames = DbTableMapping.GetDbColumnNames(joinEntity1.EntityToJoin.EntityType);
        //    int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
        //    string selectSql = SqlCreator.CreateSelectSql(joinEntity1.EntityToJoin.TableName, dbColumnNames, rowCount);
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql);
        //    SqlCreator.FillSqlParameters(db, cmd, joinEntity0.MainEntity);
        //    IDataReader reader = null;
        //    try
        //    {
        //        reader = db.ExecuteReader(cmd);
        //        while (reader.Read())
        //        {
        //            var entity = EntityInstanceTool.FillOneEntity<DetailsTable>(reader);
        //            entitys.Add(entity);
        //        }
        //        return entitys;
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        cmd.Dispose();
        //    }
        //}

        #endregion

        #region 连接查询(返回多表成员)

        ///// <summary>
        ///// 连接查询(返回双表成员)
        ///// </summary>
        ///// <typeparam name="MainTable"></typeparam>
        ///// <typeparam name="SubTable"></typeparam>
        ///// <param name="joinEntity"></param>
        ///// <param name="db"></param>
        ///// <param name="maxRowCounts"></param>
        ///// <returns></returns>
        //public static List<GenericPairEntity<MainTable, SubTable>> JoinReadEntitys<MainTable, SubTable>(
        //    GenericJoinEntity<MainTable, SubTable> joinEntity,
        //    Database db,
        //    params int[] maxRowCounts)
        //    where MainTable : class, new()
        //    where SubTable : class, new()
        //{
        //    List<GenericPairEntity<MainTable, SubTable>> pairsList = new List<GenericPairEntity<MainTable, SubTable>>();
        //    //构造查询条件
        //    string whereSql = SqlCreator.CreateWhereSql(joinEntity.MainEntity, joinEntity);
        //    List<string> dbColumnNamesA = DbTableMapping.GetDbColumnNames(joinEntity.MainEntity.EntityType);
        //    List<string> dbColumnNamesB = DbTableMapping.GetDbColumnNames(joinEntity.EntityToJoin.EntityType);
        //    int rowCount = (maxRowCounts == null || maxRowCounts.Length < 1) ? 0 : maxRowCounts[0];
        //    string selectSql = SqlCreator.GetJoinMemberSelectSql(joinEntity.MainEntity.TableName, dbColumnNamesA,
        //        joinEntity.EntityToJoin.TableName, dbColumnNamesB,
        //        rowCount);
        //    var cmd = db.GetSqlStringCommand(selectSql + whereSql);
        //    SqlCreator.FillSqlParameters(db, cmd, joinEntity.MainEntity);
        //    IDataReader reader = null;
        //    try
        //    {
        //        reader = db.ExecuteReader(cmd);
        //        while (reader.Read())
        //        {
        //            var pair = EntityInstanceTool.FillOnePairEntity<MainTable, SubTable>(reader);
        //            pairsList.Add(pair);
        //        }
        //        return pairsList;
        //    }
        //    catch (EntityToSqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        cmd.Dispose();
        //    }
        //}

        #endregion

        #region 特殊操作

        ///// <summary>
        ///// 从DataReader中加载实体类
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public static T LoadEntityFromReader<T>(IDataReader reader) where T : class, new()
        //{
        //    if (!reader.Read())
        //    {
        //        return null;
        //    }
        //    Type entityType = typeof(T);
        //    List<string> dbColumns = new List<string>(reader.FieldCount);
        //    for (int i = 0; i < reader.FieldCount; i++)
        //    {
        //        dbColumns.Add(reader.GetName(i));
        //    }
        //    List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(entityType, dbColumns);

        //    T entity = new T();
        //    for (int i = 0; i < entityPropertyInfos.Count; i++)
        //    {
        //        if (reader.IsDBNull(i) || entityPropertyInfos[i] == null)
        //        {
        //            continue;
        //        }
        //        entityPropertyInfos[i].SetValue(entity, reader.GetValue(i), null);
        //    }
        //    return entity;
        //}

        ///// <summary>
        ///// 从DataReader中加载实体类的列表
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public static List<T> LoadEntityListFromReader<T>(IDataReader reader) where T : class, new()
        //{
        //    Type entityType = typeof(T);
        //    List<string> dbColumns = new List<string>(reader.FieldCount);
        //    for (int i = 0; i < reader.FieldCount; i++)
        //    {
        //        dbColumns.Add(reader.GetName(i));
        //    }
        //    List<PropertyInfo> entityPropertyInfos = EntityMappingTool.GetEntityPropertyInfos(entityType, dbColumns);

        //    List<T> entityList = new List<T>();
        //    while (reader.Read())
        //    {
        //        T entity = new T();
        //        for (int i = 0; i < entityPropertyInfos.Count; i++)
        //        {
        //            if (reader.IsDBNull(i) || entityPropertyInfos[i] == null)
        //            {
        //                continue;
        //            }
        //            entityPropertyInfos[i].SetValue(entity, reader.GetValue(i), null);
        //        }
        //        entityList.Add(entity);
        //    }
        //    return entityList;
        //}

        #endregion

        #endregion
    }
}