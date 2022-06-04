using Google.Protobuf;
using PChat.Extensions;

namespace PChat;

public class ContactCard
{
    /// <summary>
    /// Id that is provided and registered by the PChat Server.
    /// </summary>
    public ByteString Id { get; set; }

    public string IdHexString => Id.ToHexString();

    /// <summary>
    /// Name that is shown in the chat.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Color of the name that is shown in the chat.
    /// </summary>
    public string NameColor { get; set; }

    /// <summary>
    /// Avatar icon.
    /// </summary>
    public string AvatarImageSource { get; set; }

    /// <summary>
    /// Status message - e.g. "busy".
    /// </summary>
    public string Status { get; set; }
}