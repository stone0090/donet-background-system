/*
 * First creat by wukea[2013/09/09]
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys
{
    /// <summary>
    /// 实体定义集合
    /// </summary>
    internal static class EntityDefinitions
    {
        static EntityDefinitions()
        {
            _EntityDefinitions = new List<EntityDefinitionBufferItem>(256);
        }

        #region 缓存的数据及操作

        /// <summary>
        /// 实体相关定义的缓存
        /// </summary>
        private static List<EntityDefinitionBufferItem> _EntityDefinitions = null;
        private static EntityDefinitionBufferItem GetDefinitionBufferItem(Type theEntityType)
        {
            var typeName = theEntityType.FullName;
            var theItem = _EntityDefinitions.FirstOrDefault(n => n.TypeName == typeName);
            if (theItem != null)
            {
                return theItem;
            }
            return LoadAndSaveDefinitionBufferItem(theEntityType);
        }
        private static EntityDefinitionBufferItem GetDefinitionBufferItem(object theEntity)
        {
            Type theEntityType = theEntity.GetType();
            return GetDefinitionBufferItem(theEntityType);
        }

        /// <summary>
        /// 加载并保存实体定义
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        private static EntityDefinitionBufferItem LoadAndSaveDefinitionBufferItem(Type theEntityType)
        {
            lock (_EntityDefinitions)
            {
                var typeName = theEntityType.FullName;
                EntityDefinitionBufferItem theBufferItem = _EntityDefinitions.FirstOrDefault(n => n.TypeName == typeName);
                if (theBufferItem != null)
                {
                    return theBufferItem;
                }
                else
                {
                    theBufferItem = new EntityDefinitionBufferItem();
                    theBufferItem.TypeName = typeName;
                    theBufferItem.DBTableName = EntityMemberIterateUtilitys.GetEntityDBTableName(theEntityType);
                    theBufferItem.EntityEffectiveMemberNames = EntityMemberIterateUtilitys.GetEntityEffectiveMemberNames(theEntityType);
                    theBufferItem.EntityEffectivePropertyInfos = EntityMemberIterateUtilitys.GetEffectivePropertyInfos(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                    theBufferItem.DBColumnNameMappings = EntityMemberIterateUtilitys.GetEffectiveMemberDBColumnNames(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                    theBufferItem.DBColumnTypeMappings = EntityMemberIterateUtilitys.GetEffectiveMemberDBColumnTypes(theEntityType, theBufferItem.EntityEffectiveMemberNames);
                    theBufferItem.EntityMemberOfIdentityColumn = EntityMemberIterateUtilitys.GetEntityMemberOfIdentityColumn(theEntityType);
                    if (!string.IsNullOrEmpty(theBufferItem.EntityMemberOfIdentityColumn))
                    {
                        theBufferItem.IdentityColumnName = theBufferItem.DBColumnNameMappings[theBufferItem.EntityMemberOfIdentityColumn];
                    }
                    theBufferItem.EntityMembersOfPrimaryKey = EntityMemberIterateUtilitys.GetEntityMembersOfPrimaryKey(theEntityType);
                    if (theBufferItem.EntityMembersOfPrimaryKey != null && theBufferItem.EntityMembersOfPrimaryKey.Count > 0)
                    {
                        theBufferItem.ColumnNamesOfPrimaryKey = new List<string>(theBufferItem.EntityMembersOfPrimaryKey.Count);
                        for (int i = 0; i < theBufferItem.EntityMembersOfPrimaryKey.Count; i++)
                        {
                            string columnName = theBufferItem.DBColumnNameMappings[theBufferItem.EntityMembersOfPrimaryKey[i]];
                            theBufferItem.ColumnNamesOfPrimaryKey.Add(columnName);
                        }
                    }

                    theBufferItem.DBColumnNameReverseMappings = new Dictionary<string, string>(theBufferItem.DBColumnNameMappings.Count);
                    for (int i = 0; i < theBufferItem.EntityEffectiveMemberNames.Length; i++)
                    {
                        string memberName = theBufferItem.EntityEffectiveMemberNames[i];
                        string dbColumnName = theBufferItem.DBColumnNameMappings[memberName];
                        theBufferItem.DBColumnNameReverseMappings.Add(dbColumnName, memberName);
                    }

                    _EntityDefinitions.Add(theBufferItem);
                    return theBufferItem;
                }
            }
        }


        #endregion

        /// <summary>
        /// 获取完整的定义
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static EntityDefinitionBufferItem GetDefinitionItem(this Type theEntityType)
        {
            EntityDefinitionBufferItem bufferItem = GetDefinitionBufferItem(theEntityType);
            return bufferItem;
        }

        /// <summary>
        /// 获取实体类的数据表名称
        /// </summary>
        /// <param name="theEntityType"></param>
        /// <returns></returns>
        public static string GetDBTableName(this Type theEntityType)
        {
            EntityDefinitionBufferItem bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null)
            {
                return null;
            }
            return bufferItem.DBTableName;
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

            EntityDefinitionBufferItem bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null || bufferItem.EntityEffectiveMemberNames == null || bufferItem.EntityEffectiveMemberNames.Length == 0)
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            if (!bufferItem.EntityEffectiveMemberNames.Any(n => n == memberName))
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            return bufferItem.DBColumnNameMappings[memberName];
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

            EntityDefinitionBufferItem bufferItem = GetDefinitionBufferItem(theEntityType);
            if (bufferItem == null || bufferItem.EntityEffectiveMemberNames == null || bufferItem.EntityEffectiveMemberNames.Length == 0)
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            if (!bufferItem.EntityEffectiveMemberNames.Any(n => n == memberName))
            {
                throw new Exception(string.Format("{0}中的字段{1}未设置数据库相关的信息！", typeName, memberName));
            }

            return bufferItem.DBColumnTypeMappings[memberName];
        }
    }
}
