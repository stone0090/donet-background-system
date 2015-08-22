using System;

namespace stonefw.Utility.EntityToSql.Entity
{
    /// <summary>
    /// 语法树异常
    /// </summary>
    [Serializable]
    public class EntityToSqlException : Exception
    {
        public EntityToSqlException()
        {
        }
        public EntityToSqlException(string msg)
            : base(msg)
        {
        }
    }
}
