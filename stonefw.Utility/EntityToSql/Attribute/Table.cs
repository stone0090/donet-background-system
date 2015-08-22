using System;

namespace stonefw.Utility.EntityToSql.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class Table : System.Attribute
    {
        public Table(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; set; }        
    }
}
