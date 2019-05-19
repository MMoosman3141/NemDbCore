using NemMvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NemDbCore {
  public abstract class DbBase<DbCmd, DbConn, DbReader, DbParam, DbSet> : NotifyPropertyChanged, IDisposable
    where DbCmd : DbCommand 
    where DbConn : DbConnection 
    where DbReader : DbDataReader
    where DbParam : DbParameter
    where DbSet : DbSettings {

    private DbConn _connection;
    private int _connectionCount = 0;

    public DbConn ConnectionObject {
      get => _connection;
      set => SetProperty(ref _connection, value);
    }

    public DbBase() { }
    public DbBase(DbSet settings) {
      ConnectionObject = GetConnection(settings);
    }

    public abstract DbConn GetConnection(DbSet settings);

    public virtual int ExecuteQuery(string query, DbConn connection = null) {
      return ExecuteQueryAsync(query, null, connection).Result;
    }
    public virtual int ExecuteQuery(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      return ExecuteQueryAsync(query, parameters, connection).Result;
    }
    public virtual async Task<int> ExecuteQueryAsync(string query, DbConn connection = null) {
      return await ExecuteQueryAsync(query, null, connection);
    }
    public virtual async Task<int> ExecuteQueryAsync(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      DbConn myConnection = connection ?? ConnectionObject;
      if (myConnection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      int affectedCount = 0;
      Interlocked.Increment(ref _connectionCount);
      using (DbCmd command = (DbCmd)Activator.CreateInstance(typeof(DbCmd), query, myConnection)) {
        if ((parameters?.Count() ?? 0) > 0) {
          foreach (DbParam parameter in parameters) {
            command.Parameters.Add(parameter);
          }
        }

        affectedCount = await command.ExecuteNonQueryAsync();
      }
      Interlocked.Decrement(ref _connectionCount);

      return affectedCount;
    }

    public virtual T GetScalar<T>(string query, DbConn connection = null) {
      return GetScalarAsync<T>(query, null, connection).Result;
    }
    public virtual T GetScalar<T>(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      return GetScalarAsync<T>(query, parameters, connection).Result;
    }
    public virtual async Task<T> GetScalarAsync<T>(string query, DbConn connection = null) {
      return await GetScalarAsync<T>(query, null, connection);
    }
    public virtual async Task<T> GetScalarAsync<T>(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      DbConn myConnection = connection ?? ConnectionObject;
      if (myConnection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      object returnValue = null;
      Interlocked.Increment(ref _connectionCount);
      using (DbCmd command = (DbCmd)Activator.CreateInstance(typeof(DbCmd), query, connection)) {
        if((parameters?.Count() ?? 0) > 0) {
          foreach(DbParam parameter in parameters) {
            command.Parameters.Add(parameter);
          }
        }

        returnValue = await command.ExecuteScalarAsync();
      }
      Interlocked.Decrement(ref _connectionCount);

      return (T)returnValue;
    }

    public virtual object GetScalar(string query, DbConn connection = null) {
      return GetScalarAsync<object>(query, null, connection).Result;
    }
    public virtual object GetScalar(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      return GetScalarAsync<object>(query, parameters, connection).Result;
    }
    public virtual async Task<object> GetScalarAsync(string query, DbConn connection = null) {
      return await GetScalarAsync<object>(query, null, connection);
    }
    public virtual async Task<object> GetScalarAsync(string query, IEnumerable<DbParam> parameters, DbConn connection = null) {
      return await GetScalarAsync<object>(query, parameters, connection);
    }

    public virtual void ExecuteReader(string query, Action<DbReader> readAction, CommandBehavior commandBehavior = CommandBehavior.Default) {
      ExecuteReaderAsync(query, null, readAction, ConnectionObject, commandBehavior).Wait();
    }
    public virtual void ExecuteReader(string query, IEnumerable<DbParam> parameters, Action<DbReader> readAction, CommandBehavior commandBehavior = CommandBehavior.Default) {
      ExecuteReaderAsync(query, parameters, readAction, ConnectionObject, commandBehavior).Wait();
    }
    public virtual void ExecuteReader(string query, Action<DbReader> readAction, DbConn connection, CommandBehavior readBehavior = CommandBehavior.Default) {
      ExecuteReaderAsync(query, null, readAction, connection, readBehavior).Wait();
    }
    public virtual void ExecuteReader(string query, IEnumerable<DbParam> parameters, Action<DbReader> readAction, DbConn connection, CommandBehavior readBehavior = CommandBehavior.Default) {
      ExecuteReaderAsync(query, parameters, readAction, connection, readBehavior).Wait();
    }
    public virtual async Task ExecuteReaderAsync(string query, Action<DbReader> readAction, CommandBehavior commandBehavior = CommandBehavior.Default) {
      await ExecuteReaderAsync(query, null, readAction, ConnectionObject, commandBehavior);
    }
    public virtual async Task ExecuteReaderAsync(string query, IEnumerable<DbParam> parameters, Action<DbReader> readAction, DbConn connection, CommandBehavior readBehavior = CommandBehavior.Default) {
      if (connection == null) {
        throw new ArgumentNullException("Connection cannot be null.  Either pass in a connection, or initialize with settings.");
      }

      Interlocked.Increment(ref _connectionCount);
      using (DbCmd command = (DbCmd)Activator.CreateInstance(typeof(DbCmd), query, connection)) {
        if((parameters?.Count() ?? 0) > 0) {
          foreach(DbParam parameter in parameters) {
            command.Parameters.Add(parameter);
          }
        }

        DbReader reader = (DbReader)await command.ExecuteReaderAsync(readBehavior);

        await Task.Run(() => {
          readAction?.Invoke(reader);
        });
      }
      Interlocked.Decrement(ref _connectionCount);
    }

    public void Dispose() {
      if (ConnectionObject != null) {
        Task.Run(() => {
          SpinWait.SpinUntil(() => _connectionCount == 0);
          ConnectionObject.Dispose();
          ConnectionObject = null;
        });
      }
    }

  }
}
