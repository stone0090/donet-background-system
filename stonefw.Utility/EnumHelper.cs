using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace stonefw.Utility
{
    public class EnumHelper
    {
        public delegate string GetNameDelegate<in T>(T name);

        /// <summary>
        /// 把枚举转换成字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> EnumToDictionary<T>()
        {
            var enumType = typeof(T);
            var dic = new Dictionary<string, string>();
            foreach (int value in Enum.GetValues(enumType))
            {
                string val = value.ToString();
                string text = Enum.GetName(enumType, value);
                dic.Add(val, text);
            }
            return dic;
        }

        /// <summary>
        /// 把枚举转换成表格
        /// </summary>
        /// <returns></returns>
        public static DataTable EnumToDataTable<T>()
        {
            var enumType = typeof(T);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("value"));
            dt.Columns.Add(new DataColumn("text"));
            foreach (int value in Enum.GetValues(enumType))
            {
                string val = value.ToString(CultureInfo.InvariantCulture);
                string text = Enum.GetName(enumType, value);
                var dr = dt.NewRow();
                dr["value"] = val;
                dr["text"] = text;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 把枚举转换成表格
        /// </summary>
        /// <returns></returns>
        public static DataTable EnumToDataTable<T>(GetNameDelegate<T> getNameDelegate)
        {
            var enumType = typeof(T);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("value"));
            dt.Columns.Add(new DataColumn("text"));
            dt.Columns.Add(new DataColumn("name"));
            foreach (int value in Enum.GetValues(enumType))
            {
                string val = value.ToString(CultureInfo.InvariantCulture);
                string text = Enum.GetName(enumType, value);
                string name = getNameDelegate(Str2Enum<T>(val));
                var dr = dt.NewRow();
                dr["value"] = val;
                dr["text"] = text;
                dr["name"] = name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 把枚举转换成表格
        /// </summary>
        /// <returns></returns>
        public static DataTable EnumToDataTable<T>(GetNameDelegate<T> getNameDelegate, string[] enumValues)
        {
            var enumType = typeof(T);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("value"));
            dt.Columns.Add(new DataColumn("text"));
            dt.Columns.Add(new DataColumn("name"));
            foreach (int value in Enum.GetValues(enumType))
            {
                string val = value.ToString(CultureInfo.InvariantCulture);
                if (!enumValues.ToList().Contains(val)) continue;
                string text = Enum.GetName(enumType, value);
                string name = getNameDelegate(Str2Enum<T>(val));
                var dr = dt.NewRow();
                dr["value"] = val;
                dr["text"] = text;
                dr["name"] = name;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static T Str2Enum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static string Enum2Str<T>(T t)
        {
            return Enum.GetName(typeof(T), t);
        }

    }
}
