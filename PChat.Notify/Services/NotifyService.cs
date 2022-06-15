using Grpc.Core;
using Pchat;

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
        SessionContent.ContactList.Add(contact);

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> Unfriended(User request, ServerCallContext context)
    {
        var unfriendedUser = SessionContent.ContactList.FirstOrDefault(u => u.Id == request.Id);
        if (unfriendedUser != null)
            SessionContent.ContactList.Remove(unfriendedUser);

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

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> FriendRequestAnswered(FriendRequest request, ServerCallContext context)
    {
        SessionContent.FriendRequests.FirstOrDefault(f => f.Id == request.Id)!.Status = request.Status;

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

        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }

    public override Task<ClientStatusResponse> ArrivedAtPeer(Sendable request, ServerCallContext context)
    {
        var response = new ClientStatusResponse();
        return Task.FromResult(response);
    }
}