using System;

namespace stonefw.Utility.EntityExpressions2.Entity
{
    /// <summary>
    /// 语法树异常
    /// </summary>
    [Serializable]
    public class EntityExpressionsException : Exception
    {
        public EntityExpressionsException()
        {
        }

        public EntityExpressionsException(string msg)
            : base(msg)
        {
            ExpressionsMessage = msg;
        }

        /// <summary>
        /// 语法树相关的异常信息
        /// </summary>
        public string ExpressionsMessage
        {
            get;
            set;
        }
    }
}
