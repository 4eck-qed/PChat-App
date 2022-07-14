using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Pchat;
using PChat.Config;
using PChat.Shared;
using PChat.Log;

namespace PChat.API.Client;

public class EasyApiClient
{
    private readonly ILogger _logger;
    private bool _isUnsecure;
    private string _apiIp;
    private string Host => $"{_apiIp}:{Global.ApiPort}";

    public static readonly EasyApiClient Instance = new(true);

    /// <summary>
    /// Returns an Instance with no credentials.
    /// </summary>
    /// <param name="isUnsecure"></param>
    public EasyApiClient(bool isUnsecure)
    {
        IsUnsecure = isUnsecure;
        _logger = LoggerFactory.Create(o =>
        {
            o.AddConsole();
            o.AddFile($"{Global.DataDir}");
        }).CreateLogger<EasyApiClient>();
    }

    public async Task LoadContacts()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.GetContactsAsync(new Empty());
            Session.Contacts = new ObservableCollection<ContactCard>(response.Items);
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(LoadContacts), e.ToString());
        }
    }

    public async Task LoadFriendRequests()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.GetFriendRequestsAsync(new Empty());
            Session.FriendRequests = new ObservableCollection<FriendRequest>(response.Items);
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(LoadFriendRequests), e.ToString());
        }
    }

    /// <summary>
    /// Loads the conversation with given contact. <br/>
    /// Automatically adds the conversation to the session.
    /// </summary>
    /// <returns></returns>
    public async Task<Conversation> LoadConversation(ContactCard contact)
    {
        var newConversation = new Conversation(contact);
        if (Session.Conversations.All(x => x.Contact.Id != contact.Id))
            Session.Conversations.Add(newConversation);

        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.GetMessagesAsync(new MessageFilter());
            var messages = response.Items.ToList();

            var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == contact.Id);
            if (messages.Any() == false)
            {
                return conversation ?? throw new InvalidDataException("No conversation for that contact!");
            }

            conversation!.Messages = new ObservableCollection<TextMessage>(messages.OrderBy(m => m.Time));
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

            return conversation;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(LoadConversation), e.ToString());
            return newConversation;
        }
    }

    /// <summary>
    /// Get your credentials.
    /// </summary>
    /// <returns></returns>
    public async Task<Account?> CreateAccount()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            Session.Account = await client.CreateAccountAsync(new Empty());
            return Session.Account;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(CreateAccount), e.ToString());
            return null;
        }
    }

    /// <summary>
    /// Tries to login with given credentials. <br/>
    /// On success: initializes the account in the session.
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    public async Task<Account?> Login(Credentials credentials)
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            Session.Account = await client.LoginAsync(credentials);
            await client.AnnounceOnlineAsync(new Empty());
            return Session.Account;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(Login), e.ToString());
            return null;
        }
    }

    /// <summary>
    /// Logs out, thus killing the session.
    /// </summary>
    public async Task Logout()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            await client.AnnounceOfflineAsync(new Empty());
            await client.LogoutAsync(new Empty());
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(Logout), e.ToString());
        }
    }

    public async Task Kill()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            await client.KillAsync(new Empty());
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(Kill), e.ToString());
        }
    }

    /// <summary>
    /// Returns your login history.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TextMessage>> GetQueuedMessages()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.GetQueuedMessagesAsync(new MessageFilter());
            Session.QueuedMessages = new ObservableCollection<TextMessage>(response.Items);
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.QueuedMessages)));
            return response.Items;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(GetQueuedMessages), e.ToString());
            return new List<TextMessage>();
        }
    }

    /// <summary>
    /// Returns your login history.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<DateTime>> GetLoginHistory()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.GetLoginHistoryAsync(new Empty());
            return response.Logins.Select(DateTime.Parse);
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(GetLoginHistory), e.ToString());
            return new List<DateTime>();
        }
    }

    /// <summary>
    /// Updates your account.
    /// </summary>
    public async Task UpdateAccount()
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            await client.UpdateAccountAsync(Session.Account);
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(UpdateAccount), e.ToString());
        }
    }

    /// <summary>
    /// Pings a contact.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public async Task<bool> Ping(ContactCard contact)
    {
        try
        {
            _logger.LogDebug("Trying to ping {C}..", contact.Name);
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var response = await client.PingAsync(new User {Id = contact.Id});
            Console.WriteLine(response.Status == PeerResponse.Types.Status.Received ? "\t..Success!" : "\t..Failed.");
            return response.Status == PeerResponse.Types.Status.Received;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(Ping), e.ToString());
            return false;
        }
    }

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="message"></param>
    public async Task<SendResponse.Types.Status> SendMessage(TextMessage message)
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
            if (conversation is null)
                throw new ArgumentException("Conversation does not exist!");
            conversation.Messages.Add(message);
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

            var response = await client.SendMessageAsync(message);
            if (response.Status == SendResponse.Types.Status.Queued)
            {
                Session.QueuedMessages.Add(message);
                EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.QueuedMessages)));
            }

            return response.Status;
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(SendMessage), e.ToString());
            return SendResponse.Types.Status.Unspecified;
        }
    }

    public async Task AddContact(ByteString id)
    {
        try
        {
            if (id == Session.Account.Id)
            {
                _logger.LogError("You cannot add yourself");
                return;
            }

            var contact = Session.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact != null)
            {
                _logger.LogError("You cannot add a contact that you already have");
                return;
            }

            Session.Contacts.Add(new ContactCard
            {
                Id = id,
                Avatar = ByteString.CopyFromUtf8("avares://PChat.GUI/Assets/Images/avatar_unknown.png"),
                Name = "Pending",
                Status = "..."
            });
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var _ = await client.AddContactAsync(new User {Id = id});
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(AddContact), e.ToString());
        }
    }

    public async Task RemoveContact(ByteString id)
    {
        _logger.LogDebug("RemoveContact called");
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            var contact = Session.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null)
            {
                _logger.LogError("Tried to remove non-existent contact. Ignoring..");
                return;
            }

            Session.Contacts.Remove(contact);

            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
            await client.RemoveContactAsync(new User {Id = id});
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(RemoveContact), e.ToString());
        }
    }

    public async Task AcceptFriendRequest(FriendRequest friendRequest)
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            friendRequest.Status = FriendRequestStatus.Accepted;
            Session.Contacts.Add(friendRequest.Sender);
            Session.FriendRequests.Remove(friendRequest);
            var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == friendRequest.Sender.Id);
            if (conversation == null)
            {
                conversation = new Conversation(friendRequest.Sender);
                Session.Conversations.Add(conversation);
                EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));
            }

            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));
            var _ = await client.AcceptFriendRequestAsync(friendRequest);
            var __ = await client.SendContactCardAsync(new User {Id = friendRequest.Sender.Id});
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(AcceptFriendRequest), e.ToString());
        }
    }

    public async Task RejectFriendRequest(FriendRequest friendRequest)
    {
        try
        {
            var channel = GrpcChannel.ForAddress(Host);
            var client = new Api.ApiClient(channel);
            friendRequest.Status = FriendRequestStatus.Rejected;
            Session.FriendRequests.Remove(friendRequest);
            EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));
            var _ = await client.RejectFriendRequestAsync(friendRequest);
        }
        catch (Exception e)
        {
            _logger.LogCritical("{M} failed [{E}]", nameof(RejectFriendRequest), e.ToString());
        }
    }

    /// <summary>
    /// Whether the connection is unsecure. <br/>
    /// Necessary if TLS is not supported.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public bool IsUnsecure
    {
        get => _isUnsecure;
        set
        {
            if (value)
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                _apiIp = "http://localhost";
            }
            else
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                _apiIp = "https://localhost";
            }

            _isUnsecure = value;
        }
    }
}