using System.Data.Common;
using Stonefw.Utility;

namespace Stonefw.CodeGenerate.SqlServer
{
    internal static class SqlServerTools
    {
        public static SqlServerVersions GetServerVersion(string dbName)
        {
            const string strSql = "SELECT @@version as version";
            string str2 = null;
            Database db = string.IsNullOrEmpty(dbName)
                ? DatabaseFactory.CreateDatabase()
                : DatabaseFactory.CreateDatabase(dbName);
            DbCommand sqlStringCommand = db.GetSqlStringCommand(strSql);
            str2 = db.ExecuteScalar(sqlStringCommand).ToString();
            if (str2.Contains("SQL Server  2000"))
            {
                return SqlServerVersions.SqlServer2000;
            }
            if (str2.Contains("SQL Server 2005"))
            {
                return SqlServerVersions.SqlServer2005;
            }
            if (str2.Contains("SQL Server 2008"))
            {
                return SqlServerVersions.SqlServer2008;
            }
            if (str2.Contains("SQL Server 2012"))
            {
                return SqlServerVersions.SqlServer2012;
            }
            return SqlServerVersions.Other;
        }
    }
}