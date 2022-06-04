using System.Collections.Concurrent;
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

    private ContactCard _profile;
    private ConcurrentQueue<Message> _messageQueue;

    #endregion

    public SharedViewModel(Login login, ContactCard profile, ApiClient apiClient, ConcurrentQueue<Message> messageQueue)
    {
        Login = login;
        IdHexString = login.Id.ToHexString();
        KeyHexString = login.Key.ToHexString();
        Profile = profile;
        ApiClient = apiClient;
        MessageQueue = messageQueue;
    }
    
    public readonly Login Login;
    public readonly ApiClient ApiClient;
    public LoginHistoryService LoginHistoryService { get; } = new LoginHistoryService();
    
    #region View-Relevant Properties

    public ContactCard Profile
    {
        get => _profile;
        set => this.RaiseAndSetIfChanged(ref _profile, value);
    }

    public ConcurrentQueue<Message> MessageQueue
    {
        get => _messageQueue;
        set => this.RaiseAndSetIfChanged(ref _messageQueue, value);
    }
    
    public string IdHexString { get; }
    public string KeyHexString { get; }

    #endregion
}