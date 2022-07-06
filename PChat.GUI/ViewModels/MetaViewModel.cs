using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using ReactiveUI;

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
        LoginHistoryService.AddToLoginHistory(DateTime.Now);
        LoginHistory = new ObservableCollection<DateTime>(LoginHistoryService.GetLoginHistory());
        EventBus.Instance.Subscribe(this);
        Console.WriteLine("[DEBUG] registered listener '{0}' to EventBus", this);
    }

    [UsedImplicitly]
    public void OnEvent(OnObjectChangedEvent e)
    {
        Console.WriteLine("[DEBUG] OnObjectChangedEvent '{0}'", e.ObjectName);
        this.RaisePropertyChanged(e.ObjectName);
    }

    public DateTime GetLastLogin() => LoginHistory.Any() ? LoginHistory.Last() : DateTime.Now;

    /// <summary>
    /// Keeps track of login dates.
    /// </summary>
    public ObservableCollection<DateTime> LoginHistory { get; }
}