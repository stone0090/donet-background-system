using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;

namespace stonefw.Utility
{
    /// <summary>
    /// 进行各种数据库相关操作的数据库操作类
    /// </summary>
    public class Database
    {
        #region 属性

        /// <summary>
        /// 是否是否在调用存储过程时检查参数
        /// </summary>
        public bool CheckStoredProcedurePara{ get; set; }
        public string DbName { get; private set; }
        public string ConnectionString { get; private set; }
        public string ProviderName { get; private set; }
        public DbProviderFactory ProviderFactory { get; private set; }
        public DbTransaction DbTransactions { get; set; }

        #endregion

        #region 全局变量

        private const string StrSetIsoLevelReadCommited = " SET TRANSACTION ISOLATION LEVEL READ COMMITTED; ";
        private const string StrSetIsoLevelReadUnCommited = " SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; ";

        #endregion

        #region 构造函数
        public Database(string dbName = "", bool checkStoredProcedurePara = true)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                this.ConnectionString = ConfigHelper.GetDbString(1);
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new Exception("尚未配置数据库连接字符串！");

                this.DbName = ConfigHelper.GetDbName(1);
                this.ProviderName = ConfigHelper.GetDbProviderName(1);
                this.ProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);

            }
            else
            {
                this.ConnectionString = ConfigHelper.GetDbString(dbName);
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new Exception("尚未配置数据库连接字符串！");

                this.DbName = ConfigHelper.GetDbName(dbName);
                this.ProviderName = ConfigHelper.GetDbProviderName(dbName);
                this.ProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);
            }

            if (Encryption.IsEncrypted(this.ConnectionString))
                this.ConnectionString = Encryption.Decrypt(this.ConnectionString);

            //是否检查存储过程的参数
            this.CheckStoredProcedurePara = checkStoredProcedurePara;
        }

        #endregion

        #region 传入参数

        public DbParameter AddInParameter(DbCommand dm, string name, DbType dbType, object value)
        {
            return AddParameter(dm, name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, value);
        }
        public virtual DbParameter AddParameter(DbCommand dm, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            ////if (dbType == DbType.String)
            ////    throw new Exception("请不要使用DbType.String进行数据库查询！");

            if (CheckInjectAttackForSp(dm, value))
                throw new Exception("输入的部分内容可能对系统稳定性造成影响，操作已停止！[" + value + "]");

            DbParameter param = this.ProviderFactory.CreateParameter();
            if (param != null)
            {
                param.ParameterName = name;
                param.DbType = dbType;
                param.Size = size;
                param.Value = value ?? DBNull.Value;
                param.Direction = direction;
                param.IsNullable = nullable;
                param.SourceColumn = sourceColumn;
                param.SourceVersion = sourceVersion;
                dm.Parameters.Add(param);
            }
            return param;
        }

        #endregion

        #region 获取命令

        public virtual DbCommand GetSqlStringCommand(string commandText)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("命令为空", "commandText");
            }
            return CreateCommand(CommandType.Text, commandText);
        }
        public virtual DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentException("存储过程名字为空", "storedProcedureName");
            }
            return CreateCommand(CommandType.StoredProcedure, storedProcedureName);
        }
        public virtual DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentException("存储过程名字为空", "storedProcedureName");
            }
            DbCommand dm = CreateCommand(CommandType.StoredProcedure, storedProcedureName);
            for (int i = 0; i < parameterValues.Length; i++)
            {
                IDataParameter parameter = dm.Parameters[i];
                if (CheckInjectAttackForSp(dm, parameterValues[i]))
                {
                    throw new Exception("输入的部分内容可能对系统稳定性造成影响，操作已停止！[" + parameterValues[i] + "]");
                }
                dm.Parameters[parameter.ParameterName].Value = parameterValues[i] ?? DBNull.Value;
            }
            return dm;
        }
        private DbCommand CreateCommand(CommandType commandType, string commandText = "")
        {
            DbCommand dm = ProviderFactory.CreateCommand();
            {
                if (dm != null)
                {
                    dm.CommandType = commandType;
                    dm.CommandText = commandText;
                }
                return dm;
            }
        }

        #endregion

        #region 创建连接和事务

        public virtual DbTransaction CreateTransaction()
        {
            return CreateTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public virtual DbTransaction CreateTransaction(System.Data.IsolationLevel iso)
        {
            DbConnection dc = CreateConnection();
            DbTransaction dt = dc.BeginTransaction(iso);
            return dt;
        }

        public virtual DbConnection CreateConnection()
        {
            DbConnection dc = ProviderFactory.CreateConnection();
            if (dc != null && dc.State != ConnectionState.Open)
            {
                dc.ConnectionString = this.ConnectionString;
                dc.Open();
            }
            return dc;
        }

        #endregion

        #region 执行语句

        public virtual DataTable ExecuteDataTable(string commandText)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                return ExecuteDataTable(dm);
            }
        }
        public virtual DataTable ExecuteDataTable(DbCommand dm)
        {
            using (DbConnection conn = CreateConnection())
            {
                dm.Connection = conn;
                DataTable myDataTable = new DataTable();
                using (DbDataAdapter myDataAdapter = this.ProviderFactory.CreateDataAdapter())
                {
                    if (myDataAdapter != null)
                    {
                        myDataAdapter.SelectCommand = dm;
                        myDataAdapter.Fill(myDataTable);
                    }
                    myDataTable.RemotingFormat = SerializationFormat.Binary;
                    return myDataTable;
                }
            }
        }

        public virtual DataSet ExecuteDataSet(string commandText)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                return ExecuteDataSet(dm);
            }
        }
        public virtual DataSet ExecuteDataSet(DbCommand dm)
        {
            using (DbConnection conn = CreateConnection())
            {
                dm.Connection = conn;
                using (DbDataAdapter myDataAdapter = this.ProviderFactory.CreateDataAdapter())
                {
                    DataSet myDataSet = new DataSet();
                    if (myDataAdapter != null)
                    {
                        myDataAdapter.SelectCommand = dm;
                        myDataAdapter.Fill(myDataSet);
                    }
                    myDataSet.RemotingFormat = SerializationFormat.Binary;
                    return myDataSet;
                }
            }
        }

        public virtual int ExecuteNonQuery(string commandText)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                return ExecuteNonQuery(dm);
            }
        }
        public virtual int ExecuteNonQuery(DbCommand dm)
        {
            using (DbConnection conn = CreateConnection())
            {
                dm.Connection = conn;
                int result = dm.ExecuteNonQuery();
                return result;
            }
        }
        public virtual int ExecuteNonQuery(string commandText, DbTransaction dt)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                if (dm == null) return 0;
                return ExecuteNonQuery(dm, dt);
            }
        }
        public virtual int ExecuteNonQuery(DbCommand dm, DbTransaction dt)
        {
            dm.Transaction = dt;
            dm.Connection = dt.Connection;
            int result = dm.ExecuteNonQuery();
            return result;
        }

        public virtual IDataReader ExecuteReader(string commandText)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                return ExecuteReader(dm);
            }
        }
        public virtual IDataReader ExecuteReader(DbCommand dm)
        {
            dm.Connection = CreateConnection();
            if (dm.CommandType == CommandType.Text)
                dm.CommandText = StrSetIsoLevelReadUnCommited + dm.CommandText;

            //IDataReader reader;
            //if (Transaction.Current != null)
            //    reader = dm.ExecuteReader(CommandBehavior.Default);
            //else
            //    reader = dm.ExecuteReader(CommandBehavior.CloseConnection);

            return dm.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public virtual IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand dm = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteReader(dm);
            }
        }

        public virtual object ExecuteScalar(string commandText)
        {
            using (DbCommand dm = GetSqlStringCommand(commandText))
            {
                dm.CommandText = StrSetIsoLevelReadCommited + commandText;
                return ExecuteScalar(dm);
            }
        }
        public virtual object ExecuteScalar(DbCommand dm)
        {
            using (DbConnection conn = CreateConnection())
            {
                dm.Connection = conn;
                if (dm.CommandType == CommandType.Text)
                    dm.CommandText = StrSetIsoLevelReadUnCommited + dm.CommandText;
                return dm.ExecuteScalar();
            }
        }
        public virtual object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            using (DbCommand dm = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(dm);
            }
        }

        #endregion

        #region 检查存储过程SQL注入

        /// <summary>
        /// 检查调用存储过程时相关的参数是否有注入的风险
        /// </summary>
        private bool CheckInjectAttackForSp(DbCommand dm)
        {
            if (!this.CheckStoredProcedurePara)
                return false;

            if (dm == null)
                return false;

            if (dm.CommandType != CommandType.StoredProcedure)
                return false;

            if (dm.Parameters.Count == 0)
                return false;

            for (int i = 0; i < dm.Parameters.Count; i++)
            {
                if (dm.Parameters[i].Value == null || dm.Parameters[i].Value is DBNull)
                    continue;
                if (!(dm.Parameters[i].Value is string))
                    continue;

                if (!SqlInjectionReject.CheckMssqlParameter(dm.Parameters[i].Value.ToString()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查调用存储过程时相关的参数是否有注入的风险
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool CheckInjectAttackForSp(DbCommand dm, object val)
        {
            if (!this.CheckStoredProcedurePara)
                return false;

            if (dm == null)
                return false;

            if (dm.CommandType != CommandType.StoredProcedure)
                return false;

            if (val == null || val is DBNull)
                return false;

            if (!(val is string))
                return false;

            if (!SqlInjectionReject.CheckMssqlParameter(val.ToString()))
                return true;

            return false;
        }

        #endregion

    }

    public class DatabaseFactory
    {
        public static Database CreateDatabase()
        {
            return new Database();
        }
        public static Database CreateDatabase(string dbName)
        {
            return new Database(dbName);
        }
    }

    //SQL注入判断
    public static class SqlInjectionReject
    {
        /// <summary>
        /// T-SQL关键字
        /// </summary>
        private static readonly string[] TsqlKeyWords = new string[] { 
"ADD","EXCEPT","PERCENT","ALL","EXEC","PLAN","ALTER","EXECUTE","PRECISION","AND","EXISTS","PRIMARY","ANY","EXIT",
"PRINT","AS", "FETCH", "PROC","ASC", "FILE", "PROCEDURE","AUTHORIZATION","FILLFACTOR","PUBLIC","BACKUP","FOR","RAISERROR",
"BEGIN", "FOREIGN", "READ","BETWEEN","FREETEXT","READTEXT","BREAK","FREETEXTTABLE","RECONFIGURE","BROWSE","FROM",
"REFERENCES","BULK","FULL","REPLICATION","BY","FUNCTION","RESTORE","CASCADE","GOTO","RESTRICT","CASE","GRANT","RETURN",
"CHECK","GROUP","REVOKE","CHECKPOINT","HAVING","RIGHT","CLOSE","HOLDLOCK","ROLLBACK","CLUSTERED","IDENTITY","ROWCOUNT",
"COALESCE","IDENTITY_INSERT","ROWGUIDCOL","COLLATE","IDENTITYCOL","RULE","COLUMN","IF","SAVE","COMMIT","IN","SCHEMA",
"COMPUTE","INDEX","SELECT","CONSTRAINT","INNER","SESSION_USER","CONTAINS","INSERT","SET","CONTAINSTABLE","INTERSECT",
"SETUSER","CONTINUE","INTO","SHUTDOWN","CONVERT","IS","SOME","CREATE","JOIN","STATISTICS","CROSS","KEY","SYSTEM_USER",
"CURRENT","KILL","TABLE","CURRENT_DATE","LEFT","TEXTSIZE","CURRENT_TIME","LIKE","THEN","CURRENT_TIMESTAMP","LINENO",
"TO","CURRENT_USER","LOAD","TOP","CURSOR","NATIONAL","TRAN","DATABASE","NOCHECK","TRANSACTION","DBCC","NONCLUSTERED",
"TRIGGER","DEALLOCATE","NOT","TRUNCATE","DECLARE","NULL","TSEQUAL","DEFAULT","NULLIF","UNION","DELETE","OF","UNIQUE",
"DENY","OFF","UPDATE","DESC","OFFSETS","UPDATETEXT","DISK","ON","USE","DISTINCT","OPEN","USER","DISTRIBUTED","OPENDATASOURCE",
"VALUES","DOUBLE","OPENQUERY","VARYING","DROP","OPENROWSET","VIEW","DUMMY","OPENXML","WAITFOR","DUMP","OPTION","WHEN",
"ELSE","OR","WHERE","END","ORDER","WHILE","ERRLVL","OUTER","WITH","ESCAPE","OVER","WRITETEXT"};

        /// <summary>
        /// ODBC关键字
        /// </summary>
        private static readonly string[] OdbcKeyWords = new string[]{            
"ABSOLUTE","EXEC","OVERLAPS","ACTION","EXECUTE","PAD","ADA","EXISTS","PARTIAL","ADD","EXTERNAL","PASCAL","ALL","EXTRACT","POSITION",
"ALLOCATE","FALSE","PRECISION","ALTER","FETCH","PREPARE","AND","FIRST","PRESERVE","ANY","FLOAT","PRIMARY","ARE","FOR","PRIOR","AS",
"FOREIGN","PRIVILEGES","ASC","FORTRAN","PROCEDURE","ASSERTION","FOUND","PUBLIC","AT","FROM","READ","AUTHORIZATION","FULL","REAL",
"AVG","GET","REFERENCES","BEGIN","GLOBAL","RELATIVE","BETWEEN","GO","RESTRICT","BIT","GOTO","REVOKE","BIT_LENGTH","GRANT","RIGHT",
"BOTH","GROUP","ROLLBACK","BY","HAVING","ROWS","CASCADE","HOUR","SCHEMA","CASCADED","IDENTITY","SCROLL","CASE","IMMEDIATE","SECOND",
"CAST","IN","SECTION","CATALOG","INCLUDE","SELECT","CHAR","INDEX","SESSION","CHAR_LENGTH","INDICATOR","SESSION_USER","CHARACTER",
"INITIALLY","SET","CHARACTER_LENGTH","INNER","SIZE","CHECK","INPUT","SMALLINT","CLOSE","INSENSITIVE","SOME","COALESCE","INSERT","SPACE",
"COLLATE","INT","SQL","COLLATION","INTEGER","SQLCA","COLUMN","INTERSECT","SQLCODE","COMMIT","INTERVAL","SQLERROR","CONNECT",
"INTO","SQLSTATE","CONNECTION","IS","SQLWARNING","CONSTRAINT","ISOLATION","SUBSTRING","CONSTRAINTS","JOIN","SUM","CONTINUE",
"KEY","SYSTEM_USER","CONVERT","LANGUAGE","TABLE","CORRESPONDING","LAST","TEMPORARY","COUNT","LEADING","THEN","CREATE","LEFT",
"TIME","CROSS","LEVEL","TIMESTAMP","CURRENT","LIKE","TIMEZONE_HOUR","CURRENT_DATE","LOCAL","TIMEZONE_MINUTE","CURRENT_TIME",
"LOWER","TO","CURRENT_TIMESTAMP","MATCH","TRAILING","CURRENT_USER","MAX","TRANSACTION","CURSOR","MIN","TRANSLATE","DATE",
"MINUTE","TRANSLATION","DAY","MODULE","TRIM","DEALLOCATE","MONTH","TRUE","DEC","NAMES","UNION","DECIMAL","NATIONAL","UNIQUE",
"DECLARE","NATURAL","UNKNOWN","DEFAULT","NCHAR","UPDATE","DEFERRABLE","NEXT","UPPER","DEFERRED","NO","USAGE","DELETE","NONE",
"USER","DESC","NOT","USING","DESCRIBE","NULL","VALUE","DESCRIPTOR","NULLIF","VALUES","DIAGNOSTICS","NUMERIC","VARCHAR","DISCONNECT",
"OCTET_LENGTH","VARYING","DISTINCT","OF","VIEW","DOMAIN","ON","WHEN","DOUBLE","ONLY","WHENEVER","DROP","OPEN","WHERE","ELSE",
"OPTION","WITH","END","OR","WORK","END-EXEC","ORDER","WRITE","ESCAPE","OUTER","YEAR","EXCEPT","OUTPUT","ZONE","EXCEPTION"};

        /// <summary>
        /// 特殊字符
        /// </summary>
        private static readonly string[] SpecialWords = new string[] { "'", "[", "]", "\\", "%", "_", ";", "/", "*", "-", "--", "=", ">", "<", "<>", "!=", "/*", "*/", "\n" };

        /// <summary>
        /// 时间值
        /// </summary>
        private static readonly Regex DateTimeRule = new Regex(@"^(\d{2,4}-\d{1,2}-\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}:\d{1,2})$");

        /// <summary>
        /// 部分时间值
        /// </summary>
        private static readonly Regex PartDateTimeRule = new Regex(@"(\d{2,4}-\d{1,2}-\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}|\d{2,4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}:\d{1,2})");

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
                { return false; }
            }
            //检测其他关键字和字符

            string[] check = sqlParameter.Split(' ').ToArray();
            bool result = true;
            foreach (string checkString in check)
            {
                if (TsqlKeyWords.Contains(checkString)) { result = false; break; }
                else if (OdbcKeyWords.Contains(checkString)) { result = false; break; }
            }
            return result;
        }
    }

}
