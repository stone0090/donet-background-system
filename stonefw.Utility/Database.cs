using System;
using System.Data;
using System.Data.Common;

namespace Stonefw.Utility
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
        public bool CheckStoredProcedurePara { get; set; }

        public string DbName { get; private set; }
        public string ConnectionString { get; }
        public string ProviderName { get; }
        public DbProviderFactory ProviderFactory { get; }
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
            return AddParameter(dm, name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty,
                DataRowVersion.Default, value);
        }

        public virtual DbParameter AddParameter(DbCommand dm, string name, DbType dbType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
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

    //SQL注入判断
}