using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Stonefw.Utility;

namespace Stonefw.CodeGenerate.SqlServer
{
    public static class TableEnumerator
    {
        public static string[] GetUserTables()
        {
            switch (SqlServerTools.GetServerVersion(null))
            {
                case SqlServerVersions.SqlServer2000:
                    return GetUserTables_2000();
                case SqlServerVersions.SqlServer2005:
                case SqlServerVersions.SqlServer2008:
                case SqlServerVersions.SqlServer2012:
                case SqlServerVersions.Other:
                    return GetUserTables_2005();
                default:
                    return null;
            }
        }

        private static string[] GetUserTables(string strSql)
        {
            string[] strArray;
            IDataReader reader = null;
            try
            {
                Database database = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database.GetSqlStringCommand(strSql);
                reader = database.ExecuteReader(sqlStringCommand);
                List<string> list = new List<string>(0x20);
                while (reader.Read())
                {
                    string item = reader.GetString(0);
                    if ((item != null) && !item.ToLower().StartsWith("csn_"))
                    {
                        list.Add(item);
                    }
                }
                strArray = (list.Count == 0) ? null : list.ToArray();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return strArray;
        }

        private static string[] GetUserTables_2000()
        {
            return GetUserTables("select [name] from [sysobjects] where [type]='U' AND [xtype]='U'");
        }

        private static string[] GetUserTables_2005()
        {
            return GetUserTables("select [name] from sys.[tables] where [type]='U'");
        }
    }
}