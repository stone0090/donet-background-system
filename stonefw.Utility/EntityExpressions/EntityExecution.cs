using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using stonefw.Utility.EntityExpressions.Data;
using stonefw.Utility.EntityExpressions.Entitys;

namespace stonefw.Utility.EntityExpressions
{
    /// <summary>
    /// 实体执行方法
    /// </summary>
    public class EntityExecution
    {
        protected static Database DB
        {
            get
            {
                return DatabaseFactory.CreateDatabase();
            }
        }        

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        public static void InsertEntity<E>(E entity)
        {
            EntityOperations.InsertEntity(entity, DB);
        }

        /// <summary>
        /// 插入实体，并返回标识列的值
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <returns>标识列的值</returns>
        public static object InsertEntityWithIdentity<E>(E entity)
        {
            return EntityOperations.InsertEntityWithIdentity(entity, DB);
        }
        
        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <typeparam name="TKey">要读取的字段类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="keySelector">要读取的字段</param>
        /// <returns>查询得到的字段</returns>
        public static TKey SelectSingleMember2<E, TKey>(Expression<Func<E, bool>> conditionExpression, Expression<Func<E, TKey>> keySelector)
        {
            return EntityOperations.SelectSingleMember2(conditionExpression, keySelector, DB);
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <typeparam name="TKey">要读取的字段类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="keySelector">要读取的字段</param>
        /// <returns>查询得到的字段</returns>
        public static TKey SelectSingleMember<E, TKey>(GenericWhereEntity<E> whereEntity, Expression<Func<E, TKey>> keySelector)
        {
            return EntityOperations.SelectSingleMember(whereEntity, keySelector, DB);
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <typeparam name="TKey">要读取的字段类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="keySelector">要读取的字段</param>
        /// <returns>查询得到的字段列表</returns>
        public static List<TKey> SelectSingleMemberList2<E, TKey>(Expression<Func<E, bool>> conditionExpression, Expression<Func<E, TKey>> keySelector)
        {
            return EntityOperations.SelectSingleMemberList2(conditionExpression, keySelector, DB);
        }

        /// <summary>
        /// 检索指定的字段
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <typeparam name="TKey">要读取的字段类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="keySelector">要读取的字段</param>
        /// <returns>查询得到的字段列表</returns>
        public static List<TKey> SelectSingleMemberList<E, TKey>(GenericWhereEntity<E> whereEntity, Expression<Func<E, TKey>> keySelector)
        {
            return EntityOperations.SelectSingleMemberList(whereEntity, keySelector, DB);
        }

        /// <summary>
        /// 查询指定的成员,并以DataTable的形式返回
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="memberExpression">要查询的成员</param>
        /// <param name="maxRowCounts">要返回记录的数量</param>
        /// <returns>查询得到的成员</returns>
        public static DataTable SelectMembers2<E, TResult>(Expression<Func<E, bool>> conditionExpression, Expression<VisitMember<E, TResult>> memberExpression, params int[] maxRowCounts)
        {
            return EntityOperations.SelectMembers2(conditionExpression, memberExpression, DB, maxRowCounts);
        }

        /// <summary>
        /// 查询指定的成员,并以DataTable的形式返回
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="memberExpression">要查询的成员</param>
        /// <param name="maxRowCounts">要返回记录的数量</param>
        /// <returns>查询得到的成员</returns>
        public static DataTable SelectMembers<E, TResult>(GenericWhereEntity<E> whereEntity, Expression<VisitMember<E, TResult>> memberExpression, params int[] maxRowCounts)
        {
            return EntityOperations.SelectMembers(whereEntity, memberExpression, DB, maxRowCounts);
        }        

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="conditionExpression">删除的条件</param>
        /// <param name="db"></param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity2<E>(Expression<Func<E, bool>> conditionExpression)
        {
            return EntityOperations.DeleteEntity2(conditionExpression, DB);
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity<E>(GenericWhereEntity<E> whereEntity)
        {
            return EntityOperations.DeleteEntity(whereEntity, DB);
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="theEntity">实体类实例</param>
        /// <returns>受影响的行数</returns>
        public static int DeleteEntity<E>(E theEntity)
        {
            return EntityOperations.DeleteEntity(theEntity, DB);
        }        

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="theEntity">实体类实例</param>
        public static void UpdateEntity<E>(E theEntity)
        {
            EntityOperations.UpdateEntity(theEntity, DB);
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="theEntity">实体类实例</param>
        public static void UpdateEntity<E>(GenericWhereEntity<E> whereEntity, E theEntity)
        {
            EntityOperations.UpdateEntity(whereEntity, theEntity, DB);
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="theEntity">实体类实例</param>
        /// <param name="db">数据库连接</param>
        public static void UpdateEntity2<E>(Expression<Func<E, bool>> conditionExpression, E theEntity)
        {
            EntityOperations.UpdateEntity2(conditionExpression, theEntity, DB);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <typeparam name="TKey">要设置为NULL的字段类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="keySelector">要设置为NULL的字段</param>
        public static void SetMemberNull<E, TKey>(GenericWhereEntity<E> whereEntity, Expression<Func<E, TKey>> keySelector)
        {
            EntityOperations.SetMemberNull(whereEntity, keySelector, DB);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <typeparam name="TKey">要设置为NULL的字段类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="keySelector">要设置为NULL的字段</param>
        public static void SetMemberNull2<E, TKey>(Expression<Func<E, bool>> conditionExpression, Expression<Func<E, TKey>> keySelector)
        {
            EntityOperations.SetMemberNull2(conditionExpression, keySelector, DB);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="keySelectors">要设置为NULL的字段</param>
        public static void SetMembersNull<E>(GenericWhereEntity<E> whereEntity, params Expression<Func<E, object>>[] keySelectors)
        {
            EntityOperations.SetMembersNull(whereEntity, DB, keySelectors);
        }

        /// <summary>
        /// 将指定的字段设置为NULL
        /// </summary>
        /// <typeparam name="E">实体类类型</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <param name="keySelectors">要设置为NULL的字段</param>
        public static void SetMembersNull2<E>(Expression<Func<E, bool>> conditionExpression, params Expression<Func<E, object>>[] keySelectors)
        {
            EntityOperations.SetMembersNull2(conditionExpression, DB, keySelectors);
        }

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="E">实体的类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <returns></returns>
        public static E ReadEntity2<E>(Expression<Func<E, bool>> conditionExpression) where E : class,new()
        {
            return EntityOperations.ReadEntity2(conditionExpression, DB);
        }

        /// <summary>
        /// 读取一个实体类的实例
        /// </summary>
        /// <typeparam name="E">实体类的类型</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <returns>实体类的实例</returns>
        public static E ReadEntity<E>(GenericWhereEntity<E> whereEntity) where E : class,new()
        {
            return EntityOperations.ReadEntity(whereEntity, DB);
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="E">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件的表达式</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<E> ReadEntityList2<E>(Expression<Func<E, bool>> conditionExpression, params int[] maxRowCounts) where E : class,new()
        {
            return EntityOperations.ReadEntityList2(conditionExpression, DB, maxRowCounts);
        }

        /// <summary>
        /// 读取一个实体类的列表
        /// </summary>
        /// <typeparam name="E">实体类别</typeparam>
        /// <param name="whereEntity">查询条件</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>实体类的列表</returns>
        public static List<E> ReadEntityList<E>(GenericWhereEntity<E> whereEntity, params int[] maxRowCounts) where E : class,new()
        {
            return EntityOperations.ReadEntityList(whereEntity, DB, maxRowCounts);
        }        

        /// <summary>
        /// 关联读取主表
        /// </summary>
        /// <typeparam name="MainTable">主表对应实体类类型</typeparam>
        /// <typeparam name="SubTable">子表对应实体类类型</typeparam>
        /// <param name="joinEntity">连接条件</param>
        /// <returns></returns>
        public static MainTable ReadMainEntity2<MainTable, SubTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where MainTable : class,new()
        {
            return EntityOperations.ReadMainEntity2(mainConditionExpression, subConditionExpression, joinConditionExpression, DB);
        }

        /// <summary>
        /// 关联读取主表
        /// </summary>
        /// <typeparam name="MainTable"></typeparam>
        /// <typeparam name="SubTable"></typeparam>
        /// <param name="joinEntity"></param>
        /// <returns></returns>
        public static MainTable ReadMainEntity<MainTable, SubTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity) where MainTable : class,new()
        {
            return EntityOperations.ReadMainEntity(joinEntity, DB);
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
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression) where SubTable : class,new()
        {
            return EntityOperations.ReadSubEntity2(mainConditionExpression, subConditionExpression, joinConditionExpression, DB);
        }

        /// <summary>
        /// 关联读取子表
        /// </summary>
        /// <typeparam name="MainTable"></typeparam>
        /// <typeparam name="SubTable"></typeparam>
        /// <param name="joinEntity"></param>
        /// <returns></returns>
        public static SubTable ReadSubEntity<MainTable, SubTable>(
            GenericJoinEntity<MainTable, SubTable> joinEntity) where SubTable : class,new()
        {
            return EntityOperations.ReadSubEntity(joinEntity, DB);
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
            params int[] maxRowCounts) where DetailsTable : class,new()
        {
            return EntityOperations.ReadDetailsEntityList2(mainConditionExpression, detailsConditionExpression, joinConditionExpression, DB, maxRowCounts);
        }

        /// <summary>
        /// 读取一个明细表的实体类的列表
        /// </summary>
        /// <typeparam name="MainTable">主表的类型</typeparam>
        /// <typeparam name="DetailsTable">明细表的类型</typeparam>
        /// <param name="joinEntity">连接条件</param>
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns>明细表实体类的列表</returns>
        public static List<DetailsTable> ReadDetailsEntityList<MainTable, DetailsTable>(
            GenericJoinEntity<MainTable, DetailsTable> joinEntity,
            params int[] maxRowCounts) where DetailsTable : class,new()
        {
            return EntityOperations.ReadDetailsEntityList(joinEntity, DB);
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
        /// <param name="maxRowCounts">读取的记录数量</param>
        /// <returns></returns>
        public static List<DetailsTable> ReadDetailsEntityList2<MainTable, SubTable, DetailsTable>(Expression<Func<MainTable, bool>> mainConditionExpression,
            Expression<Func<SubTable, bool>> subConditionExpression,
            Expression<Func<DetailsTable, bool>> detailsConditionExpression,
            Expression<Func<MainTable, SubTable, bool>> joinConditionExpression0,
            Expression<Func<MainTable, DetailsTable, bool>> joinConditionExpression1,
            params int[] maxRowCounts) where DetailsTable : class,new()
        {
            return EntityOperations.ReadDetailsEntityList2(mainConditionExpression, subConditionExpression, detailsConditionExpression, joinConditionExpression0, joinConditionExpression1, DB, maxRowCounts);
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
            params int[] maxRowCounts) where DetailsTable : class,new()
        {
            return EntityOperations.ReadDetailsEntityList(joinEntity0, joinEntity1, DB, maxRowCounts);
        }

        /// <summary>
        /// 获取符合条件的实体的数量
        /// </summary>
        /// <typeparam name="E">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <returns>符合条件的实体的数量</returns>
        public static int GetEntityCount2<E>(Expression<Func<E, bool>> conditionExpression)
        {
            return EntityOperations.GetEntityCount2(conditionExpression);
        }

        /// <summary>
        /// 查询是否存在指定条件的记录
        /// </summary>
        /// <typeparam name="E">实体类别</typeparam>
        /// <param name="conditionExpression">查询条件</param>
        /// <returns>是否存在指定条件的记录</returns>
        public static bool ExistsRecord2<E>(Expression<Func<E, bool>> conditionExpression)
        {
            return EntityOperations.ExistsRecord2(conditionExpression);
        }
    }
}
