using System;
using System.Data;

namespace stonefw.Utility.EntityExpressions2.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Field : System.Attribute
    {
        public Field(string strFieldName)
        {
            this.FieldName = strFieldName;
        }

        public Field(string strFieldName, string strFieldDesc)
        {
            this.FieldName = strFieldName;
            this.FieldDesc = strFieldDesc;
        }

        public Field(string strFieldName, string strFieldDesc, DbType FieldType)
        {
            this.FieldName = strFieldName;
            this.FieldDesc = strFieldDesc;
            this.FieldDbType = FieldType;
        }

        public Field(string strFieldName, DbType FieldType)
        {
            this.FieldName = strFieldName;
            this.FieldDbType = FieldType;
        }

        public string FieldDesc { get; set; }

        public string FieldName { get; set; }

        public DbType FieldDbType { get; set; }

        public bool IsIndexKey { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsIdentityField { get; set; }

        public bool Ignore { get; set; }

    }
}
