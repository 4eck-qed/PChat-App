namespace PChat.Shared;

public class OnObjectChangedEvent
{
    public OnObjectChangedEvent(string objectName)
    {
        ObjectName = objectName;
    }

    public string ObjectName { get; }
}