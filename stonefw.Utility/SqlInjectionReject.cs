using System.Linq;
using System.Text.RegularExpressions;

namespace Stonefw.Utility
{
    public static class SqlInjectionReject
    {
        /// <summary>
        /// T-SQL关键字
        /// </summary>
        private static readonly string[] TsqlKeyWords = new string[]
        {
            "ADD", "EXCEPT", "PERCENT", "ALL", "EXEC", "PLAN", "ALTER", "EXECUTE", "PRECISION", "AND", "EXISTS",
            "PRIMARY", "ANY", "EXIT",
            "PRINT", "AS", "FETCH", "PROC", "ASC", "FILE", "PROCEDURE", "AUTHORIZATION", "FILLFACTOR", "PUBLIC",
            "BACKUP", "FOR", "RAISERROR",
            "BEGIN", "FOREIGN", "READ", "BETWEEN", "FREETEXT", "READTEXT", "BREAK", "FREETEXTTABLE", "RECONFIGURE",
            "BROWSE", "FROM",
            "REFERENCES", "BULK", "FULL", "REPLICATION", "BY", "FUNCTION", "RESTORE", "CASCADE", "GOTO", "RESTRICT",
            "CASE", "GRANT", "RETURN",
            "CHECK", "GROUP", "REVOKE", "CHECKPOINT", "HAVING", "RIGHT", "CLOSE", "HOLDLOCK", "ROLLBACK", "CLUSTERED",
            "IDENTITY", "ROWCOUNT",
            "COALESCE", "IDENTITY_INSERT", "ROWGUIDCOL", "COLLATE", "IDENTITYCOL", "RULE", "COLUMN", "IF", "SAVE",
            "COMMIT", "IN", "SCHEMA",
            "COMPUTE", "INDEX", "SELECT", "CONSTRAINT", "INNER", "SESSION_USER", "CONTAINS", "INSERT", "SET",
            "CONTAINSTABLE", "INTERSECT",
            "SETUSER", "CONTINUE", "INTO", "SHUTDOWN", "CONVERT", "IS", "SOME", "CREATE", "JOIN", "STATISTICS", "CROSS",
            "KEY", "SYSTEM_USER",
            "CURRENT", "KILL", "TABLE", "CURRENT_DATE", "LEFT", "TEXTSIZE", "CURRENT_TIME", "LIKE", "THEN",
            "CURRENT_TIMESTAMP", "LINENO",
            "TO", "CURRENT_USER", "LOAD", "TOP", "CURSOR", "NATIONAL", "TRAN", "DATABASE", "NOCHECK", "TRANSACTION",
            "DBCC", "NONCLUSTERED",
            "TRIGGER", "DEALLOCATE", "NOT", "TRUNCATE", "DECLARE", "NULL", "TSEQUAL", "DEFAULT", "NULLIF", "UNION",
            "DELETE", "OF", "UNIQUE",
            "DENY", "OFF", "UPDATE", "DESC", "OFFSETS", "UPDATETEXT", "DISK", "ON", "USE", "DISTINCT", "OPEN", "USER",
            "DISTRIBUTED", "OPENDATASOURCE",
            "VALUES", "DOUBLE", "OPENQUERY", "VARYING", "DROP", "OPENROWSET", "VIEW", "DUMMY", "OPENXML", "WAITFOR",
            "DUMP", "OPTION", "WHEN",
            "ELSE", "OR", "WHERE", "END", "ORDER", "WHILE", "ERRLVL", "OUTER", "WITH", "ESCAPE", "OVER", "WRITETEXT"
        };

        /// <summary>
        /// ODBC关键字
        /// </summary>
        private static readonly string[] OdbcKeyWords = new string[]
        {
            "ABSOLUTE", "EXEC", "OVERLAPS", "ACTION", "EXECUTE", "PAD", "ADA", "EXISTS", "PARTIAL", "ADD", "EXTERNAL",
            "PASCAL", "ALL", "EXTRACT", "POSITION",
            "ALLOCATE", "FALSE", "PRECISION", "ALTER", "FETCH", "PREPARE", "AND", "FIRST", "PRESERVE", "ANY", "FLOAT",
            "PRIMARY", "ARE", "FOR", "PRIOR", "AS",
            "FOREIGN", "PRIVILEGES", "ASC", "FORTRAN", "PROCEDURE", "ASSERTION", "FOUND", "PUBLIC", "AT", "FROM", "READ",
            "AUTHORIZATION", "FULL", "REAL",
            "AVG", "GET", "REFERENCES", "BEGIN", "GLOBAL", "RELATIVE", "BETWEEN", "GO", "RESTRICT", "BIT", "GOTO",
            "REVOKE", "BIT_LENGTH", "GRANT", "RIGHT",
            "BOTH", "GROUP", "ROLLBACK", "BY", "HAVING", "ROWS", "CASCADE", "HOUR", "SCHEMA", "CASCADED", "IDENTITY",
            "SCROLL", "CASE", "IMMEDIATE", "SECOND",
            "CAST", "IN", "SECTION", "CATALOG", "INCLUDE", "SELECT", "CHAR", "INDEX", "SESSION", "CHAR_LENGTH",
            "INDICATOR", "SESSION_USER", "CHARACTER",
            "INITIALLY", "SET", "CHARACTER_LENGTH", "INNER", "SIZE", "CHECK", "INPUT", "SMALLINT", "CLOSE",
            "INSENSITIVE", "SOME", "COALESCE", "INSERT", "SPACE",
            "COLLATE", "INT", "SQL", "COLLATION", "INTEGER", "SQLCA", "COLUMN", "INTERSECT", "SQLCODE", "COMMIT",
            "INTERVAL", "SQLERROR", "CONNECT",
            "INTO", "SQLSTATE", "CONNECTION", "IS", "SQLWARNING", "CONSTRAINT", "ISOLATION", "SUBSTRING", "CONSTRAINTS",
            "JOIN", "SUM", "CONTINUE",
            "KEY", "SYSTEM_USER", "CONVERT", "LANGUAGE", "TABLE", "CORRESPONDING", "LAST", "TEMPORARY", "COUNT",
            "LEADING", "THEN", "CREATE", "LEFT",
            "TIME", "CROSS", "LEVEL", "TIMESTAMP", "CURRENT", "LIKE", "TIMEZONE_HOUR", "CURRENT_DATE", "LOCAL",
            "TIMEZONE_MINUTE", "CURRENT_TIME",
            "LOWER", "TO", "CURRENT_TIMESTAMP", "MATCH", "TRAILING", "CURRENT_USER", "MAX", "TRANSACTION", "CURSOR",
            "MIN", "TRANSLATE", "DATE",
            "MINUTE", "TRANSLATION", "DAY", "MODULE", "TRIM", "DEALLOCATE", "MONTH", "TRUE", "DEC", "NAMES", "UNION",
            "DECIMAL", "NATIONAL", "UNIQUE",
            "DECLARE", "NATURAL", "UNKNOWN", "DEFAULT", "NCHAR", "UPDATE", "DEFERRABLE", "NEXT", "UPPER", "DEFERRED",
            "NO", "USAGE", "DELETE", "NONE",
            "USER", "DESC", "NOT", "USING", "DESCRIBE", "NULL", "VALUE", "DESCRIPTOR", "NULLIF", "VALUES", "DIAGNOSTICS",
            "NUMERIC", "VARCHAR", "DISCONNECT",
            "OCTET_LENGTH", "VARYING", "DISTINCT", "OF", "VIEW", "DOMAIN", "ON", "WHEN", "DOUBLE", "ONLY", "WHENEVER",
            "DROP", "OPEN", "WHERE", "ELSE",
            "OPTION", "WITH", "END", "OR", "WORK", "END-EXEC", "ORDER", "WRITE", "ESCAPE", "OUTER", "YEAR", "EXCEPT",
            "OUTPUT", "ZONE", "EXCEPTION"
        };

        /// <summary>
        /// 特殊字符
        /// </summary>
        private static readonly string[] SpecialWords = new string[]
        {"'", "[", "]", "\\", "%", "_", ";", "/", "*", "-", "--", "=", ">", "<", "<>", "!=", "/*", "*/", "\n"};

        /// <summary>
        /// 时间值
        /// </summary>
        private static readonly Regex DateTimeRule =
            new Regex(
                @"^(\d{2,4}-\d{1,2}-\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}:\d{1,2})$");

        /// <summary>
        /// 部分时间值
        /// </summary>
        private static readonly Regex PartDateTimeRule =
            new Regex(
                @"(\d{2,4}-\d{1,2}-\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}:\d{1,2})");

        /// <summary>
        /// 对数字 字符串 日期 txt xml image类型的参数进行校验，校验不通过就返回
        /// </summary>
        /// <param name="sqlParameter"></param>
        /// <returns>校验不通过就返回false</returns>
        public static bool CheckMssqlParameter(string sqlParameter)
        {
            sqlParameter = sqlParameter.ToUpper();

            //常用场景的特殊处理
            if (sqlParameter == "-1")
                return true;
            if (DateTimeRule.IsMatch(sqlParameter))
                return true;
            //如果内容中含有时间字符串，则需要移除
            var matchs = PartDateTimeRule.Matches(sqlParameter);
            if (matchs.Count > 0)
            {
                for (int i = 0; i < matchs.Count; i++)
                {
                    sqlParameter = sqlParameter.Replace(matchs[i].Groups[1].Value, "");
                }
            }

            //首先检查是否包含特殊字符
            foreach (string checkString in SpecialWords)
            {
                if (sqlParameter.Contains(checkString))
                {
                    return false;
                }
            }
            //检测其他关键字和字符

            string[] check = sqlParameter.Split(' ').ToArray();
            bool result = true;
            foreach (string checkString in check)
            {
                if (TsqlKeyWords.Contains(checkString))
                {
                    result = false;
                    break;
                }
                else if (OdbcKeyWords.Contains(checkString))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}