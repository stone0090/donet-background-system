namespace Stonefw.Biz.SystemModule
{
    public static class SysEnumNameExBiz
    {
        public static string GetDescription<T>(this T enumValue)
        {
            var enumType = typeof (T);
            var enumName = enumValue.ToString();
            var entity = new SysEnumNameBiz().GetSingleSysEnumName(enumType.Name, enumName);
            if (entity != null && !string.IsNullOrEmpty(entity.Name))
                return entity.Name;
            return enumName;
        }

        public static string GetDescription<T>(this string enumName)
        {
            var enumType = typeof (T);
            var entity = new SysEnumNameBiz().GetSingleSysEnumName(enumType.Name, enumName);
            if (entity != null && !string.IsNullOrEmpty(entity.Name))
                return entity.Name;
            return enumName;
        }
    }
}