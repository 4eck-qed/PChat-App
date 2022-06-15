using System.Collections.ObjectModel;
using Google.Protobuf;
using Pchat;

namespace PChat;

public static class SessionContent
{
    public static Account Account { get; set; } = new();

    public static Dictionary<ByteString, ObservableCollection<TextMessage>> Messages { get; set; } = new();
    public static ObservableCollection<ContactCard> ContactList { get; set; } = new();
    public static ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
}