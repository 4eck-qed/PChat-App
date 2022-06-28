using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using PChat.Notify;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

/// <summary>
/// Contains everything you don't see.
/// </summary>
public class MetaViewModel : ViewModelBase
{
    protected SharedViewModel Shared { get; }
    
    #region Fields
    
    private readonly CancellationToken _cancellationToken;

    #endregion

    public MetaViewModel(SharedViewModel shared, CancellationToken cancellationToken)
    {
        Shared = shared;
        _cancellationToken = cancellationToken;
        
        // TODO (maybe) encode login history
        Shared.LoginHistoryService.AddToLoginHistory(DateTime.Now);
        LoginHistory = new ObservableCollection<DateTime>(Shared.LoginHistoryService.GetLoginHistory());
    }

    #region Private Methods

    #endregion

    #region Public Methods
    
    public DateTime GetLastLogin() => LoginHistory.Any() ? LoginHistory.Last() : DateTime.Now;
    
    #endregion
    
    #region Properties

    /// <summary>
    /// Keeps track of login dates.
    /// </summary>
    public ObservableCollection<DateTime> LoginHistory { get; }

    #endregion
}