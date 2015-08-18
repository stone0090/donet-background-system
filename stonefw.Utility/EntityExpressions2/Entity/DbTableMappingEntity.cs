using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace stonefw.Utility.EntityExpressions2.Entity
{
    /// <summary>
    /// 实体类定义的缓存
    /// </summary>
    internal class DbTableMappingEntity
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 对应的数据表名称
        /// </summary>
        public string DbTableName { get; set; }

        /// <summary>
        /// 实体的有效成员的名称的集合
        /// </summary>
        public string[] EntityEffectiveMemberNames { get; set; }

        /// <summary>
        /// 实体的有效成员的属性的集合(key为成员名称)
        /// </summary>
        public Dictionary<string, PropertyInfo> EntityEffectivePropertyInfos { get; set; }

        /// <summary>
        /// 数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        public Dictionary<string, string> DbColumnNameMappings { get; set; }

        /// <summary>
        /// 数据库字段名和实体类成员的反向映射(key为数据库字段名)
        /// </summary>
        public Dictionary<string, string> DbColumnNameReverseMappings { get; set; }

        /// <summary>
        /// 数据库字段类型和实体类成员的映射(key为成员名称)
        /// </summary>
        public Dictionary<string, DbType> DbColumnTypeMappings { get; set; }

        /// <summary>
        /// 标识列对应的实体类成员
        /// </summary>
        public string EntityMemberOfIdentityColumn { get; set; }

        /// <summary>
        /// 标识字段的名称
        /// </summary>
        public string IdentityColumnName { get; set; }

        /// <summary>
        /// 主键相关的实体类成员
        /// </summary>
        public List<string> EntityMembersOfPrimaryKey { get; set; }

        /// <summary>
        /// 主键相关的字段名称
        /// </summary>
        public List<string> ColumnNamesOfPrimaryKey { get; set; }
    }
}
