using System.Collections.ObjectModel;
using Google.Protobuf;
using Pchat;
using PChat.API.Client;

namespace PChat.Shared;

public class SessionContent
{
    public static SessionContent Singleton = new();
    public static Account Account { get; set; } = new();
    public static Client Client { get; set; } = new(true);
    public static ObservableCollection<ContactCard> Contacts { get; set; } = new();
    public static ObservableCollection<ContactCard> Groups { get; set; } = new();
    public static Dictionary<ByteString, ObservableCollection<TextMessage>> Messages { get; set; } = new();
    public static ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
}