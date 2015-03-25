using System;
using System.Data;

namespace stonefw.Utility.EntityExpressions.Attribute
{
    /// <summary>
    /// FieldAttribute 的摘要说明。
    /// 加在实体类上的属性使用的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Field : System.Attribute
    {
        public bool Ignore
        {
            get { return _Ignore; }
            set { _Ignore = value; }
        }
        private bool _Ignore = false;

        /// <summary>
        /// 是否为索引键
        /// </summary>
        public bool IsIndexKey
        {
            get { return _IsIndexKey; }
            set { _IsIndexKey = value; }
        }
        private bool _IsIndexKey = false;

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return _IsPrimaryKey; }
            set { _IsPrimaryKey = value; }
        }
        private bool _IsPrimaryKey = false;

        /// <summary>
        /// 是否为标识列
        /// </summary>
        public bool IsIdentityField
        {
            get { return _IsIdentityField; }
            set { _IsIdentityField = value; }
        }
        private bool _IsIdentityField = false;

        public string FieldDesc
        {
            get { return _FieldDesc; }
            set { _FieldDesc = value; }
        }
        private string _FieldDesc = "";

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        private string _FieldName = "";

        /// <summary>
        /// 字段对应的数据库列的数据库字段类型
        /// </summary>
        public DbType FieldDBType
        {
            set
            {
                this.m_strFieldType = value;
            }
            get
            {
                return this.m_strFieldType;
            }
        }
        private DbType m_strFieldType = DbType.AnsiString;


        ///// <summary>
        ///// 字段对应的比较类型(比如等于，大于，小于,LIKE等等，多用于sql条件部分拼装
        ///// </summary>
        //public CompareSignal FieldCompareSignal
        //{
        //    set
        //    {
        //        this.m_FieldCompareSignal = value;
        //    }
        //    get
        //    {
        //        return this.m_FieldCompareSignal;
        //    }
        //}
        //private CompareSignal m_FieldCompareSignal;

        ///// <summary>
        ///// 字段对应的逻辑连接类型,比如and,or等等，多用于sql条件部分拼装
        ///// </summary>
        //public LogicalSignal FieldLogicalConnnectionSignal
        //{
        //    set
        //    {
        //        this.m_FieldLogicalSignal = value;
        //    }
        //    get
        //    {
        //        return this.m_FieldLogicalSignal;
        //    }
        //}
        //private LogicalSignal m_FieldLogicalSignal;


        public Field(string strFieldName)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            this._FieldName = strFieldName;

            //
        }

        public Field(string strFieldName, string strFieldDesc)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            this._FieldName = strFieldName;
            this._FieldDesc = strFieldDesc;

            //
        }

        public Field(string strFieldName, string strFieldDesc, DbType FieldType)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            this._FieldName = strFieldName;
            this._FieldDesc = strFieldDesc;
            this.m_strFieldType = FieldType;

            //
        }

        public Field(string strFieldName, DbType FieldType)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            this._FieldName = strFieldName;
            this.m_strFieldType = FieldType;

            //
        }
    }


}
