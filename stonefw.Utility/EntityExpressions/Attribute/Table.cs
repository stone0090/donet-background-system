using System;

namespace stonefw.Utility.EntityExpressions.Attribute
{
    /// <summary>
    /// 标志实体类对应的数据库表的一些属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class Table : System.Attribute
    {
        /// <summary>
        /// 构造函数，创建这个类的实例
        /// </summary>
        public Table(string strTableName)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            this.TableName = strTableName;
            //
        }

        //定义一些表要使用的属性

        /// <summary>
        /// 实体类对应的数据库表的名字
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体类对应的数据库表的别名
        /// </summary>
        public string TableAliasName { get; set; }

        /// <summary>
        /// 是否为存储过程的名称
        /// </summary>
        public bool IsStoreProcedureName { get; set; }
    }
}
