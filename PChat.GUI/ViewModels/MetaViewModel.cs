using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PChat.Notify;
using PChat.Shared;
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
        EventBus.Instance.Register(this);
    }

    public void OnEvent(object e)
    {
        switch (e)
        {
            case OnObjectChangedEvent oe:
                Console.WriteLine("OnObjectChangedEvent '{0}'", oe.ObjectName);
                this.RaisePropertyChanged(oe.ObjectName);
                break;
            default:
                throw new UnsupportedContentTypeException($"{e.GetType()} is not supported.");
        }
    }

    public DateTime GetLastLogin() => LoginHistory.Any() ? LoginHistory.Last() : DateTime.Now;

    /// <summary>
    /// Keeps track of login dates.
    /// </summary>
    public ObservableCollection<DateTime> LoginHistory { get; }
}