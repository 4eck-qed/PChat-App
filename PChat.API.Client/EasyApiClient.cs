using System.Collections.ObjectModel;
using System.Net;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Pchat;
using PChat.Extensions;
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

    /// <summary>
    /// Loads the conversation with given contact. <br/>
    /// Automatically adds the conversation to the session.
    /// </summary>
    /// <returns></returns>
    public async Task<Conversation> LoadConversation(ContactCard contact)
    {
        if (SessionContent.Conversations.All(x => x.Contact.Id != contact.Id))
            SessionContent.Conversations.Add(new Conversation(contact));

        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var filter = new MessageFilter();
        var response = await client.GetMessagesAsync(filter);
        var messages = response.Items.ToList();
        
        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == contact.Id);
        if (messages?.Any() == false)
        {
            return conversation ?? throw new InvalidDataException("No conversation for that contact!");
        }
        
        conversation!.Messages = new ObservableCollection<TextMessage>(messages!.OrderBy(m => m.Time));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));

        return conversation;
    }

    public async Task<bool> Login(Credentials credentials)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.LoginAsync(credentials);
        return response.Id != null && response.Key != null;
    }

    /// <summary>
    /// Get your credentials.
    /// </summary>
    /// <returns></returns>
    public async Task<Account> CreateAccount()
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.CreateAccountAsync(new Empty());
        Console.WriteLine("GetCredentials returned: {0}:{1}", response.Account.Id.ToHexString(),
            response.Account.Key.ToHexString());
        return new Account {Id = response.Account.Id, Key = response.Account.Key};
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

        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        if (conversation is null)
            throw new ArgumentException("Conversation does not exist!");

        conversation.Messages.Add(message);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));

        var response = await client.SendMessageAsync(message);
        return response.Status == PeerResponse.Types.Status.Received;
    }

    public async Task AddContact(ByteString id)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var user = new User {Id = id};
        SessionContent.Contacts.Add(new ContactCard
        {
            Id = id,
            Avatar = ByteString.CopyFromUtf8("avares://PChat.GUI/Assets/Images/avatar_unknown.png")
        });
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));

        var response = await client.AddFriendAsync(user);
    }

    public async Task AcceptFriendRequest(FriendRequest friendRequest)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        friendRequest.Status = FriendRequestStatus.Accepted;
        var contact = new ContactCard
        {
            Id = friendRequest.SenderId,
        };
        SessionContent.Contacts.Add(contact);
        SessionContent.FriendRequests.Remove(friendRequest);
        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == contact.Id);
        if (conversation == null)
        {
            conversation = new Conversation(contact);
            SessionContent.Conversations.Add(conversation);
        }
        
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));

        var response = await client.AcceptFriendRequestAsync(friendRequest);
    }

    public async Task RejectFriendRequest(FriendRequest friendRequest)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        friendRequest.Status = FriendRequestStatus.Rejected;
        SessionContent.FriendRequests.Remove(friendRequest);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));

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

    private static string ExternalIp => new WebClient().DownloadString("https://ipinfo.io/ip")
        .Replace("\\r\\n", "").Replace("\\n", "").Trim();

    private string Host => $"{ApiIp}:{ApiPort}";
}