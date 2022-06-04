using System.Collections.ObjectModel;

namespace PChat;

public class Group
{
    /// <summary>
    /// Members of this group.
    /// </summary>
    public ObservableCollection<ContactCard> Members = new ObservableCollection<ContactCard>();
    
    /// <summary>
    /// Name of this group.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Description of this group.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Messages contained in this group chat.
    /// </summary>
    public ObservableCollection<Message> Messages = new ObservableCollection<Message>();
}