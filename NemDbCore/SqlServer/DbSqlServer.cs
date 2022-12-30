using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NemDbCore.SqlServer;

public class DbSqlServer : DbBase {

  public DbSqlServer() {
  }
  public DbSqlServer(DbSqlServerSettings settings) {
    ConnectionObject = GetConnection(settings);
  }
  public DbSqlServer(SqlConnection connection) {
    ConnectionObject = connection;
  }

  public override DbConnection GetConnection(DbSettings settings) {
    if(settings is not DbSqlServerSettings) {
      throw new ArgumentException("settings must be of type DbSqlServerSettings.");
    }

    DbSqlServerSettings sqlSettings = (DbSqlServerSettings)settings;

    SqlConnection connection = new(sqlSettings.ConnectionString) {
      Credential = new SqlCredential(sqlSettings.UserID, sqlSettings.SecurePassword)
    };

    return connection;
  }

  public override int ExecuteQuery(string query, DbConnection connection = null) {
    return GenExecuteQuery<SqlCommand>(query, connection ?? ConnectionObject, null);
  }

  public override int ExecuteQuery(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null) {
    return GenExecuteQuery<SqlCommand>(query, connection ?? ConnectionObject, parameters);
  }

  public override void ExecuteReader(string query, Action<DbDataReader> readAction, DbConnection connection = null, CommandBehavior readBehavior = CommandBehavior.Default) {
    GenExecuteReader<SqlCommand>(query, null, readAction, connection ?? ConnectionObject, readBehavior);
  }

  public override void ExecuteReader(string query, IEnumerable<DbParameter> parameters, Action<DbDataReader> readAction, DbConnection connection = null, CommandBehavior readBehavior = CommandBehavior.Default) {
    GenExecuteReader<SqlCommand>(query, parameters, readAction, connection ?? ConnectionObject, readBehavior);
  }

  public override T GetScalar<T>(string query, DbConnection connection = null) {
    return GenGetScalar<T, SqlCommand>(query, connection ?? ConnectionObject, null);
  }

  public override T GetScalar<T>(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null) {
    return GenGetScalar<T, SqlCommand>(query, connection ?? ConnectionObject, parameters);
  }

  public override object GetScalar(string query, DbConnection connection = null) {
    return GenGetScalar<object, SqlCommand>(query, connection ?? ConnectionObject, null);
  }

  public override object GetScalar(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null) {
    return GenGetScalar<object, SqlCommand>(query, connection ?? ConnectionObject, parameters);
  }
}
