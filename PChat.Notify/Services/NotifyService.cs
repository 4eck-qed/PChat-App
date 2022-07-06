using System.Collections.ObjectModel;
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

    public override Task<ClientStatusResponse> FriendRequestReceived(FriendRequest friendRequest, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(FriendRequestReceived)} called");
        
        SessionContent.FriendRequests.Add(friendRequest);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> FriendRequestAnswered(FriendRequest friendRequest, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(FriendRequestAnswered)} called");

        switch (friendRequest.Status)
        {
            case FriendRequestStatus.Accepted:
                SessionContent.Contacts.Add(new ContactCard
                {
                    Id = friendRequest.TargetId,
                    Name = "placeholder",
                    Status = "placeholder"
                });
                break;
            case FriendRequestStatus.Rejected:
                SessionContent.Contacts.Remove(SessionContent.Contacts.FirstOrDefault(x => x.Id == friendRequest.TargetId)!);
                break;
        }

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> MessageReceived(TextMessage message, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(MessageReceived)} called");

        if (message.SenderId == SessionContent.Account.Id)
        {
            // SessionContent.Messages[request.SenderId].FirstOrDefault(x => x.Id == request.Id).Status == Received;
        }
        else
        {
            // not a friend
            if (!SessionContent.Contacts.Select(x => x.Id).Contains(message.SenderId))
                return Task.FromResult(new ClientStatusResponse());
            
            if (!SessionContent.Messages.ContainsKey(message.SenderId))
                SessionContent.Messages.Add(message.SenderId, new ObservableCollection<TextMessage>());
            
            SessionContent.Messages[message.SenderId].Add(message);
        }
        
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Messages)));
        return Task.FromResult(new ClientStatusResponse());
    }
}