using NemMvvm;
using System;
using System.Data;
using System.Data.Common;

namespace NemDbCore {
  public abstract class DbBase : NotifyPropertyChanged, IDisposable {
    private DbConnection _connection;

    public DbConnection ConnectionObject {
      get => _connection;
      set => SetProperty(ref _connection, value);
    }

    public abstract DbConnection GetConnection(DbSettings settings);

    public abstract int ExecuteQuery(string query, DbConnection connection = null);
    public abstract T GetScalar<T>(string query, DbConnection connection = null);
    public abstract object GetScalar(string query, DbConnection connection = null);
    public abstract void ExecuteReader(string query, Action<DbDataReader> readAction, DbConnection connection = null, CommandBehavior readBehavior = CommandBehavior.Default);

    protected int GenExecuteQuery<C>(string query, DbConnection connection) where C : DbCommand {
      if(connection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      int affectedCount = 0;
      using(C command = (C)Activator.CreateInstance(typeof(C), query, connection)) {
        affectedCount = command.ExecuteNonQuery();
      }

      return affectedCount;
    }
    protected T GenGetScalar<T,C>(string query, DbConnection connection) where C : DbCommand {
      if(connection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      object returnValue;
      using(C command = (C)Activator.CreateInstance(typeof(C), query, connection)) {
        returnValue = command.ExecuteScalar();
      }

      return (T)returnValue;
    }
    protected void GenExecuteReader<C>(string query, Action<DbDataReader> readAction, DbConnection connection, CommandBehavior readBehavior = CommandBehavior.Default) where C : DbCommand {
      if(connection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      using(C command = (C)Activator.CreateInstance(typeof(C), query, connection)) {
        DbDataReader reader = command.ExecuteReader(readBehavior);

        readAction(reader);
      }
    }

    public void Dispose() {
      if(ConnectionObject != null) {
        ConnectionObject.Dispose();
        ConnectionObject = null;
      }
    }

  }
}
