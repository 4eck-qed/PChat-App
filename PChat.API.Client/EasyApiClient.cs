using System.Collections.ObjectModel;
using System.Net;
using Google.Protobuf;
using Grpc.Net.Client;
using Pchat;
using PChat.Shared;

namespace PChat.API.Client;

public class EasyApiClient
{
    private bool _isUnsecure;

    /// <summary>
    /// Returns an Instance with no credentials.
    /// </summary>
    /// <param name="isUnsecure"></param>
    public EasyApiClient(bool isUnsecure)
    {
        IsUnsecure = isUnsecure;
    }

    #region Private

    private static ByteString Parse(IEnumerable<byte> bytes) => ByteString.CopyFrom(bytes.ToArray());

    #endregion

    public async Task LoadContacts()
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.GetContactsAsync(new Empty());
        Session.Contacts = new ObservableCollection<ContactCard>(response.Items);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        // await client.AnnounceOnlineAsync(new Empty()); // TODO add setting for invisible
    }

    /// <summary>
    /// Loads the conversation with given contact. <br/>
    /// Automatically adds the conversation to the session.
    /// </summary>
    /// <returns></returns>
    public async Task<Conversation> LoadConversation(ContactCard contact)
    {
        if (Session.Conversations.All(x => x.Contact.Id != contact.Id))
            Session.Conversations.Add(new Conversation(contact));

        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var filter = new MessageFilter();
        var response = await client.GetMessagesAsync(filter);
        var messages = response.Items.ToList();

        var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == contact.Id);
        if (messages?.Any() == false)
        {
            return conversation ?? throw new InvalidDataException("No conversation for that contact!");
        }

        conversation!.Messages = new ObservableCollection<TextMessage>(messages!.OrderBy(m => m.Time));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

        return conversation;
    }

    public async Task<Account> Login(Credentials credentials)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var account = await client.LoginAsync(credentials);
        Session.Account = account;
        return account;
    }

    /// <summary>
    /// Get your credentials.
    /// </summary>
    /// <returns></returns>
    public async Task<Account> CreateAccount()
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var account = await client.CreateAccountAsync(new Empty());
        Session.Account = account;
        return account;
    }

    /// <summary>
    /// Updates your account.
    /// </summary>
    public async Task UpdateAccount()
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        await client.UpdateAccountAsync(Session.Account);
    }

    /// <summary>
    /// Pings a contact.
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public async Task<bool> Ping(ContactCard contact)
    {
        Console.WriteLine($"[DEBUG] Trying to ping {contact.Name}..");
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.PingAsync(new User {Id = contact.Id});
        Console.WriteLine(response.Status == PeerResponse.Types.Status.Received ? "\t..Success!" : "\t..Failed.");

        return response.Status == PeerResponse.Types.Status.Received;
    }

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="message"></param>
    public async Task<bool> SendMessage(TextMessage message)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);

        var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        if (conversation is null)
            throw new ArgumentException("Conversation does not exist!");

        conversation.Messages.Add(message);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

        var response = await client.SendMessageAsync(message);
        return response.Status == PeerResponse.Types.Status.Received;
    }

    public async Task AddContact(ByteString id)
    {
        if (id == Session.Account.Id)
        {
            Console.WriteLine("[ERROR] You cannot add yourself");
            return;
        }
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var contact = Session.Contacts.FirstOrDefault(x => x.Id == id);
        if (contact != null)
        {
            Console.WriteLine("[ERROR] You cannot add a contact that you already have!");
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

        var response = await client.AddContactAsync(new User {Id = id});
    }

    public async Task RemoveContact(ByteString id)
    {
        Console.WriteLine("[DEBUG] RemoveContact called.");
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var contact = Session.Contacts.FirstOrDefault(x => x.Id == id);
        if (contact == null)
        {
            Console.WriteLine("[ERROR] Tried to remove non-existent contact. Ignoring..");
            return;
        }

        Session.Contacts.Remove(contact);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        await client.RemoveContactAsync(new User {Id = id});
    }

    public async Task AcceptFriendRequest(FriendRequest friendRequest)
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
        }

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));

        var response = await client.AcceptFriendRequestAsync(friendRequest);
    }

    public async Task RejectFriendRequest(FriendRequest friendRequest)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        friendRequest.Status = FriendRequestStatus.Rejected;
        Session.FriendRequests.Remove(friendRequest);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));

        var response = await client.RejectFriendRequestAsync(friendRequest);
    }

    /// <summary>
    /// Whether the connection is unsecure. <br/>
    /// Necessary if TLS is not supported.
    /// </summary>
    public bool IsUnsecure
    {
        get => _isUnsecure;
        set
        {
            if (value)
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                ApiIp = "http://localhost";
            }
            else
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                ApiIp = "https://localhost";
            }

            _isUnsecure = value;
        }
    }

    private string ApiIp { get; set; }
    private const int ApiPort = 50051;

    [Obsolete("Obsolete")]
    private static string ExternalIp => new WebClient().DownloadString("https://ipinfo.io/ip")
        .Replace("\\r\\n", "").Replace("\\n", "").Trim();

    private string Host => $"{ApiIp}:{ApiPort}";
}