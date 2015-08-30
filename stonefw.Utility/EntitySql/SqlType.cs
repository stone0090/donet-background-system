using System.Data;

namespace Stonefw.Utility.EntitySql
{
    public class SqlType
    {
        /// <summary>
        /// 根据数据库字段类型获取默认的ObjectType类型
        /// </summary>
        public static string GetObjectTypeFromSqlType(string sqlType)
        {
            string retType = "string";
            if (string.IsNullOrEmpty(sqlType))
                return retType;

            sqlType = sqlType.ToLower();

            if (sqlType.Contains("char"))
                return retType;

            if (sqlType.Contains("text"))
                return retType;

            switch (sqlType)
            {
                case "bigint":
                    retType = "long";
                    break;
                case "bit":
                    retType = "bool";
                    break;
                case "date":
                    retType = "DateTime";
                    break;
                case "datetime":
                    retType = "DateTime";
                    break;
                case "datetime2":
                    retType = "DateTime";
                    break;
                case "decimal":
                    retType = "decimal";
                    break;
                case "float":
                    retType = "double";
                    break;
                case "int":
                    retType = "int";
                    break;
                case "money":
                    retType = "decimal";
                    break;
                case "numeric":
                    retType = "decimal";
                    break;
                case "smalldatetime":
                    retType = "DateTime";
                    break;
                case "smallint":
                    retType = "short";
                    break;
                case "smallmoney":
                    retType = "decimal";
                    break;
                case "tinyint":
                    retType = "byte";
                    break;
                default:
                    break;
            }

            return retType;
        }

        /// <summary>
        /// 根据数据库字段类型获取默认的DbType类型
        /// </summary>
        public static DbType GetDbTypeFromSqlType(string sqlType)
        {
            DbType retType = DbType.AnsiString;
            if (string.IsNullOrEmpty(sqlType))
                return retType;

            sqlType = sqlType.ToLower();

            if (sqlType.Contains("char"))
                return retType;

            if (sqlType.Contains("text"))
                return retType;

            switch (sqlType)
            {
                case "bigint":
                    retType = DbType.Int64;
                    break;
                case "bit":
                    retType = DbType.Boolean;
                    break;
                case "date":
                    retType = DbType.DateTime;
                    break;
                case "datetime":
                    retType = DbType.DateTime;
                    break;
                case "datetime2":
                    retType = DbType.DateTime;
                    break;
                case "decimal":
                    retType = DbType.Decimal;
                    break;
                case "float":
                    retType = DbType.Double;
                    break;
                case "int":
                    retType = DbType.Int32;
                    break;
                case "money":
                    retType = DbType.Decimal;
                    break;
                case "numeric":
                    retType = DbType.Decimal;
                    break;
                case "smalldatetime":
                    retType = DbType.DateTime;
                    break;
                case "smallint":
                    retType = DbType.Int16;
                    break;
                case "smallmoney":
                    retType = DbType.Decimal;
                    break;
                case "tinyint":
                    retType = DbType.Byte;
                    break;
                default:
                    break;
            }

            return retType;
        }

        /// <summary>
        /// 根据数据库字段类型获取默认的DbType类型
        /// </summary>
        public static string GetDbTypeFromSqlType2(string sqlType)
        {
            string retType = "DbType.AnsiString";
            if (string.IsNullOrEmpty(sqlType))
                return retType;

            sqlType = sqlType.ToLower();

            if (sqlType.Contains("char"))
                return retType;

            if (sqlType.Contains("text"))
                return retType;

            switch (sqlType)
            {
                case "bigint":
                    retType = "DbType.Int64";
                    break;
                case "bit":
                    retType = "DbType.Boolean";
                    break;
                case "date":
                    retType = "DbType.DateTime";
                    break;
                case "datetime":
                    retType = "DbType.DateTime";
                    break;
                case "datetime2":
                    retType = "DbType.DateTime";
                    break;
                case "decimal":
                    retType = "DbType.Decimal";
                    break;
                case "float":
                    retType = "DbType.Double";
                    break;
                case "int":
                    retType = "DbType.Int32";
                    break;
                case "money":
                    retType = "DbType.Decimal";
                    break;
                case "numeric":
                    retType = "DbType.Decimal";
                    break;
                case "smalldatetime":
                    retType = "DbType.DateTime";
                    break;
                case "smallint":
                    retType = "DbType.Int16";
                    break;
                case "smallmoney":
                    retType = "DbType.Decimal";
                    break;
                case "tinyint":
                    retType = "DbType.Byte";
                    break;
                default:
                    break;
            }

            return retType;
        }
    }
}