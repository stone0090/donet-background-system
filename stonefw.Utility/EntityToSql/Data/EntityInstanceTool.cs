using System.Collections.Generic;
using System.Data;
using System.Reflection;
using stonefw.Utility.EntityToSql.Entity;

namespace stonefw.Utility.EntityToSql.Data
{
    /// <summary>
    /// 实体实例的工具
    /// </summary>
    public static class EntityInstanceTool
    {
        /// <summary>
        /// 从DataReader实例化一个Entity
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="reader">DataReader</param>
        /// <param name="entityPropertyInfos">已经排好序的实体类的属性集合</param>
        /// <returns></returns>
        public static T FillOneEntity<T>(IDataReader reader, List<PropertyInfo> entityPropertyInfos) where T : class, new()
        {
            T entity = new T();
            for (int i = 0; i < entityPropertyInfos.Count; i++)
            {
                if (reader.IsDBNull(i))
                    continue;

                entityPropertyInfos[i].SetValue(entity, reader.GetValue(i), null);
            }
            return entity;
        }

        /// <summary>
        /// 从DataReader实例化一个Entity对
        /// </summary>
        /// <typeparam name="TA">实体类的类型TA</typeparam>
        /// <typeparam name="TB">实体类的类型TB</typeparam>
        /// <param name="reader">DataReader</param>
        /// <param name="entityPropertysA">已经排好序的实体类的属性集合TA</param>
        /// <param name="entityPropertysB">已经排好序的实体类的属性集合TB</param>
        /// <returns></returns>
        public static GenericPairEntity<TA, TB> FillOneEntityPair<TA, TB>(IDataReader reader, List<PropertyInfo> entityPropertysA, List<PropertyInfo> entityPropertysB)
            where TA : class, new()
            where TB : class, new()
        {
            GenericPairEntity<TA, TB> pair = new GenericPairEntity<TA, TB>();
            pair.EntityA = new TA();
            pair.EntityB = new TB();
            int offset = 0;
            for (int i = 0; i < entityPropertysA.Count; i++)
            {
                if (reader.IsDBNull(offset + i))
                {
                    continue;
                }
                entityPropertysA[i].SetValue(pair.EntityA, reader.GetValue(offset + i), null);
            }
            offset = entityPropertysA.Count;
            for (int i = 0; i < entityPropertysB.Count; i++)
            {
                if (reader.IsDBNull(offset + i))
                {
                    continue;
                }
                entityPropertysB[i].SetValue(pair.EntityB, reader.GetValue(offset + i), null);
            }
            return pair;
        }

        /// <summary>
        /// 检索出Entity实例中有设置值的字段
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="entity">实体类的实例</param>
        /// <param name="effectiveEntityMemberNames">有效的Entity字段名称</param>
        /// <param name="entityPropertyInfos">实体类对应的属性</param>
        /// <returns>Entity实例中有设置值的字段</returns>
        public static List<string> GetNotNullEntityMembers<T>(T entity, List<string> effectiveEntityMemberNames, List<PropertyInfo> entityPropertyInfos)
        {
            List<string> notNullEntityMembers = new List<string>(effectiveEntityMemberNames.Count);
            for (int i = 0; i < effectiveEntityMemberNames.Count; i++)
            {
                if (entityPropertyInfos[i].GetValue(entity, null) != null)
                {
                    notNullEntityMembers.Add(effectiveEntityMemberNames[i]);
                }
            }
            return notNullEntityMembers;
        }

        /// <summary>
        /// 检索出Entity实例中有设置值的属性
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="entity">实体类的实例</param>
        /// <param name="entityPropertyInfos">实体类对应的属性</param>
        /// <returns>Entity实例中有设置值的属性</returns>
        public static List<PropertyInfo> GetNotNullEntityPropertyInfos<T>(T entity, List<PropertyInfo> entityPropertyInfos)
        {
            List<PropertyInfo> notNullEntityPropertyInfos = new List<PropertyInfo>(entityPropertyInfos.Count);
            for (int i = 0; i < entityPropertyInfos.Count; i++)
            {
                if (entityPropertyInfos[i].GetValue(entity, null) != null)
                {
                    notNullEntityPropertyInfos.Add(entityPropertyInfos[i]);
                }
            }
            return notNullEntityPropertyInfos;
        }
    }
}
