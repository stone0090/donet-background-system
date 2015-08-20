using System;

namespace stonefw.Utility.EntityExpression.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class Table : System.Attribute
    {
        public Table(string strTableName)
        {
            this.TableName = strTableName;
        }

        public string TableName { get; set; }

        //public string TableAliasName { get; set; }

        //public bool IsStoreProcedureName { get; set; }
    }
}
