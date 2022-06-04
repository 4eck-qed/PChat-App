using Google.Protobuf;

namespace PChat;

public class Message
{
    public Message()
    {
        
    }
    
    public Message(Message other)
    {
        Replace(other);
    }
    
    public void Replace(Message other)
    {
        Id = other.Id;
        Sender = other.Sender;
        Receiver = other.Receiver;
        ImageSource = other.ImageSource;
        Content = other.Content;
        Time = other.Time;
        IsNativeOrigin = other.IsNativeOrigin;
        FirstMessage = other.FirstMessage;
    }
    
    public ByteString Id { get; set; }
    
    // core 
    public ContactCard Sender { get; set; }
    public ContactCard Receiver { get; set; }
    public string Content { get; set; }
    //
    
    public string ImageSource { get; set; }
    public DateTime Time { get; set; }
    public bool? IsNativeOrigin { get; set; } = false;
    public bool? FirstMessage { get; set; }
}