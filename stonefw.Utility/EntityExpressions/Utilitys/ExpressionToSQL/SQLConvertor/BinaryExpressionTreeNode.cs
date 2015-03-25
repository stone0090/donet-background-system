using System.Linq.Expressions;

namespace stonefw.Utility.EntityExpressions.Utilitys.ExpressionToSQL.SQLConvertor
{
    /// <summary>
    /// 二元表达式树的节点
    /// </summary>
    internal class BinaryExpressionTreeNode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exp"></param>
        public BinaryExpressionTreeNode(BinaryExpression exp)
        {
            AboutBinaryExpression = exp;
            BuildChildrenTree();
        }

        /// <summary>
        /// 创建子树
        /// </summary>
        private void BuildChildrenTree()
        {
            BinaryExpression lbe = this.AboutBinaryExpression.Left as BinaryExpression;
            BinaryExpression rbe = this.AboutBinaryExpression.Left as BinaryExpression;

            if (lbe != null)
            {
                this.LeftChildNode = new BinaryExpressionTreeNode(lbe);
                this.LeftChildNode.FatherNode = this;
            }

            if (rbe != null)
            {
                this.RightChildNode = new BinaryExpressionTreeNode(rbe);
                this.RightChildNode.FatherNode = this;
            }
        }

        /// <summary>
        /// 相关的二元表达式
        /// </summary>
        public BinaryExpression AboutBinaryExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public BinaryExpressionTreeNode FatherNode
        {
            get;
            set;
        }

        /// <summary>
        /// 左子树
        /// </summary>
        public BinaryExpressionTreeNode LeftChildNode
        {
            get;
            set;
        }

        /// <summary>
        /// 右子树
        /// </summary>
        public BinaryExpressionTreeNode RightChildNode
        {
            get;
            set;
        }
    }
}
