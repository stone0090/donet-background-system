using System;

namespace stonefw.Utility.EntityExpression.Entity
{
    /// <summary>
    /// 实体对,用于部分连接操作的Entity化
    /// </summary>
    /// <typeparam name="EA"></typeparam>
    /// <typeparam name="EB"></typeparam>
    [Serializable]
    public class EntityPairs<EA, EB>
        where EA : class, new()
        where EB : class, new()
    {
        public EA EntityA
        {
            get;
            set;
        }

        public EB EntityB
        {
            get;
            set;
        }
    }
}
