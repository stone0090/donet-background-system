/*
 * First creat by wukea[2013/09/09]
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions.Attribute;

namespace stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys
{
    /// <summary>
    /// 实体成员遍历的工具
    /// </summary>
    internal static class EntityMemberIterateUtilitys
    {
        /// <summary>
        /// 获取实体对应的数据表的名称
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static string GetEntityDBTableName(Type entityType)
        {
            try
            {
                object[] attributes = entityType.GetCustomAttributes(typeof(Table), false);
                Table theAttribute = (Table)attributes[0];
                return theAttribute.TableName;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取实体对应的数据表的名称
        /// </summary>
        /// <param name="obj">实体的一个实例</param>
        /// <returns></returns>
        public static string GetEntityDBTableName(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            Type entityType = obj.GetType();
            return GetEntityDBTableName(entityType);
        }

        /// <summary>
        /// 扫描有效字段的名称列表
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static string[] GetEntityEffectiveMemberNames(Type entityType)
        {
            PropertyInfo[] propertys = entityType.GetProperties();
            if (propertys == null || propertys.Length == 0)
            {
                return null;
            }

            List<string> effectivePropertyNames = new List<string>(propertys.Length);
            for (int i = 0; i < propertys.Length; i++)
            {
                try
                {
                    object[] attributes = propertys[i].GetCustomAttributes(typeof(Field), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        effectivePropertyNames.Add(propertys[i].Name);
                    }
                }
                catch
                {
                }
            }

            return (effectivePropertyNames.Count > 0) ? effectivePropertyNames.ToArray() : null;
        }

        /// <summary>
        /// 扫描有效字段的名称列表
        /// </summary>
        /// <param name="obj">实体的一个实例</param>
        /// <returns></returns>
        public static string[] GetEntityEffectiveMemberNames(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            Type entityType = obj.GetType();
            return GetEntityEffectiveMemberNames(entityType);
        }

        /// <summary>
        /// 获取有效的属性集合
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, PropertyInfo> GetEffectivePropertyInfos(Type entityType, string[] memberNames)
        {
            if (memberNames == null || memberNames.Length == 0)
            {
                return null;
            }

            PropertyInfo[] propertys = entityType.GetProperties();
            Dictionary<string, PropertyInfo> dicPropertyInfos = new Dictionary<string, PropertyInfo>(memberNames.Length);
            for (int i = 0; i < memberNames.Length; i++)
            {
                var thePropertyInfo = propertys.FirstOrDefault(n => n.Name == memberNames[i]);
                dicPropertyInfos.Add(memberNames[i], thePropertyInfo);
            }

            return dicPropertyInfos;
        }

        /// <summary>
        /// 获取有效的属性集合
        /// </summary>
        /// <param name="obj">实体的一个实例</param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, PropertyInfo> GetEffectivePropertyInfos(object obj, string[] memberNames)
        {
            if (obj == null)
            {
                return null;
            }

            Type entityType = obj.GetType();
            return GetEffectivePropertyInfos(entityType, memberNames);
        }

        /// <summary>
        /// 获取数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEffectiveMemberDBColumnNames(Type entityType, string[] memberNames)
        {
            if (memberNames == null || memberNames.Length == 0)
            {
                return null;
            }

            PropertyInfo[] propertys = entityType.GetProperties();
            Dictionary<string, string> dicDBColumnNames = new Dictionary<string, string>(memberNames.Length);
            for (int i = 0; i < memberNames.Length; i++)
            {
                var thePropertyInfo = propertys.FirstOrDefault(n => n.Name == memberNames[i]);
                object[] attributes = thePropertyInfo.GetCustomAttributes(typeof(Field), false);
                Field theAttribute = (Field)attributes[0];
                dicDBColumnNames.Add(memberNames[i], theAttribute.FieldName);
            }

            return dicDBColumnNames;
        }

        /// <summary>
        /// 获取数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        /// <param name="obj">实体的一个实例</param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEffectiveMemberDBColumnNames(object obj, string[] memberNames)
        {
            if (obj == null)
            {
                return null;
            }

            Type entityType = obj.GetType();
            return GetEffectiveMemberDBColumnNames(entityType, memberNames);
        }

        /// <summary>
        /// 获取数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, DbType> GetEffectiveMemberDBColumnTypes(Type entityType, string[] memberNames)
        {
            if (memberNames == null || memberNames.Length == 0)
            {
                return null;
            }

            PropertyInfo[] propertys = entityType.GetProperties();
            Dictionary<string, DbType> dicDBColumnTypes = new Dictionary<string, DbType>(memberNames.Length);
            for (int i = 0; i < memberNames.Length; i++)
            {
                var thePropertyInfo = propertys.FirstOrDefault(n => n.Name == memberNames[i]);
                object[] attributes = thePropertyInfo.GetCustomAttributes(typeof(Field), false);
                Field theAttribute = (Field)attributes[0];
                dicDBColumnTypes.Add(memberNames[i], theAttribute.FieldDBType);
            }

            return dicDBColumnTypes;
        }

        /// <summary>
        /// 获取数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        /// <param name="obj">实体的一个实例</param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        public static Dictionary<string, DbType> GetEffectiveMemberDBColumnTypes(object obj, string[] memberNames)
        {
            if (obj == null)
            {
                return null;
            }

            Type entityType = obj.GetType();
            return GetEffectiveMemberDBColumnTypes(entityType, memberNames);
        }

        /// <summary>
        /// 获取标识列对应的实体类成员
        /// </summary>
        /// <param name="entityType">实体类别</param>
        /// <returns>标识列对应的实体类成员</returns>
        public static string GetEntityMemberOfIdentityColumn(Type entityType)
        {
            PropertyInfo[] propertys = entityType.GetProperties();
            string identityMember = null;
            for (int i = 0; i < propertys.Length; i++)
            {
                var thePropertyInfo = propertys[i];
                object[] attributes = thePropertyInfo.GetCustomAttributes(typeof(Field), false);
                if(attributes==null || attributes.Length==0)
                {
                    continue;
                }
                Field theAttribute = (Field)attributes[0];
                if (theAttribute.IsIdentityField)
                {
                    identityMember = thePropertyInfo.Name;
                    break;
                }                
            }

            return identityMember;
        }

        /// <summary>
        /// 获取主键对应的成员名称
        /// </summary>
        /// <param name="entityType">实体类别</param>
        /// <returns>主键对应的成员名称</returns>
        public static List<string> GetEntityMembersOfPrimaryKey(Type entityType)
        {
            PropertyInfo[] propertys = entityType.GetProperties();
            List<string> tmpMembers = new List<string>(propertys.Length);
            for (int i = 0; i < propertys.Length; i++)
            {
                var thePropertyInfo = propertys[i];
                object[] attributes = thePropertyInfo.GetCustomAttributes(typeof(Field), false);
                if (attributes == null || attributes.Length == 0)
                {
                    continue;
                }
                Field theAttribute = (Field)attributes[0];
                if (theAttribute.IsPrimaryKey)
                {
                    tmpMembers.Add(thePropertyInfo.Name);
                }
            }

            return tmpMembers;
        }

        /// <summary>
        /// 获取主键对应的字段名称
        /// </summary>
        /// <param name="entityType">实体类别</param>
        /// <returns>主键对应的字段名称</returns>
        public static List<string> GetDBColumnNamesOfPrimaryKey(Type entityType)
        {
            PropertyInfo[] propertys = entityType.GetProperties();
            List<string> tmpColumns = new List<string>(propertys.Length);
            for (int i = 0; i < propertys.Length; i++)
            {
                var thePropertyInfo = propertys[i];
                object[] attributes = thePropertyInfo.GetCustomAttributes(typeof(Field), false);
                if (attributes == null || attributes.Length == 0)
                {
                    continue;
                }
                Field theAttribute = (Field)attributes[0];
                if (theAttribute.IsPrimaryKey)
                {
                    tmpColumns.Add(theAttribute.FieldName);
                }
            }

            return tmpColumns;
        }
    }
}
