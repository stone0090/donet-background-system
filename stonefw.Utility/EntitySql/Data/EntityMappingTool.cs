using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntitySql.Entity;
using stonefw.Utility.EntitySql.Attribute;
using System.Data.Common;

namespace stonefw.Utility.EntitySql.Data
{
    internal static class EntityMappingTool
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static List<EntityMappingEntity> _dbMappingEntityBuffer = null;

        static EntityMappingTool()
        {
            _dbMappingEntityBuffer = new List<EntityMappingEntity>(1024);
        }

        public static EntityMappingEntity GetDbTableMappingEntity(Type entityType)
        {
            lock (_dbMappingEntityBuffer)
            {
                var typeName = entityType.FullName;
                var buffer = _dbMappingEntityBuffer.FirstOrDefault(n => n.EntityTypeName == typeName);
                if (buffer != null)
                    return buffer;

                PropertyInfo[] propertys = entityType.GetProperties();
                if (propertys == null || propertys.Length == 0)
                    return null;

                object[] attributes = null;
                DataTable dt = null;
                var sql = string.Empty;
                var db = DatabaseFactory.CreateDatabase();

                var listFieldName = new List<string>();
                var dicPropertyInfo = new Dictionary<string, PropertyInfo>();
                var dicDbColumnName = new Dictionary<string, string>();
                var dicDbColumnType = new Dictionary<string, DbType>();
                var dicDbIdentity = new Dictionary<string, string>();
                var dicDbPrimaryKey = new Dictionary<string, string>();

                // EntityFieldNames,EntityPropertyInfos,DbColumnNames 
                for (int i = 0; i < propertys.Length; i++)
                {
                    try
                    {
                        attributes = propertys[i].GetCustomAttributes(typeof(Field), false);
                        if (attributes != null && attributes.Length > 0)
                        {
                            listFieldName.Add(propertys[i].Name);
                            var propertyInfo = propertys.FirstOrDefault(n => n.Name == propertys[i].Name);
                            dicPropertyInfo.Add(propertys[i].Name, propertyInfo);
                            dicDbColumnName.Add(propertys[i].Name, ((Field)attributes[0]).FieldName);
                        }
                    }
                    catch
                    {
                    }
                }

                // DbTableName
                attributes = entityType.GetCustomAttributes(typeof(Table), false);
                var dbTableName = ((Table)attributes[0]).TableName;

                // DbColumnTypes,DbIdentity 
                sql = @"SELECT a.name as ColumnName, c.name as ColumnType, a.is_identity FROM sys.columns a
                    INNER JOIN sys.tables b ON a.object_id = b.object_id
                    INNER JOIN sys.types c ON a.system_type_id = c.system_type_id 
                    WHERE b.name = @name ";

                using (DbCommand dm = db.GetSqlStringCommand(sql))
                {
                    db.AddInParameter(dm, "@name", DbType.AnsiString, dbTableName);
                    dt = db.ExecuteDataTable(dm);
                }

                if (dt != null || dt.Rows.Count > 0)
                {
                    foreach (string key in dicDbColumnName.Keys)
                    {
                        var rows = dt.Select(" ColumnName = '" + dicDbColumnName[key] + "' ");
                        if (rows != null || rows.Length > 0)
                        {
                            dicDbColumnType.Add(key, SqlType.GetDbTypeFromSqlType(rows[0]["ColumnType"].ToString()));
                            if (rows[0]["is_identity"].ToString() != "0")
                                dicDbIdentity.Add(key, dicDbColumnName[key]);
                        }
                    }
                }


                // DbPrimaryKeys
                sql = @"SELECT d.name as ColumnName FROM sys.indexes a
                    INNER JOIN sys.tables b ON a.object_id = b.object_id
                    INNER JOIN sys.index_columns c ON c.object_id = b.object_id
                    INNER JOIN sys.columns d ON d.column_id = c.column_id and d.object_id = b.object_id
                    WHERE b.name = @name AND a.is_primary_key = '1' ";

                using (DbCommand dm = db.GetSqlStringCommand(sql))
                {
                    db.AddInParameter(dm, "@name", DbType.AnsiString, dbTableName);
                    dt = db.ExecuteDataTable(dm);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (string key in dicDbColumnName.Keys)
                    {
                        var rows = dt.Select(" ColumnName = '" + dicDbColumnName[key] + "' ");
                        if (rows != null && rows.Length > 0)
                            dicDbPrimaryKey.Add(key, dicDbColumnName[key]);
                    }
                }

                buffer = new EntityMappingEntity()
                {
                    EntityTypeName = typeName,
                    EntityFieldNames = listFieldName,
                    EntityPropertyInfoMapping = dicPropertyInfo,
                    DbTableName = dbTableName,
                    DbColumnNameMapping = dicDbColumnName,
                    DbColumnTypeMapping = dicDbColumnType,
                    DbIdentityMapping = dicDbIdentity,
                    DbPrimaryKeyMapping = dicDbPrimaryKey,
                };

                _dbMappingEntityBuffer.Add(buffer);

                return buffer;
            }
        }

        public static string GetDbTableName(Type entityType)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null)
                return null;

            return buffer.DbTableName;
        }

        public static List<string> GetEntityFieldNames(Type entityType)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.EntityFieldNames == null)
                return null;

            return buffer.EntityFieldNames;
        }


        public static List<PropertyInfo> GetEntityPropertyInfos(Type entityType)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.EntityPropertyInfoMapping == null)
                return null;

            return buffer.EntityPropertyInfoMapping.Values.ToList();
        }
        public static List<PropertyInfo> GetEntityPropertyInfos(Type entityType, List<string> entityFieldNames)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.EntityPropertyInfoMapping == null || entityFieldNames == null)
                return null;

            var entityPropertyInfos = new List<PropertyInfo>(entityFieldNames.Count);
            foreach (string fieldName in entityFieldNames)
            {
                entityPropertyInfos.Add(buffer.EntityPropertyInfoMapping[fieldName]);
            }
            return entityPropertyInfos;
        }


        public static string GetDbColumnName(Type entityType, string entityFieldName)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbColumnNameMapping == null || string.IsNullOrEmpty(entityFieldName))
                return null;

            return buffer.DbColumnNameMapping[entityFieldName];
        }
        public static List<string> GetDbColumnNames(Type entityType)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbColumnNameMapping == null)
                return null;

            return buffer.DbColumnNameMapping.Values.ToList();
        }
        public static List<string> GetDbColumnNames(Type entityType, List<string> entityFieldNames)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbColumnNameMapping == null || entityFieldNames == null)
                return null;

            var dbColumnNames = new List<string>(entityFieldNames.Count);
            foreach (string fieldName in entityFieldNames)
            {
                dbColumnNames.Add(buffer.DbColumnNameMapping[fieldName]);
            }
            return dbColumnNames;
        }


        public static DbType GetDbColumnType(Type entityType, string entityFieldName)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbColumnTypeMapping == null || string.IsNullOrEmpty(entityFieldName))
                return DbType.AnsiString;

            return buffer.DbColumnTypeMapping[entityFieldName];
        }
        public static List<DbType> GetDbColumnTypes(Type entityType, List<string> entityFieldNames)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbColumnTypeMapping == null || entityFieldNames == null)
                return null;

            var dbColumnTypes = new List<DbType>(entityFieldNames.Count);
            foreach (string fieldName in entityFieldNames)
            {
                dbColumnTypes.Add(buffer.DbColumnTypeMapping[fieldName]);
            }
            return dbColumnTypes;
        }


        public static List<string> GetPrimaryKeyOfEntityField(Type entityType)
        {
            var buffer = GetDbTableMappingEntity(entityType);
            if (buffer == null || buffer.DbPrimaryKeyMapping == null)
                return null;

            return buffer.DbPrimaryKeyMapping.Keys.ToList();
        }


        public static bool HasPrimaryKey(Type entityType)
        {
            var list = GetPrimaryKeyOfEntityField(entityType);
            return list != null && list.Count > 0;
        }


    }
}
