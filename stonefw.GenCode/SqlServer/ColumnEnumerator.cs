using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Stonefw.Utility;

namespace Stonefw.CodeGenerate.SqlServer
{
    public static class ColumnEnumerator
    {
        public static SqlServerColumn[] GetColumnsOfTable(string table)
        {
            switch (SqlServerTools.GetServerVersion((string) null))
            {
                case SqlServerVersions.SqlServer2000:
                    return ColumnEnumerator.GetColumnsOfTable_2000(table);
                case SqlServerVersions.SqlServer2005:
                case SqlServerVersions.SqlServer2008:
                case SqlServerVersions.SqlServer2012:
                case SqlServerVersions.Other:
                    return ColumnEnumerator.GetColumnsOfTable_2005(table);
                default:
                    return null;
            }
        }

        public static string[] GetPrimaryKeyColumns(string table)
        {
            switch (SqlServerTools.GetServerVersion((string) null))
            {
                case SqlServerVersions.SqlServer2000:
                    return null;
                case SqlServerVersions.SqlServer2005:
                case SqlServerVersions.SqlServer2008:
                case SqlServerVersions.SqlServer2012:
                case SqlServerVersions.Other:
                    return ColumnEnumerator.GetPrimaryKeyColumnsOfTable(table);
                default:
                    return null;
            }
        }

        public static string GetIdentityColumn(string table)
        {
            switch (SqlServerTools.GetServerVersion((string) null))
            {
                case SqlServerVersions.SqlServer2000:
                    return null;
                case SqlServerVersions.SqlServer2005:
                case SqlServerVersions.SqlServer2008:
                case SqlServerVersions.SqlServer2012:
                case SqlServerVersions.Other:
                    return ColumnEnumerator.GetIdentityColumnOfTable(table);
                default:
                    return null;
            }
        }

        private static string GetIdentityColumnOfTable(string table)
        {
            const string strSql =
                "select c.[name] from [sys].[tables] as t,[sys].[columns] as c where t.[name]=@TheTable and t.[object_id]=c.[object_id] and c.[is_identity]='1'";
            try
            {
                Database database2 = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database2.GetSqlStringCommand(strSql);
                database2.AddInParameter(sqlStringCommand, "@TheTable", DbType.AnsiString, (object) table);
                object obj = database2.ExecuteScalar(sqlStringCommand);
                if (obj == null || obj is DBNull)
                    return (string) null;
                else
                    return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string[] GetPrimaryKeyColumnsOfTable(string table)
        {
            const string strSql =
                "select c.[name] from [sys].[tables] as t,[sys].[columns] as c,[sys].[indexes] as i,[sys].[index_columns] as ic where t.[name]=@TheTable and i.[is_primary_key]='1' and t.[object_id]=c.[object_id] and t.[object_id]=i.[object_id] and t.[object_id]=ic.[object_id] and c.[column_id]=ic.[column_id] and i.[index_id]=ic.[index_id]";
            IDataReader dataReader = (IDataReader) null;
            try
            {
                Database database2 = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database2.GetSqlStringCommand(strSql);
                database2.AddInParameter(sqlStringCommand, "@TheTable", DbType.AnsiString, (object) table);
                dataReader = database2.ExecuteReader(sqlStringCommand);
                List<string> list = new List<string>(32);
                while (dataReader.Read())
                    list.Add(dataReader.GetString(0));
                return list.Count == 0 ? (string[]) null : list.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
        }

        private static SqlServerColumn[] GetColumnsOfTable_2000(string table)
        {
            const string strSql =
                "select c.name as ColumnName, ty.name as ColumnType, c.length as ColumnSize\r\n  from syscolumns as c\r\n  inner join sysobjects as t on t.id=c.id\r\n  inner join systypes as ty on ty.xtype = c.xtype\r\nwhere t.xtype='U' and t.name=@TheTable";
            IDataReader dataReader = (IDataReader) null;
            try
            {
                Database database2 = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database2.GetSqlStringCommand(strSql);
                database2.AddInParameter(sqlStringCommand, "@TheTable", DbType.AnsiString, (object) table);
                dataReader = database2.ExecuteReader(sqlStringCommand);
                List<SqlServerColumn> list = new List<SqlServerColumn>(32);
                while (dataReader.Read())
                {
                    SqlServerColumn sqlServerColumn = new SqlServerColumn()
                    {
                        ColumnName = dataReader.GetString(0),
                        ColumnType = dataReader.GetString(1),
                        ColumnSize = int.Parse(dataReader.GetValue(2).ToString())
                    };
                    list.Add(sqlServerColumn);
                }
                return list.Count == 0 ? (SqlServerColumn[]) null : list.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
        }

        private static SqlServerColumn[] GetColumnsOfTable_2005(string table)
        {
            const string strSql =
                "select c.name as ColumnName, ty.name as ColumnType, c.max_length as ColumnSize\r\n  from sys.columns as c\r\n  inner join sys.tables as t on t.object_id=c.object_id\r\n  inner join sys.types as ty on ty.system_type_id = c.system_type_id\r\nwhere ty.system_type_id=ty.user_type_id and t.name=@TheTable";
            IDataReader dataReader = (IDataReader) null;
            try
            {
                Database database2 = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database2.GetSqlStringCommand(strSql);
                database2.AddInParameter(sqlStringCommand, "@TheTable", DbType.AnsiString, (object) table);
                dataReader = database2.ExecuteReader(sqlStringCommand);
                List<SqlServerColumn> list = new List<SqlServerColumn>(32);
                while (dataReader.Read())
                {
                    SqlServerColumn sqlServerColumn = new SqlServerColumn()
                    {
                        ColumnName = dataReader.GetString(0),
                        ColumnType = dataReader.GetString(1),
                        ColumnSize = int.Parse(dataReader.GetValue(2).ToString())
                    };
                    list.Add(sqlServerColumn);
                }
                return list.Count == 0 ? (SqlServerColumn[]) null : list.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
            }
        }
    }
}