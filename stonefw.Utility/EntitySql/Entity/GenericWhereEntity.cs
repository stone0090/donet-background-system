using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Stonefw.Utility.EntitySql.Entity
{
    /// <summary>
    /// 支持泛型的查询条件
    /// </summary>
    public class GenericWhereEntity<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenericWhereEntity()
        {
            this.Guid = new Guid().ToString();
            this.DbProviderName = "System.Data.SqlClient";
            this.EntityType = typeof (T);
            this.WhereExpressions = new List<Expression>();
            this.WhereParameterNames = new List<string>(8);
            this.WhereParameterValues = new List<object>(8);
            this.WhereParameterTypes = new List<DbType>(8);

            this._tableName = "T" + TableNameIndex.ToString().PadLeft(2, '0');
            this._tableNameIndex = 0;
        }

        private int _tableNameIndex = 0;

        public int TableNameIndex
        {
            get { return _tableNameIndex; }
        }

        private string _tableName = string.Empty;

        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// 数据库提供器
        /// </summary>
        public string DbProviderName { get; set; }

        /// <summary>
        /// Guid标识符
        /// </summary>
        public string Guid { get; }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public List<Expression> WhereExpressions { get; }

        /// <summary>
        /// 实体的类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string WhereCondition { get; set; }

        /// <summary>
        /// 是否禁用表的别名
        /// </summary>
        public bool DisableTableAlias { get; set; }

        /// <summary>
        /// 查询条件参数列表
        /// </summary>
        public List<string> WhereParameterNames { get; }

        /// <summary>
        /// 查询条件参数值
        /// </summary>
        public List<object> WhereParameterValues { get; }

        /// <summary>
        /// 查询条件类型
        /// </summary>
        public List<DbType> WhereParameterTypes { get; }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="predicate"></param>
        public void Where(Expression<Func<T, bool>> conditionExpression)
        {
            if (conditionExpression == null || conditionExpression.Body == null)
                return;
            this.WhereExpressions.Add(conditionExpression.Body);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="predicate"></param>
        public void Where(Expression conditionExpression)
        {
            if (conditionExpression == null)
                return;
            this.WhereExpressions.Add(conditionExpression);
        }

        /// <summary>
        /// 重置表的别名
        /// </summary>
        /// <param name="tableNameIndex"></param>
        public void ResetTableName(int tableNameIndex)
        {
            this._tableNameIndex = tableNameIndex;
            this._tableName = "T" + this._tableNameIndex.ToString().PadLeft(2, '0');
        }
    }
}