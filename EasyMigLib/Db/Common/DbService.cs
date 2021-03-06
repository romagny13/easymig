﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace EasyMigLib.Db.Common
{
    public class DbService : IDbService
    {
        public DbConnection Connection { get; protected set; }

        public IDbService CreateConnection(string connectionString, string providerName)
        {
            var connection = DbProviderFactories.GetFactory(providerName).CreateConnection();
            connection.ConnectionString = connectionString;
            this.Connection = connection;
            return this;
        }

        public bool IsOpen()
        {
            return this.Connection.State == ConnectionState.Open;
        }

        public void Open()
        {
            try
            {
                if (!this.IsOpen())
                {
                    this.Connection.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task OpenAsync()
        {
            try
            {
                if (!this.IsOpen())
                {
                    await this.Connection.OpenAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Close()
        {
            this.Connection.Close();
        }


        protected void CheckAndAddParameters(DbCommand command, List<DbServiceParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.ParameterName;
                    dbParameter.Value = parameter.Value;

                    if (parameter.ParameterType.HasValue)
                    {
                        dbParameter.DbType = parameter.ParameterType.Value;
                    }

                    if (parameter.Direction.HasValue)
                    {
                        dbParameter.Direction = parameter.Direction.Value;
                    }

                    command.Parameters.Add(dbParameter);
                }
            }
        }

        public int Execute(string sql, List<DbServiceParameter> parameters = null)
        {
            int rowsAffected = 0;

            try
            {
                using (var command = this.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    this.CheckAndAddParameters(command, parameters);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return rowsAffected;
        }

        public List<Dictionary<string, object>> ReadAll(string sql, List<DbServiceParameter> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                using (var command = this.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    this.CheckAndAddParameters(command, parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rows = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var value = reader.GetValue(i);
                                rows[reader.GetName(i)] = value.GetType() == typeof(DBNull) ? null : value;
                            }
                            result.Add(rows);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public Dictionary<string, object> ReadOne(string sql, List<DbServiceParameter> parameters = null)
        {
            var result = new Dictionary<string, object>();

            try
            {
                using (var command = this.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    this.CheckAndAddParameters(command, parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var value = reader.GetValue(i);
                                result[reader.GetName(i)] = value.GetType() == typeof(DBNull) ? null : value;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public object ExecuteScalar(string sql, List<DbServiceParameter> parameters = null)
        {
            object result = null;

            try
            {
                using (var command = this.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    this.CheckAndAddParameters(command, parameters);

                    result = command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public async Task ExecuteAsync(string sql, List<DbServiceParameter> parameters = null)
        {
            try
            {
                using (var command = this.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    this.CheckAndAddParameters(command, parameters);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
