using System;
using System.Data;

namespace stonefw.Utility.EntityToSql.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Field : System.Attribute
    {
        public Field(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; set; }
    }
}
