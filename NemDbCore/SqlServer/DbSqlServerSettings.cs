using System.Data.SqlClient;

namespace NemDbCore.SqlServer {
  public class DbSqlServerSettings : DbSettings {
    private SqlConnectionStringBuilder _strBuilder;
    private string _userId;

    /// <summary>
    /// The name of the application using the Sql Server
    /// </summary>
    public string ApplicationName {
      get => _strBuilder.ApplicationName;
      set {
        _strBuilder.ApplicationName = value;
        RaisePropertyChanged(nameof(ApplicationName));
      }
    }

    /// <summary>
    /// A description of the intent of the application using the Sql database
    /// </summary>
    public ApplicationIntent ApplicationIntent {
      get => _strBuilder.ApplicationIntent;
      set {
        _strBuilder.ApplicationIntent = value;
        RaisePropertyChanged(nameof(ApplicationIntent));
      }
    }
    public string AttachDBFilename {
      get => _strBuilder.AttachDBFilename;
      set {
        _strBuilder.AttachDBFilename = value;
        RaisePropertyChanged(nameof(AttachDBFilename));
      }
    }
    public int ConnectTimeout {
      get => _strBuilder.ConnectTimeout;
      set {
        _strBuilder.ConnectTimeout = value;
        RaisePropertyChanged(nameof(ConnectTimeout));
      }
    }
    public int ConnectRetryCount {
      get => _strBuilder.ConnectRetryCount;
      set {
        _strBuilder.ConnectRetryCount = value;
        RaisePropertyChanged(nameof(ConnectRetryCount));
      }
    }
    public int ConnectRetryInterval {
      get => _strBuilder.ConnectRetryInterval;
      set {
        _strBuilder.ConnectRetryInterval = value;
        RaisePropertyChanged(nameof(ConnectRetryInterval));
      }
    }
    public string CurrentLanguage {
      get => _strBuilder.CurrentLanguage;
      set {
        _strBuilder.CurrentLanguage = value;
        RaisePropertyChanged(nameof(CurrentLanguage));
      }
    }
    public string DataSource {
      get => _strBuilder.DataSource;
      set {
        _strBuilder.DataSource = value;
        RaisePropertyChanged(nameof(DataSource));
      }
    }
    public bool Encrypt {
      get => _strBuilder.Encrypt;
      set {
        _strBuilder.Encrypt = value;
        RaisePropertyChanged(nameof(Encrypt));
      }
    }
    public bool Enlist {
      get => _strBuilder.Enlist;
      set {
        _strBuilder.Enlist = value;
        RaisePropertyChanged(nameof(Enlist));
      }
    }
    public string FailoverPartner {
      get => _strBuilder.FailoverPartner;
      set {
        _strBuilder.FailoverPartner = value;
        RaisePropertyChanged(nameof(FailoverPartner));
      }
    }
    public string InitialCatalog {
      get => _strBuilder.InitialCatalog;
      set {
        _strBuilder.InitialCatalog = value;
        RaisePropertyChanged(nameof(InitialCatalog));
      }
    }
    public bool IntegratedSecurity {
      get => _strBuilder.IntegratedSecurity;
      set {
        _strBuilder.IntegratedSecurity = value;
        RaisePropertyChanged(nameof(IntegratedSecurity));
      }
    }
    public int LoadBalanceTimeout {
      get => _strBuilder.LoadBalanceTimeout;
      set {
        _strBuilder.LoadBalanceTimeout = value;
        RaisePropertyChanged(nameof(LoadBalanceTimeout));
      }
    }
    public int MaxPoolSize {
      get => _strBuilder.MaxPoolSize;
      set {
        _strBuilder.MaxPoolSize = value;
        RaisePropertyChanged(nameof(MaxPoolSize));
      }
    }
    public int MinPoolSize {
      get => _strBuilder.MinPoolSize;
      set {
        _strBuilder.MinPoolSize = value;
        RaisePropertyChanged(nameof(MinPoolSize));
      }
    }
    public bool MultipleActiveResultSets {
      get => _strBuilder.MultipleActiveResultSets;
      set {
        _strBuilder.MultipleActiveResultSets = value;
        RaisePropertyChanged(nameof(MultipleActiveResultSets));
      }
    }
    public bool MultiSubnetFailover {
      get => _strBuilder.MultiSubnetFailover;
      set {
        _strBuilder.MultiSubnetFailover = value;
        RaisePropertyChanged(nameof(MultiSubnetFailover));
      }
    }
    public int PacketSize {
      get => _strBuilder.PacketSize;
      set {
        _strBuilder.PacketSize = value;
        RaisePropertyChanged(nameof(PacketSize));
      }
    }
    public bool PersistSecurityInfo {
      get => _strBuilder.PersistSecurityInfo;
      set {
        _strBuilder.PersistSecurityInfo = value;
        RaisePropertyChanged(nameof(PersistSecurityInfo));
      }
    }
    public bool Pooling {
      get => _strBuilder.Pooling;
      set {
        _strBuilder.Pooling = value;
        RaisePropertyChanged(nameof(Pooling));
      }
    }
    public bool Replication {
      get => _strBuilder.Replication;
      set {
        _strBuilder.Replication = value;
        RaisePropertyChanged(nameof(Replication));
      }
    }
    public string TransactionBinding {
      get => _strBuilder.TransactionBinding;
      set {
        _strBuilder.TransactionBinding = value;
        RaisePropertyChanged(nameof(TransactionBinding));
      }
    }
    public bool TrustServerCertificate {
      get => _strBuilder.TrustServerCertificate;
      set {
        _strBuilder.TrustServerCertificate = value;
        RaisePropertyChanged(nameof(TrustServerCertificate));
      }
    }
    public string TypeSystemVersion {
      get => _strBuilder.TypeSystemVersion;
      set {
        _strBuilder.TypeSystemVersion = value;
        RaisePropertyChanged(nameof(TypeSystemVersion));
      }
    }
    public string UserID {
      get => _userId;
      set => SetProperty(ref _userId, value);
    }
    public bool UserInstance {
      get => _strBuilder.UserInstance;
      set {
        _strBuilder.UserInstance = value;
        RaisePropertyChanged(nameof(UserInstance));
      }
    }
    public string WorkstationID {
      get => _strBuilder.WorkstationID;
      set {
        _strBuilder.WorkstationID = value;
        RaisePropertyChanged(nameof(WorkstationID));
      }
    }

    public override string ConnectionString {
      get => _strBuilder.ConnectionString;
    }

    public DbSqlServerSettings() {
      _strBuilder = new SqlConnectionStringBuilder();
    }

  }
}
