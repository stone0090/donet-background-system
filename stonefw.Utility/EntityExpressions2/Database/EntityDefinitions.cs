using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions2.Entity;

namespace stonefw.Utility.EntityExpressions2.Utility.InternalEntityUtilitys
{
    /// <summary>
    /// 实体定义集合
    /// </summary>
    internal static class EntityDefinitions
    {
        static EntityDefinitions()
        {
            _EntityDefinitions = new List<DbTableMappingEntity>(256);
        }        

        /// <summary>
        /// 实体相关定义的缓存
        /// </summary>
        private static List<DbTableMappingEntity> _EntityDefinitions = null;

        private static DbTableMappingEntity GetDefinitionBufferItem(object theEntity)
        {
            Type theEntityType = theEntity.GetType();
            return GetDefinitionBufferItem(theEntityType);
        }

        /// <summary>
        /// 加载并保存实体定义
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        private static DbTableMappingEntity GetDefinitionBufferItem(Type theEntityType)
        {
            lock (_EntityDefinitions)
            {
                var typeName = theEntityType.FullName;
                var theBufferItem = _EntityDefinitions.FirstOrDefault(n => n.TypeName == typeName);
                if (theBufferItem != null)
                    return theBufferItem;

                theBufferItem = new DbTableMappingEntity();
                theBufferItem.TypeName = typeName;
                theBufferItem.DbTableName = EntityMemberIterateUtilitys.GetEntityDBTableName(theEntityType);
                theBufferItem.EntityEffectiveMemberNames = EntityMemberIterateUtilitys.GetEntityEffectiveMemberNames(theEntityType);
                theBufferItem.EntityEffectivePropertyInfos = EntityMemberIterateUtilitys.GetEffectivePropertyInfos(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                theBufferItem.DbColumnNameMappings = EntityMemberIterateUtilitys.GetEffectiveMemberDBColumnNames(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                theBufferItem.DbColumnTypeMappings = EntityMemberIterateUtilitys.GetEffectiveMemberDBColumnTypes(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                theBufferItem.EntityMemberOfIdentityColumn = EntityMemberIterateUtilitys.GetEntityMemberOfIdentityColumn(theEntityType);

                if (!string.IsNullOrEmpty(theBufferItem.EntityMemberOfIdentityColumn))
                    theBufferItem.IdentityColumnName = theBufferItem.DbColumnNameMappings[theBufferItem.EntityMemberOfIdentityColumn];

                theBufferItem.EntityMembersOfPrimaryKey = EntityMemberIterateUtilitys.GetEntityMembersOfPrimaryKey(theEntityType);

                if (theBufferItem.EntityMembersOfPrimaryKey != null && theBufferItem.EntityMembersOfPrimaryKey.Count > 0)
                {
                    theBufferItem.ColumnNamesOfPrimaryKey = new List<string>(theBufferItem.EntityMembersOfPrimaryKey.Count);
                    for (int i = 0; i < theBufferItem.EntityMembersOfPrimaryKey.Count; i++)
                    {
                        string columnName = theBufferItem.DbColumnNameMappings[theBufferItem.EntityMembersOfPrimaryKey[i]];
                        theBufferItem.ColumnNamesOfPrimaryKey.Add(columnName);
                    }
                }

                theBufferItem.DbColumnNameReverseMappings = new Dictionary<string, string>(theBufferItem.DbColumnNameMappings.Count);
                for (int i = 0; i < theBufferItem.EntityEffectiveMemberNames.Length; i++)
                {
                    string memberName = theBufferItem.EntityEffectiveMemberNames[i];
                    string dbColumnName = theBufferItem.DbColumnNameMappings[memberName];
                    theBufferItem.DbColumnNameReverseMappings.Add(dbColumnName, memberName);
                }

                _EntityDefinitions.Add(theBufferItem);
                return theBufferItem;
            }
        }        

        /// <summary>
        /// 获取完整的定义
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static DbTableMappingEntity GetDefinitionItem(this Type theEntityType)
        {
            DbTableMappingEntity bufferItem = GetDefinitionBufferItem(theEntityType);
            return bufferItem;
        }

        /// <summary>
        /// 获取完整的定义
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static string GetDBTableName(this Type theEntityType)
        {
            DbTableMappingEntity bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null)
                return null;
            return bufferItem.DbTableName;
        }

        /// <summary>
        /// 获取字段在数据库表中的名称
        /// </summary>
        /// <param name="m"></param>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static string GetDBColumnName(this MemberExpression m, Type theEntityType)
        {
            string typeName = theEntityType.FullName;
            string memberName = m.Member.Name;

            DbTableMappingEntity bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null || bufferItem.EntityEffectiveMemberNames == null || bufferItem.EntityEffectiveMemberNames.Length == 0)
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            if (!bufferItem.EntityEffectiveMemberNames.Any(n => n == memberName))
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            return bufferItem.DbColumnNameMappings[memberName];
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="m"></param>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static DbType GetDBColumnType(this MemberExpression m, Type theEntityType)
        {
            string typeName = theEntityType.FullName;
            string memberName = m.Member.Name;

            DbTableMappingEntity bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null || bufferItem.EntityEffectiveMemberNames == null || bufferItem.EntityEffectiveMemberNames.Length == 0)
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            if (!bufferItem.EntityEffectiveMemberNames.Any(n => n == memberName))
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            return bufferItem.DbColumnTypeMappings[memberName];
        }
    }
}
