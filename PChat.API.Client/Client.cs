using System.Net;
using Google.Protobuf;
using Grpc.Net.Client;
using Pchat;
using PChat.Extensions;
using PChat.Shared;

namespace PChat.API.Client;

public class Client
{
    private bool _isUnsecure;

    /// <summary>
    /// Returns an Instance with no credentials.
    /// </summary>
    /// <param name="isUnsecure"></param>
    public Client(bool isUnsecure)
    {
        IsUnsecure = isUnsecure;
    }

    #region Private

    private static ByteString Parse(IEnumerable<byte> bytes) => ByteString.CopyFrom(bytes.ToArray());

    #endregion

    /// <summary>
    /// Get all messages that you send or received to/from this user id.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TextMessage>> GetMessages(ByteString userId)
    {
        return null; // TBI
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
        Console.WriteLine($"Trying to ping {contact.Name}..");
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.PingAsync(new User {Id = contact.Id});
        Console.WriteLine(response.Status == PeerResponse.Types.Status.Received ? "Success!" : "Failed.");
        return response.Status == PeerResponse.Types.Status.Received;
    }

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="messageToBeRemoved"></param>
    public async Task<bool> SendMessage(TextMessage message)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.SendMessageAsync(message);
        return response.Status == PeerResponse.Types.Status.Received;
    }

    public async Task AddContact(ByteString id)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var user = new User {Id = id};
        var response = await client.AddFriendAsync(user);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
    }

    public async Task AcceptFriendRequest(FriendRequest friendRequest)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        friendRequest.Status = FriendRequestStatus.Accepted;
        var response = await client.AcceptFriendRequestAsync(friendRequest);
    }
    
    public async Task RejectFriendRequest(FriendRequest friendRequest)
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        friendRequest.Status = FriendRequestStatus.Rejected;
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