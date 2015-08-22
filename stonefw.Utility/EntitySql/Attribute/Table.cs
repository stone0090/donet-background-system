using System;

namespace stonefw.Utility.EntitySql.Attribute
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
