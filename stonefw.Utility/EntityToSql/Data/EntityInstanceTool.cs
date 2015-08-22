using System.Collections.Generic;
using System.Data;
using System.Reflection;
using stonefw.Utility.EntityToSql.Entity;
using System.Linq;

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
        public static T FillOneEntity<T>(IDataReader reader) where T : class, new()
        {
            List<PropertyInfo> entityPropertyInfo = DbTableMapping.GetEntityPropertyInfos(typeof(T));
            T entity = new T();

            for (int i = 0; i < entityPropertyInfo.Count; i++)
            {
                if (reader.IsDBNull(i))
                    continue;
                entityPropertyInfo[i].SetValue(entity, reader.GetValue(i), null);
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
        public static GenericPairEntity<TA, TB> FillOnePairEntity<TA, TB>(IDataReader reader)
            where TA : class, new()
            where TB : class, new()
        {
            List<PropertyInfo> entityPropertyInfoA = DbTableMapping.GetEntityPropertyInfos(typeof(TA));
            List<PropertyInfo> entityPropertyInfoB = DbTableMapping.GetEntityPropertyInfos(typeof(TB));
            GenericPairEntity<TA, TB> pair = new GenericPairEntity<TA, TB>();
            pair.EntityA = new TA();
            pair.EntityB = new TB();

            int offset = 0;
            for (int i = 0; i < entityPropertyInfoA.Count; i++)
            {
                if (reader.IsDBNull(offset + i))
                    continue;
                entityPropertyInfoA[i].SetValue(pair.EntityA, reader.GetValue(offset + i), null);
            }

            offset = entityPropertyInfoA.Count;
            for (int i = 0; i < entityPropertyInfoB.Count; i++)
            {
                if (reader.IsDBNull(offset + i))
                    continue;
                entityPropertyInfoB[i].SetValue(pair.EntityB, reader.GetValue(offset + i), null);
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
        public static List<string> GetNotNullFields<T>(T entity)
        {
            List<string> entityFieldNames = DbTableMapping.GetEntityFieldNames((typeof(T)));
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos((typeof(T)));
            List<string> notNullEntityFields = new List<string>(entityFieldNames.Count);

            for (int i = 0; i < entityFieldNames.Count; i++)
            {
                if (entityPropertyInfos[i].GetValue(entity, null) != null)
                    notNullEntityFields.Add(entityFieldNames[i]);
            }

            return notNullEntityFields;
        }

        /// <summary>
        /// 检索出Entity实例中有设置值的属性
        /// </summary>
        /// <typeparam name="T">实体类的类型</typeparam>
        /// <param name="entity">实体类的实例</param>
        /// <param name="entityPropertyInfos">实体类对应的属性</param>
        /// <returns>Entity实例中有设置值的属性</returns>
        public static List<PropertyInfo> GetNotNullEntityPropertys<T>(T entity)
        {
            List<PropertyInfo> entityPropertyInfos = DbTableMapping.GetEntityPropertyInfos((typeof(T)));
            List<PropertyInfo> notNullEntityPropertys = new List<PropertyInfo>(entityPropertyInfos.Count);

            for (int i = 0; i < entityPropertyInfos.Count; i++)
            {
                if (entityPropertyInfos[i].GetValue(entity, null) != null)
                    notNullEntityPropertys.Add(entityPropertyInfos[i]);
            }

            return notNullEntityPropertys;
        }

        /// <summary>
        /// 判断Entity实例中有没有设置主键值
        /// </summary>
        public static bool HasPrimaryKeyValue<T>(T entity)
        {
            if (!DbTableMapping.HasPrimaryKey(typeof(T)))
                return false;

            List<string> notNullEntityFields = GetNotNullFields(entity);
            if (notNullEntityFields == null)
                return false;

            var primaryKeyEntityFieldNames = DbTableMapping.GetPrimaryKeyOfEntityField(typeof(T));
            for (int i = 0; i < primaryKeyEntityFieldNames.Count; i++)
            {
                if (!notNullEntityFields.Any(n => n == primaryKeyEntityFieldNames[i]))
                    return false;
            }

            return true;
        }
    }
}
