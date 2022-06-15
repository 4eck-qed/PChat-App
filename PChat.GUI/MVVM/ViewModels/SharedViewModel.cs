using System.Collections.Concurrent;
using Pchat;
using PChat.API.Client;
using PChat.Extensions;
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

    private ConcurrentQueue<TextMessage> _messageQueue;

    #endregion

    public SharedViewModel(ApiClient apiClient)
    {
        IdHexString = SessionContent.Account.Id.ToHexString();
        KeyHexString = SessionContent.Account.Key.ToHexString();
        ApiClient = apiClient;

        MessageQueue = new ConcurrentQueue<TextMessage>(); // TODO load from file
    }

    public readonly ApiClient ApiClient;
    public LoginHistoryService LoginHistoryService { get; } = new LoginHistoryService();

    #region View-Relevant Properties

    public ConcurrentQueue<TextMessage> MessageQueue
    {
        get => _messageQueue;
        set => this.RaiseAndSetIfChanged(ref _messageQueue, value);
    }

    public string IdHexString { get; }
    public string KeyHexString { get; }

    #endregion
}