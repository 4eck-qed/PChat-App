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

    public override Task<ClientStatusResponse> NewFriend(User request, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(NewFriend)} called");

        var contact = new ContactCard {Id = request.Id};
        SessionContent.Contacts.Add(contact);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> Unfriended(User request, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(Unfriended)} called");

        var unfriendedUser = SessionContent.Contacts.FirstOrDefault(u => u.Id == request.Id);
        if (unfriendedUser != null)
            SessionContent.Contacts.Remove(unfriendedUser);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> FriendRequestReceived(FriendRequest request, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(FriendRequestReceived)} called");

        var friendRequest = new FriendRequest
        {
            Id = request.Id,
            Status = request.Status,
            SenderId = request.SenderId,
            TargetId = request.TargetId,
        };
        SessionContent.FriendRequests.Add(friendRequest);
        
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> FriendRequestAnswered(FriendRequest request, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(FriendRequestAnswered)} called");

        // SessionContent.FriendRequests.FirstOrDefault(f => f.Id == request.Id)!.Status = request.Status;
        if (request.Status == FriendRequestStatus.Accepted)
        {
            SessionContent.Contacts.Add(new ContactCard
            {
                Id = request.TargetId,
                Name = "placeholder",
                Status = "placeholder"
            });
        }

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Contacts)));
        return Task.FromResult(new ClientStatusResponse());
    }

    public override Task<ClientStatusResponse> MessageReceived(TextMessage request, ServerCallContext context)
    {
        if (Global.Debug)
            _logger.LogDebug($"[Notify] {nameof(MessageReceived)} called");

        var message = new TextMessage
        {
            Id = request.Id,
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            Time = request.Time,
        };
        SessionContent.Messages[request.SenderId].Add(message);

        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.Messages)));
        return Task.FromResult(new ClientStatusResponse());
    }
}