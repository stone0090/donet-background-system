namespace stonefw.Utility.EntityToSql.Data
{
    /// <summary>
    /// 访问成员的委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="arg"></param>
    /// <returns></returns>
    public delegate TResult VisitMember<T, TResult>(T arg);
}
