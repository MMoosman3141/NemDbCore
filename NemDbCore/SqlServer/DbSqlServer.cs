using System.Data.SqlClient;

namespace NemDbCore.SqlServer {
  public class DbSqlServer : DbBase<SqlCommand, SqlConnection, SqlDataReader, SqlParameter, DbSqlServerSettings> {

    public DbSqlServer() {
    }
    public DbSqlServer(DbSqlServerSettings settings) {
      ConnectionObject = GetConnection(settings);
    }

    public override SqlConnection GetConnection(DbSqlServerSettings settings) {
      SqlConnection connection = new SqlConnection(settings.ConnectionString) {
        Credential = new SqlCredential(settings.UserID, settings.SecurePassword)
      };

      return connection;
    }
  }
}
