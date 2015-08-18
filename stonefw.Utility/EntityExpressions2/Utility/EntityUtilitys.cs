using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions2.Utility.InternalEntityUtilitys;
using stonefw.Utility.EntityExpressions2.Entity;

namespace stonefw.Utility.EntityExpressions2.Utilitys
{
    /// <summary>
    /// 实体类的工具
    /// </summary>
    public static class EntityUtilitys
    {
        public static string GetDbColumnName(Type entityType, string memberName)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            return definitionItme.DbColumnNameMappings[memberName];
        }

        /// <summary>
        /// 填充实体类的有效字段
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <param name="entityMemberNames">实体的名称</param>
        /// <param name="entityPropertys"></param>
        public static void FillEffectiveEntityMembers(Type entityType, List<string> entityMemberNames, List<PropertyInfo> entityPropertys)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            string[] memberNames = definitionItme.EntityEffectiveMemberNames;
            for (int i = 0; i < memberNames.Length; i++)
            {
                entityMemberNames.Add(memberNames[i]);
                entityPropertys.Add(definitionItme.EntityEffectivePropertyInfos[memberNames[i]]);
            }
        }

        /// <summary>
        /// 获取实体指定成员对应的数据库字段名称
        /// </summary>
        /// <param name="entityType">实体类别</param>
        /// <param name="appointMemberNames">指定成员的名称</param>
        /// <returns>数据库字段名称</returns>
        public static List<string> GetDBColumnNameOfAppointMembers(Type entityType, List<string> appointMemberNames)
        {
            List<string> dbColumnNames = new List<string>(appointMemberNames.Count);
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            for (int i = 0; i < appointMemberNames.Count; i++)
            {
                dbColumnNames.Add(definitionItme.DbColumnNameMappings[appointMemberNames[i]]);
            }
            return dbColumnNames;
        }

        /// <summary>
        /// 获取实体指定成员对应的数据库字段类型
        /// </summary>
        /// <param name="entityType">实体类别</param>
        /// <param name="appointMemberNames">指定成员的名称</param>
        /// <returns>数据库字段类型</returns>
        public static List<DbType> GetDBColumnTypeOfAppointMembers(Type entityType, List<string> appointMemberNames)
        {
            List<DbType> dbColumnTypes = new List<DbType>(appointMemberNames.Count);
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            for (int i = 0; i < appointMemberNames.Count; i++)
            {
                dbColumnTypes.Add(definitionItme.DbColumnTypeMappings[appointMemberNames[i]]);
            }
            return dbColumnTypes;
        }

        /// <summary>
        /// 获取指定字段的属性
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="appointMemberNames"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyInfosOfAppointMembers(Type entityType, List<string> appointMemberNames)
        {
            List<PropertyInfo> dbPropertyInfos = new List<PropertyInfo>(appointMemberNames.Count);
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            for (int i = 0; i < appointMemberNames.Count; i++)
            {
                dbPropertyInfos.Add(definitionItme.EntityEffectivePropertyInfos[appointMemberNames[i]]);
            }
            return dbPropertyInfos;
        }

        /// <summary>
        /// 获取指定数据库字段对应的属性（需要考虑部分字段没有对应的实体成员）
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="appointDBColumns"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyInfosOfAppointDBColumns(Type entityType, List<string> appointDBColumns)
        {
            List<PropertyInfo> dbPropertyInfos = new List<PropertyInfo>(appointDBColumns.Count);
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            for (int i = 0; i < appointDBColumns.Count; i++)
            {
                if (!definitionItme.DbColumnNameReverseMappings.ContainsKey(appointDBColumns[i]))
                {
                    dbPropertyInfos.Add(null);
                    continue;
                }
                string memberName = definitionItme.DbColumnNameReverseMappings[appointDBColumns[i]];
                dbPropertyInfos.Add(definitionItme.EntityEffectivePropertyInfos[memberName]);
            }
            return dbPropertyInfos;
        }

        /// <summary>
        /// 获取实体类对应的数据表的表名
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <returns>数据表的表名</returns>
        public static string GetTableNameOfEntity(Type entityType)
        {
            return entityType.GetDBTableName();
        }

        /// <summary>
        /// 获取标识字段对应的成员
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <returns>标识字段对应的成员</returns>
        public static string GetEntityMemberOfIndentity(Type entityType)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            return definitionItme.EntityMemberOfIdentityColumn;
        }

        /// <summary>
        /// 获取标识字段的名称
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <returns>标识字段的名称</returns>
        public static string GetColumnNameOfIndentity(Type entityType)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            return definitionItme.IdentityColumnName;
        }

        /// <summary>
        /// 获取主键相关的成员
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <returns>主键相关的成员</returns>
        public static List<string> GetEntityMemberOfPrimaryKey(Type entityType)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            return definitionItme.EntityMembersOfPrimaryKey;
        }

        /// <summary>
        /// 获取主键的组成字段
        /// </summary>
        /// <param name="entityType">实体类的类型</param>
        /// <returns>主键的组成字段</returns>
        public static List<string> GetColumnNamesOfPrimaryKey(Type entityType)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            return definitionItme.ColumnNamesOfPrimaryKey;
        }
    }
}
