using System;
using System.Collections.Generic;
using System.Data;

namespace Stonefw.Utility
{
    public class EnumHelper
    {
        // 注意：Enum 的 GetName() 方法等同于枚举本身的 ToString() 方法，非特殊情况尽量用枚举本身的 ToString()

        public delegate string GetDescriptionDelegate<in T>(T enumValue);

        /// <summary>
        /// 把枚举转换成字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary<T>()
        {
            var enumType = typeof (T);
            var dic = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(enumType))
            {
                dic.Add(value, Enum.GetName(enumType, value));
            }
            return dic;
        }

        /// <summary>
        /// 把枚举转换成表格
        /// </summary>
        /// <returns></returns>
        public static DataTable ToDataTable<T>()
        {
            var enumType = typeof (T);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("value"));
            dt.Columns.Add(new DataColumn("name"));
            foreach (int value in Enum.GetValues(enumType))
            {
                var dr = dt.NewRow();
                dr["value"] = value;
                dr["name"] = Enum.GetName(enumType, value);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 把枚举转换成表格
        /// </summary>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(GetDescriptionDelegate<T> getDescription)
        {
            var enumType = typeof (T);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("value"));
            dt.Columns.Add(new DataColumn("name"));
            dt.Columns.Add(new DataColumn("description"));
            foreach (int value in Enum.GetValues(enumType))
            {
                var dr = dt.NewRow();
                dr["value"] = value;
                dr["name"] = Enum.GetName(enumType, value);
                dr["description"] = getDescription((T) Enum.Parse(enumType, value.ToString()));
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}