using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace stonefw.Utility.EntityExpressions2.Entity
{
    /// <summary>
    /// 支持泛型的查询条件
    /// </summary>
    public class GenericWhereEntity<W>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenericWhereEntity()
        {
            _WhereExpressions = new List<Expression>();
            _EntityType = typeof(W);
            _GUID = new Guid().ToString();
            _TableNameDeclareIndex = 0;
            _TableNameDeclare = "T" + _TableNameDeclareIndex.ToString().PadLeft(2, '0');
        }

        private string _DBProviderName = "System.Data.SqlClient";
        /// <summary>
        /// 数据库提供器
        /// </summary>
        public string DBProviderName
        {
            get
            {
                return _DBProviderName;
            }
            set
            {
                _DBProviderName = value;
            }
        }

        private string _GUID = null;
        /// <summary>
        /// GUID标识符
        /// </summary>
        public string GUID
        {
            get
            {
                return _GUID;
            }
        }

        private List<Expression> _WhereExpressions = null;
        /// <summary>
        /// 条件表达式
        /// </summary>
        public List<Expression> WhereExpressions
        {
            get
            {
                return _WhereExpressions;
            }
        }

        private Type _EntityType = null;
        /// <summary>
        /// 实体的类型
        /// </summary>
        public Type EntityType
        {
            get
            {
                return _EntityType;
            }
        }

        private string _WhereCondition = null;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string WhereCondition
        {
            get
            {
                return _WhereCondition;
            }
            set
            {
                _WhereCondition = value;
            }
        }

        private int _TableNameDeclareIndex = 0;
        internal int TableNameDeclareIndex
        {
            get
            {
                return _TableNameDeclareIndex;
            }
        }

        private string _TableNameDeclare = null;
        /// <summary>
        /// T-SQL中对数据表的声明（包含连接的表）
        /// </summary>
        public string TableNameDeclare
        {
            get
            {
                return _TableNameDeclare;
            }
        }

        private bool _DisableTableAlias = false;
        /// <summary>
        /// 是否禁用表的别名
        /// </summary>
        public bool DisableTableAlias
        {
            get
            {
                return _DisableTableAlias;
            }
            set
            {
                _DisableTableAlias = value;
            }
        }

        private List<string> _WhereParameterNames = new List<string>(8);
        /// <summary>
        /// 查询条件参数列表
        /// </summary>
        public List<string> WhereParameterNames
        {
            get
            {
                return _WhereParameterNames;
            }
        }

        private List<object> _WhereParameterValues = new List<object>(8);
        /// <summary>
        /// 查询条件参数值
        /// </summary>
        public List<object> WhereParameterValues
        {
            get
            {
                return _WhereParameterValues;
            }
        }

        private List<DbType> _WhereParameterTypes = new List<DbType>(8);
        /// <summary>
        /// 查询条件类型
        /// </summary>
        public List<DbType> WhereParameterTypes
        {
            get
            {
                return _WhereParameterTypes;
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="predicate"></param>
        public void Where(Expression<Func<W, bool>> conditionExpression)
        {
            if (conditionExpression == null || conditionExpression.Body == null)
                return;
            _WhereExpressions.Add(conditionExpression.Body);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="predicate"></param>
        public void Where(Expression conditionExpression)
        {
            if (conditionExpression == null)
                return;
            _WhereExpressions.Add(conditionExpression);
        }

        /// <summary>
        /// 重置表的别名
        /// </summary>
        /// <param name="tableNameDeclareIndex"></param>
        public void ResetTableNameDeclare(int tableNameDeclareIndex)
        {
            _TableNameDeclareIndex = tableNameDeclareIndex;
            _TableNameDeclare = "T" + _TableNameDeclareIndex.ToString().PadLeft(2, '0');
        }
    }
}