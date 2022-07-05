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
        var contact = new ContactCard {Id = request.Id};
        SessionContent.Contacts.Add(contact);

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> Unfriended(User request, ServerCallContext context)
    {
        var unfriendedUser = SessionContent.Contacts.FirstOrDefault(u => u.Id == request.Id);
        if (unfriendedUser != null)
            SessionContent.Contacts.Remove(unfriendedUser);

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> FriendRequestReceived(FriendRequest request, ServerCallContext context)
    {
        var friendRequest = new FriendRequest
        {
            Id = request.Id,
            Status = request.Status,
            SenderId = request.SenderId,
            TargetId = request.TargetId,
        };
        SessionContent.FriendRequests.Add(friendRequest);
        EventBus.Instance.PostEvent(new OnObjectChangedEvent(nameof(SessionContent.FriendRequests)));

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> FriendRequestAnswered(FriendRequest request, ServerCallContext context)
    {
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

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> MessageReceived(TextMessage request, ServerCallContext context)
    {
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

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }
}