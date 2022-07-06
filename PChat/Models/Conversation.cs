using System.Collections.ObjectModel;
using Pchat;

namespace PChat;

/// <summary>
/// A Conversation consists of 2 Persons. <br/>
/// In this case one person is you and the other is your contact. <br/>
/// This model keeps track of the messages that you send or received to/from this contact.
/// </summary>
public class Conversation
{
    public Conversation(ContactCard contact, ObservableCollection<TextMessage>? messages = null)
    {
        Contact = contact;
        Messages = messages ?? new ObservableCollection<TextMessage>();
    }
    public ContactCard Contact { get; set; }
    public ObservableCollection<TextMessage> Messages { get; set; }
}