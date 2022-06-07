using System.Globalization;
using System.Net;
using Google.Protobuf;
using Grpc.Net.Client;
using Pchat;
using PChat.Extensions;

namespace PChat.API.Client;

public class ApiClient
{
    private bool _isUnsecure;

    /// <summary>
    /// Returns an Instance with no credentials.
    /// </summary>
    /// <param name="isUnsecure"></param>
    public ApiClient(bool isUnsecure)
    {
        IsUnsecure = isUnsecure;
    }

    /// <summary>
    /// Returns an Instance with given login credentials.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="isUnsecure"></param>
    public ApiClient(Login login, bool isUnsecure)
    {
        Login = login;
        IsUnsecure = isUnsecure;
    }

    #region Private

    private static ByteString Parse(IEnumerable<byte> bytes) => ByteString.CopyFrom(bytes.ToArray());

    #endregion

    /// <summary>
    /// Get all messages that you send or received to/from this user id.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Message>> GetMessages(ByteString userId)
    {
        return null; // TBI
    }

    /// <summary>
    /// Gets all messages that you have received from this user id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Message>> GetReceivedMessages(ByteString userId)
    {
        return null; // TBI
    }

    /// <summary>
    /// Gets all messages that you have send to this user id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Message>> GetSendMessages(ByteString userId)
    {
        return null; // TBI
    }

    /// <summary>
    /// Get your credentials.
    /// </summary>
    /// <returns></returns>
    public async Task<Login> CreateAccount()
    {
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.CreateAccountAsync(new Empty());
        Console.WriteLine("GetCredentials returned: {0}:{1}", response.Account.Id.ToHexString(),
            response.Account.Key.ToHexString());
        return new Login {Id = response.Account.Id, Key = response.Account.Key};
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
    /// <param name="message"></param>
    public async Task<bool> Send(Message message)
    {
        var request = new TextMessage
        {
            Id = message.Id,
            SenderId = message.Sender.Id,
            ReceiverId = message.Receiver.Id,
            Content = message.Content,
            Time = message.Time.ToString(CultureInfo.CurrentCulture)
        };
        var channel = GrpcChannel.ForAddress(Host);
        var client = new Api.ApiClient(channel);
        var response = await client.SendMessageAsync(request);
        return response.Status == PeerResponse.Types.Status.Received;
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

    public Login Login { get; }

    private string ApiIp { get; set; }
    private const int ApiPort = 50051;

    private static string ExternalIp => new WebClient().DownloadString("https://ipinfo.io/ip")
        .Replace("\\r\\n", "").Replace("\\n", "").Trim();

    private string Host => $"{ApiIp}:{ApiPort}";
}