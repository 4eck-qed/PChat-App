using System.Collections.ObjectModel;
using Google.Protobuf;
using Pchat;

namespace PChat.Shared;

public class SessionContent
{
    public static SessionContent Singleton = new();
    public static Account Account { get; set; } = new();
    public static ObservableCollection<ContactCard> Contacts { get; set; } = new();
    public static ObservableCollection<ContactCard> Groups { get; set; } = new();
    public static Dictionary<ByteString, ObservableCollection<TextMessage>> Messages { get; set; } = new();
    public static ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
}