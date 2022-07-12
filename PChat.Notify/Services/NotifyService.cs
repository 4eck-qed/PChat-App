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
        Session.Contacts.Add(contact);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> Unfriended(User user, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(Unfriended)} called");

        var unfriendedUser = Session.Contacts.FirstOrDefault(u => u.Id == user.Id);
        if (unfriendedUser != null)
            Session.Contacts.Remove(unfriendedUser);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> NewFriendRequest(FriendRequest friendRequest,
        ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewFriendRequest)} called");

        Session.FriendRequests.Add(friendRequest);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.FriendRequests)));
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
        var contact = Session.Contacts.FirstOrDefault(x => x.Id == friendRequest.TargetId);
        if (contact == null)
        {
            _logger.LogError(
                "[Notify] A Friend Request was answered but there has not been a placeholder added in the contact list!");
            throw new ArgumentNullException($"{nameof(contact)}");
        }

        switch (friendRequest.Status)
        {
            case FriendRequestStatus.Accepted:
                contact.Name = "?";
                contact.Avatar = ByteString.Empty;
                contact.Status = "?";
                Session.Conversations.Add(new Conversation(contact));
                break;
            case FriendRequestStatus.Rejected:
                Session.Contacts.Remove(contact);
                break;
        }

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(contact)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> NewMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewMessage)} called");

        var contact = Session.Contacts.FirstOrDefault(x => x.Id == message.SenderId);

        var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == message.SenderId);
        if (conversation == null)
        {
            conversation = new Conversation(contact!);
            Session.Conversations.Add(conversation);
        }

        conversation.Messages.Add(message);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> SeenMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(SeenMessage)} called");

        var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        conversation!.Messages.FirstOrDefault(x => x.Id == message.Id)!.Status = TextMessage.Types.Status.Seen;
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> ReceivedMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(ReceivedMessage)} called");

        var conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == message.ReceiverId);
        conversation!.Messages.FirstOrDefault(x => x.Id == message.Id)!.Status = TextMessage.Types.Status.Received;
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Conversations)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> DequeuedMessage(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(ReceivedMessage)} called");
        
        var dequeued = Session.QueuedMessages.FirstOrDefault(x => x.Id == message.Id);
        if (dequeued == null)
        {
            _logger.LogError($"[Notify] (de-)queued message was not found in session");
            return Task.FromResult(new ClientStatusResponse());
        }

        Session.QueuedMessages.Remove(dequeued);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.QueuedMessages)));

        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> ReceivedContactCard(ContactCard newCard, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(ReceivedContactCard)} called");

        var oldCard = Session.Contacts.FirstOrDefault(x => x.Id == newCard.Id);
        if (oldCard is null)
        {
            _logger.LogError($"[Notify] Error in {nameof(ReceivedContactCard)}: Contact is null!");
            return Task.FromResult(new ClientStatusResponse());
        }

        oldCard.MergeFrom(newCard);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(Session.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }
}