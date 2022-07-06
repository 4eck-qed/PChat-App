using System.Collections.ObjectModel;
using Google.Protobuf;
using Pchat;

namespace PChat.Shared;

/// <summary>
/// Contains everything that is relevant and bound to this session. <br/>
/// Please leave manipulation of these objects to the gRPC interfaces <br/>
/// - meaning, every class that is a gRPC service or communicates with one.
/// </summary>
public class SessionContent
{
    public static readonly SessionContent Singleton = new();
    public static Account Account { get; set; } = new();
    public static ObservableCollection<ContactCard> Contacts { get; set; } = new();
    public static ObservableCollection<ContactCard> Groups { get; set; } = new();
    public static Dictionary<ByteString, ObservableCollection<TextMessage>> Messages { get; set; } = new();
    public static ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
}