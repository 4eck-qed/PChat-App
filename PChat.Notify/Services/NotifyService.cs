using Google.Protobuf;
using Grpc.Core;
using Pchat;
using PChat.Shared;

namespace PChat.Notify.Services;

public class NotifyService : Pchat.Notify.NotifyBase
{
    private readonly ILogger<NotifyService> _logger;

    public NotifyService(ILogger<NotifyService> logger)
    {
        _logger = logger;
    }

    public override Task<ClientStatusResponse> NewFriend(User user, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewFriend)} called");

        var contact = new ContactCard {Id = user.Id};
        SessionContent.Contacts.Add(contact);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> Unfriended(User user, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(Unfriended)} called");

        var unfriendedUser = SessionContent.Contacts.FirstOrDefault(u => u.Id == user.Id);
        if (unfriendedUser != null)
            SessionContent.Contacts.Remove(unfriendedUser);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> NewFriendRequest(FriendRequest friendRequest,
        ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewFriendRequest)} called");

        SessionContent.FriendRequests.Add(friendRequest);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> ReceivedFriendRequest(FriendRequest friendRequest,
        ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(ReceivedFriendRequest)} called");

        throw new NotImplementedException();
    }

    public override Task<ClientStatusResponse> AnsweredFriendRequest(FriendRequest friendRequest,
        ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(AnsweredFriendRequest)} called");
        var contact = SessionContent.Contacts.FirstOrDefault(x => x.Id == friendRequest.TargetId);
        if (contact == null)
        {
            _logger.LogError(
                "[Notify] A Friend Request was answered but there has not been a placeholder added in the contact list!");
            throw new ArgumentNullException($"{nameof(contact)}");
        }

        switch (friendRequest.Status)
        {
            case FriendRequestStatus.Accepted:
                contact.Avatar = ByteString.Empty;
                SessionContent.Conversations.Add(new Conversation(contact));
                break;
            case FriendRequestStatus.Rejected:
                SessionContent.Contacts.Remove(contact);
                break;
        }

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> NewMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewMessage)} called");

        var contact = SessionContent.Contacts.FirstOrDefault(x => x.Id == message.SenderId);

        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == message.SenderId);
        if (conversation == null)
        {
            conversation = new Conversation(contact!);
            SessionContent.Conversations.Add(conversation);
        }

        conversation.Messages.Add(message);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> SeenMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(SeenMessage)} called");

        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        conversation!.Messages.FirstOrDefault(x => x.Id == message.Id)!.Status = TextMessage.Types.Status.Seen;
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> ReceivedMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(ReceivedMessage)} called");

        var conversation = SessionContent.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        conversation!.Messages.FirstOrDefault(x => x.Id == message.Id)!.Status = TextMessage.Types.Status.Received;
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }
}