using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace stonefw.Utility.EntityToSql.Entity
{
    /// <summary>
    /// 实体类定义的缓存
    /// </summary>
    internal class DbTableMappingEntity
    {
        /// <summary>
        /// 实体的类型名称
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// 对应的数据表名称
        /// </summary>
        public string DbTableName { get; set; }

        /// <summary>
        /// 实体的字段名称的集合
        /// </summary>
        public List<string> EntityFieldNames { get; set; }

        /// <summary>
        /// 实体的属性的集合的映射(key为EntityFieldName)
        /// </summary>
        public Dictionary<string, PropertyInfo> EntityPropertyInfoMapping { get; set; }

        /// <summary>
        /// 数据库字段名和实体类成员的映射(key为EntityFieldName)
        /// </summary>
        public Dictionary<string, string> DbColumnNameMapping { get; set; }

        /// <summary>
        /// 数据库字段类型和实体类成员的映射(key为成员名称)
        /// </summary>
        public Dictionary<string, DbType> DbColumnTypeMapping { get; set; }

        /// <summary>
        /// 数据库字段和标识字段的映射(key为EntityFieldName)
        /// </summary>
        public Dictionary<string, string> DbIdentityMapping { get; set; }

        /// <summary>
        /// 数据库字段和主键字段的映射(key为EntityFieldName)
        /// </summary>
        public Dictionary<string, string> DbPrimaryKeyMapping { get; set; }
    }
}
