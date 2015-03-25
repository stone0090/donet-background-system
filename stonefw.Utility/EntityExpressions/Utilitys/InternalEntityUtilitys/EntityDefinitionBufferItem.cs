/*
 * First creat by wukea[2013/09/09]
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys
{
    /// <summary>
    /// 实体类定义的缓存
    /// </summary>
    internal class EntityDefinitionBufferItem
    {
        private string _TypeName = null;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                _TypeName = value;
            }
        }

        private string _DBTableName = null;
        /// <summary>
        /// 对应的数据表名称
        /// </summary>
        public string DBTableName
        {
            get
            {
                return _DBTableName;
            }
            set
            {
                _DBTableName = value;
            }
        }

        private string[] _EntityEffectiveMemberNames = null;
        /// <summary>
        /// 实体的有效成员的名称的集合
        /// </summary>
        public string[] EntityEffectiveMemberNames
        {
            get
            {
                return _EntityEffectiveMemberNames;
            }
            set
            {
                _EntityEffectiveMemberNames = value;
            }
        }

        private Dictionary<string, PropertyInfo> _EntityEffectivePropertyInfos = null;
        /// <summary>
        /// 实体的有效成员的属性的集合(key为成员名称)
        /// </summary>
        public Dictionary<string, PropertyInfo> EntityEffectivePropertyInfos
        {
            get
            {
                return _EntityEffectivePropertyInfos;
            }
            set
            {
                _EntityEffectivePropertyInfos = value;
            }
        }

        private Dictionary<string, string> _DBColumnNameMappings = null;
        /// <summary>
        /// 数据库字段名和实体类成员的映射(key为成员名称)
        /// </summary>
        public Dictionary<string, string> DBColumnNameMappings
        {
            get
            {
                return _DBColumnNameMappings;
            }
            set
            {
                _DBColumnNameMappings = value;
            }
        }

        private Dictionary<string, string> _DBColumnNameReverseMappings = null;
        /// <summary>
        /// 数据库字段名和实体类成员的反向映射(key为数据库字段名)
        /// </summary>
        public Dictionary<string, string> DBColumnNameReverseMappings
        {
            get
            {
                return _DBColumnNameReverseMappings;
            }
            set
            {
                _DBColumnNameReverseMappings = value;
            }
        }

        private Dictionary<string, DbType> _DBColumnTypeMappings = null;
        /// <summary>
        /// 数据库字段类型和实体类成员的映射(key为成员名称)
        /// </summary>
        public Dictionary<string, DbType> DBColumnTypeMappings
        {
            get
            {
                return _DBColumnTypeMappings;
            }
            set
            {
                _DBColumnTypeMappings = value;
            }
        }

        private string _EntityMemberOfIdentityColumn = null;
        /// <summary>
        /// 标识列对应的实体类成员
        /// </summary>
        public string EntityMemberOfIdentityColumn
        {
            get
            {
                return _EntityMemberOfIdentityColumn;
            }
            set
            {
                _EntityMemberOfIdentityColumn = value;
            }
        }

        private string _IdentityColumnName = null;
        /// <summary>
        /// 标识字段的名称
        /// </summary>
        public string IdentityColumnName
        {
            get
            {
                return _IdentityColumnName;
            }
            set
            {
                _IdentityColumnName = value;
            }
        }

        private List<string> _EntityMembersOfPrimaryKey = null;
        /// <summary>
        /// 主键相关的实体类成员
        /// </summary>
        public List<string> EntityMembersOfPrimaryKey
        {
            get
            {
                return _EntityMembersOfPrimaryKey;
            }
            set
            {
                _EntityMembersOfPrimaryKey = value;
            }
        }

        private List<string> _ColumnNamesOfPrimaryKey = null;
        /// <summary>
        /// 主键相关的字段名称
        /// </summary>
        public List<string> ColumnNamesOfPrimaryKey
        {
            get
            {
                return _ColumnNamesOfPrimaryKey;
            }
            set
            {
                _ColumnNamesOfPrimaryKey = value;
            }
        }
    }
}
