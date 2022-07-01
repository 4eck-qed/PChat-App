using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Pchat;
using PChat.API.Client;
using PChat.Extensions;
using PChat.Shared;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

/// <summary>
/// Contains everything that needs to be shared between view models. <br/>
/// Q: Why is this a view model? <br/>
/// A: Because properties need the ReactiveUI functionalities to update the view.
/// </summary>
public class SharedViewModel : ViewModelBase
{
    #region Fields

    private ObservableCollection<DateTime> _loginHistory;
    private ConcurrentQueue<TextMessage> _messageQueue;

    #endregion

    public SharedViewModel(Client client)
    {
        Client = client;
        LoginHistory = new ObservableCollection<DateTime>(LoginHistoryService.GetLoginHistory());
        MessageQueue = new ConcurrentQueue<TextMessage>(); // TODO load from file
    }

    public readonly Client Client;

    #region View-Relevant Properties

    public ObservableCollection<DateTime> LoginHistory
    {
        get => _loginHistory;
        set => this.RaiseAndSetIfChanged(ref _loginHistory, value);
    }

    public ConcurrentQueue<TextMessage> MessageQueue
    {
        get => _messageQueue;
        set => this.RaiseAndSetIfChanged(ref _messageQueue, value);
    }

    #endregion
}