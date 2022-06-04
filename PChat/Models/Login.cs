using Google.Protobuf;

namespace PChat;

public class Login
{
    public ByteString Id { get; set; }
    public ByteString Key { get; set; }
}