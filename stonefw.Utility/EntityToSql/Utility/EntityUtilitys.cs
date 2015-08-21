//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//using stonefw.Utility.EntityToSql.Entity;
//using stonefw.Utility.EntityToSql.GenSQL;

//namespace stonefw.Utility.EntityToSql.Utilitys
//{
//    /// <summary>
//    /// 实体类的工具
//    /// </summary>
//    public static class EntityUtilitys
//    {
//        public static string GetDbColumnName(Type entityType, string memberName)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            return dbTableMappingEntity.DbColumnNames[memberName];
//        }

//        /// <summary>
//        /// 填充实体类的有效字段
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <param name="entityMemberNames">实体的名称</param>
//        /// <param name="entityPropertyInfos"></param>
//        public static void FillEffectiveEntityMembers(Type entityType, List<string> entityMemberNames, List<PropertyInfo> entityPropertyInfos)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            List<string> memberNames = dbTableMappingEntity.EntityFieldNames;
//            for (int i = 0; i < memberNames.Count; i++)
//            {
//                entityMemberNames.Add(memberNames[i]);
//                entityPropertyInfos.Add(dbTableMappingEntity.EntityPropertyInfos[memberNames[i]]);
//            }
//        }

//        /// <summary>
//        /// 获取实体指定成员对应的数据库字段名称
//        /// </summary>
//        /// <param name="entityType">实体类别</param>
//        /// <param name="appointMemberNames">指定成员的名称</param>
//        /// <returns>数据库字段名称</returns>
//        public static List<string> GetDBColumnNameOfAppointMembers(Type entityType, List<string> appointMemberNames)
//        {
//            List<string> dbColumnNames = new List<string>(appointMemberNames.Count);
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            for (int i = 0; i < appointMemberNames.Count; i++)
//            {
//                dbColumnNames.Add(dbTableMappingEntity.DbColumnNames[appointMemberNames[i]]);
//            }
//            return dbColumnNames;
//        }

//        /// <summary>
//        /// 获取实体指定成员对应的数据库字段类型
//        /// </summary>
//        /// <param name="entityType">实体类别</param>
//        /// <param name="appointMemberNames">指定成员的名称</param>
//        /// <returns>数据库字段类型</returns>
//        public static List<DbType> GetDBColumnTypeOfAppointMembers(Type entityType, List<string> appointMemberNames)
//        {
//            List<DbType> dbColumnTypes = new List<DbType>(appointMemberNames.Count);
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            for (int i = 0; i < appointMemberNames.Count; i++)
//            {
//                dbColumnTypes.Add(dbTableMappingEntity.DbColumnTypes[appointMemberNames[i]]);
//            }
//            return dbColumnTypes;
//        }

//        /// <summary>
//        /// 获取指定字段的属性
//        /// </summary>
//        /// <param name="entityType"></param>
//        /// <param name="appointMemberNames"></param>
//        /// <returns></returns>
//        public static List<PropertyInfo> GetPropertyInfosOfAppointMembers(Type entityType, List<string> appointMemberNames)
//        {
//            List<PropertyInfo> dbPropertyInfos = new List<PropertyInfo>(appointMemberNames.Count);
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            for (int i = 0; i < appointMemberNames.Count; i++)
//            {
//                dbPropertyInfos.Add(dbTableMappingEntity.EntityPropertyInfos[appointMemberNames[i]]);
//            }
//            return dbPropertyInfos;
//        }

//        /// <summary>
//        /// 获取指定数据库字段对应的属性（需要考虑部分字段没有对应的实体成员）
//        /// </summary>
//        /// <param name="entityType"></param>
//        /// <param name="appointDBColumns"></param>
//        /// <returns></returns>
//        public static List<PropertyInfo> GetPropertyInfosOfAppointDBColumns(Type entityType, List<string> appointDBColumns)
//        {
//            List<PropertyInfo> dbPropertyInfos = new List<PropertyInfo>(appointDBColumns.Count);
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            for (int i = 0; i < appointDBColumns.Count; i++)
//            {
//                //if (!dbTableMappingEntity.DbColumnNameReverseMappings.ContainsKey(appointDBColumns[i]))
//                if (!dbTableMappingEntity.DbColumnNames.ContainsValue(appointDBColumns[i]))
//                {
//                    dbPropertyInfos.Add(null);
//                    continue;
//                }
//                //string memberName = dbTableMappingEntity.DbColumnNameReverseMappings[appointDBColumns[i]];
//                string memberName = dbTableMappingEntity.DbColumnNames.FirstOrDefault(n => n.Value == appointDBColumns[i]).Key;
//                dbPropertyInfos.Add(dbTableMappingEntity.EntityPropertyInfos[memberName]);
//            }
//            return dbPropertyInfos;
//        }

//        /// <summary>
//        /// 获取实体类对应的数据表的表名
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <returns>数据表的表名</returns>
//        public static string GetTableNameOfEntity(Type entityType)
//        {
//            return DbTableMapping.GetDbTableName(entityType);
//        }

//        /// <summary>
//        /// 获取标识字段对应的成员
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <returns>标识字段对应的成员</returns>
//        public static string GetEntityMemberOfIndentity(Type entityType)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            //return dbTableMappingEntity.EntityMemberOfIdentityColumn;
//            return dbTableMappingEntity.DbIdentity.FirstOrDefault().Key;
//        }

//        /// <summary>
//        /// 获取标识字段的名称
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <returns>标识字段的名称</returns>
//        public static string GetColumnNameOfIndentity(Type entityType)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            //return dbTableMappingEntity.DbIdentity;
//            return dbTableMappingEntity.DbIdentity.FirstOrDefault().Value;
//        }

//        /// <summary>
//        /// 获取主键相关的成员
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <returns>主键相关的成员</returns>
//        public static List<string> GetEntityMemberOfPrimaryKey(Type entityType)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            //return dbTableMappingEntity.EntityMembersOfPrimaryKey;
//            return dbTableMappingEntity.DbPrimaryKeys.Select(n => n.Key).ToList();
//        }

//        /// <summary>
//        /// 获取主键的组成字段
//        /// </summary>
//        /// <param name="entityType">实体类的类型</param>
//        /// <returns>主键的组成字段</returns>
//        public static List<string> GetColumnNamesOfPrimaryKey(Type entityType)
//        {
//            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
//            //return dbTableMappingEntity.DbPrimaryKeys;
//            return dbTableMappingEntity.DbPrimaryKeys.Select(n => n.Value).ToList();
//        }
//    }
//}
