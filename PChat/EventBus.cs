using System.Reflection;

namespace PChat;

// yoinked from https://stackoverflow.com/a/23489097
public class EventBus
{
    private static EventBus _instance;

    private EventBus()
    {
    }

    private readonly List<EventListenerWrapper> _listeners = new();

    public static EventBus Instance => _instance ??= new EventBus();

    public void Register(object listener)
    {
        if (_listeners.All(l => l.Listener != listener))
            _listeners.Add(new EventListenerWrapper(listener));
    }

    public void Unregister(object listener)
    {
        _listeners.RemoveAll(l => l.Listener == listener);
    }

    public void PostEvent(object e)
    {
        Console.WriteLine("[DEBUG] Posted event '{0}'", e.GetType());
        _listeners.Where(l => l.EventType == e.GetType()).ToList().ForEach(l => l.PostEvent(e));
    }

    private class EventListenerWrapper
    {
        private readonly MethodBase? _method;

        public EventListenerWrapper(object listener)
        {
            Listener = listener;

            var type = listener.GetType();

            _method = type.GetMethod("OnEvent");
            if (_method == null)
                throw new ArgumentException("Class " + type.Name + " does not contain method OnEvent");

            var parameters = _method.GetParameters();
            if (parameters.Length != 1)
                throw new ArgumentException(
                    $"Method OnEvent of class {type.Name} has invalid number of parameters (should be one)");

            EventType = parameters[0].ParameterType;
        }

        public void PostEvent(object e)
        {
            _method?.Invoke(Listener, new[] {e});
        }

        public object Listener { get; }
        public Type EventType { get; }
    }
}