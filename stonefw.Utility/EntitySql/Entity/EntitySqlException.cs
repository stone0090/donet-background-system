using System;

namespace stonefw.Utility.EntitySql.Entity
{
    /// <summary>
    /// 语法树异常
    /// </summary>
    [Serializable]
    public class EntitySqlException : Exception
    {
        public EntitySqlException()
        {
        }
        public EntitySqlException(string msg)
            : base(msg)
        {
        }
    }
}
