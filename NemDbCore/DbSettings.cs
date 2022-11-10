using NemMvvm;
using System.Security;

namespace NemDbCore;

public abstract class DbSettings : NotifyPropertyChanged {
  private SecureString _securePassword = null;

  public SecureString SecurePassword {
    get => _securePassword;
    set => SetProperty(ref _securePassword, value);
  }

  public abstract string ConnectionString { get; }

}
