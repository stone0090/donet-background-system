using System;

namespace Stonefw.Utility.EntitySql.Attribute
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