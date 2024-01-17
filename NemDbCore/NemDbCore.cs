using NemMvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace NemDbCore;

public abstract class DbBase : NotifyPropertyChanged, IDisposable {
  private DbConnection _connection;
  protected ArgumentNullException _connectionNullException = new("Connection cannot be null.  Either pass in a connection, or initialize with settings.");

  public DbConnection ConnectionObject {
    get => _connection;
    set => SetProperty(ref _connection, value);
  }

  public abstract DbConnection GetConnection(DbSettings settings);

  public abstract int ExecuteQuery(string query, DbConnection connection = null);
  public abstract int ExecuteQuery(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null);
  public abstract T GetScalar<T>(string query, DbConnection connection = null);
  public abstract T GetScalar<T>(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null);
  public abstract object GetScalar(string query, DbConnection connection = null);
  public abstract object GetScalar(string query, IEnumerable<DbParameter> parameters, DbConnection connection = null);
  public abstract void ExecuteReader(string query, Action<DbDataReader> readAction, DbConnection connection = null, CommandBehavior readBehavior = CommandBehavior.Default);
  public abstract void ExecuteReader(string query, IEnumerable<DbParameter> parameters, Action<DbDataReader> readAction, DbConnection connection = null, CommandBehavior readBehavior = CommandBehavior.Default);


  protected int GenExecuteQuery<C>(string query, DbConnection connection, IEnumerable<DbParameter> parameters) where C : DbCommand {
    if (connection is null) {
      throw _connectionNullException;
    }

    int affectedCount = 0;
    using (C command = (C)Activator.CreateInstance(typeof(C), query, connection)) {
      if (parameters?.Any() ?? false) {
        command.Parameters.AddRange(parameters.ToArray());
      }

      affectedCount = command.ExecuteNonQuery();
    }

    return affectedCount;
  }

  protected T GenGetScalar<T, C>(string query, DbConnection connection, IEnumerable<DbParameter> parameters) where C : DbCommand {
    if (connection is null) {
      throw _connectionNullException;
    }

    object returnValue;
    using (C command = (C)Activator.CreateInstance(typeof(C), query, connection)) {
      if (parameters?.Any() ?? false) {
        command.Parameters.AddRange(parameters.ToArray());
      }

      returnValue = command.ExecuteScalar();
    }

    return (T)returnValue;
  }

  protected void GenExecuteReader<C>(string query, IEnumerable<DbParameter> parameters, Action<DbDataReader> readAction, DbConnection connection, CommandBehavior readBehavior = CommandBehavior.Default) where C : DbCommand {
    if (connection is null) {
      throw _connectionNullException;
    }

    using C command = (C)Activator.CreateInstance(typeof(C), query, connection);
    if (parameters?.Any() ?? false) {
      command.Parameters.AddRange(parameters.ToArray());
    }

    DbDataReader reader = command.ExecuteReader(readBehavior);

    readAction(reader);
  }

  public void Dispose() {
    GC.SuppressFinalize(this);
    ConnectionObject?.Dispose();
    ConnectionObject = null;
  }

}
