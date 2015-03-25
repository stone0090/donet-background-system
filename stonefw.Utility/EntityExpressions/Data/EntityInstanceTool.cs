using System.Collections.Generic;
using System.Data;
using System.Reflection;
using stonefw.Utility.EntityExpressions.Entitys;

namespace stonefw.Utility.EntityExpressions.Data
{
    /// <summary>
    /// 实体实例的工具
    /// </summary>
    public static class EntityInstanceTool
    {
        /// <summary>
        /// 从DataReader实例化一个Entity
        /// </summary>
        /// <typeparam name="D">实体类的类型</typeparam>
        /// <param name="reader">DataReader</param>
        /// <param name="entityPropertys">已经排好序的实体类的属性集合</param>
        /// <returns></returns>
        public static D FillOneEntity<D>(IDataReader reader, List<PropertyInfo> entityPropertys) where D : class,new()
        {
            D entity = new D();
            for (int i = 0; i < entityPropertys.Count; i++)
            {
                if (reader.IsDBNull(i))
                {
                    continue;
                }
                entityPropertys[i].SetValue(entity, reader.GetValue(i), null);
            }
            return entity;
        }

        /// <summary>
        /// 从DataReader实例化一个Entity对
        /// </summary>
        /// <typeparam name="A">实体类的类型A</typeparam>
        /// <typeparam name="B">实体类的类型B</typeparam>
        /// <param name="reader">DataReader</param>
        /// <param name="entityPropertysA">已经排好序的实体类的属性集合A</param>
        /// <param name="entityPropertysB">已经排好序的实体类的属性集合B</param>
        /// <returns></returns>
        public static EntityPairs<A, B> FillOneEntityPair<A, B>(IDataReader reader, List<PropertyInfo> entityPropertysA, List<PropertyInfo> entityPropertysB)
            where A : class,new()
            where B : class,new()
        {
            EntityPairs<A, B> pair = new EntityPairs<A, B>();
            pair.EntityA = new A();
            pair.EntityB = new B();
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
        /// <typeparam name="E">实体类的类型</typeparam>
        /// <param name="entity">实体类的实例</param>
        /// <param name="effectiveEntityMemberNames">有效的Entity字段名称</param>
        /// <param name="entityPropertys">实体类对应的属性</param>
        /// <returns>Entity实例中有设置值的字段</returns>
        public static List<string> GetNotNullEntityMembers<E>(E entity, List<string> effectiveEntityMemberNames, List<PropertyInfo> entityPropertys)
        {
            List<string> notNullEntityMembers = new List<string>(effectiveEntityMemberNames.Count);
            for (int i = 0; i < effectiveEntityMemberNames.Count; i++)
            {
                if (entityPropertys[i].GetValue(entity, null) != null)
                {
                    notNullEntityMembers.Add(effectiveEntityMemberNames[i]);
                }
            }
            return notNullEntityMembers;
        }

        /// <summary>
        /// 检索出Entity实例中有设置值的属性
        /// </summary>
        /// <typeparam name="E">实体类的类型</typeparam>
        /// <param name="entity">实体类的实例</param>
        /// <param name="entityPropertys">实体类对应的属性</param>
        /// <returns>Entity实例中有设置值的属性</returns>
        public static List<PropertyInfo> GetNotNullEntityPropertyInfos<E>(E entity, List<PropertyInfo> entityPropertys)
        {
            List<PropertyInfo> notNullEntityPropertyInfos = new List<PropertyInfo>(entityPropertys.Count);
            for (int i = 0; i < entityPropertys.Count; i++)
            {
                if (entityPropertys[i].GetValue(entity, null) != null)
                {
                    notNullEntityPropertyInfos.Add(entityPropertys[i]);
                }
            }
            return notNullEntityPropertyInfos;
        }
    }
}
