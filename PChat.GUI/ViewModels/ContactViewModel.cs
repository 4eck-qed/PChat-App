using Google.Protobuf;
using Pchat;
using ReactiveUI;

namespace PChat.GUI;

/// <summary>
/// Necessary because changes of ContactCards do not update the view.
/// </summary>
public class ContactViewModel : ViewModelBase
{
    private ContactCard _card;
    private ByteString _avatar;
    private string _name;
    private string _status;

    public ContactViewModel(ContactCard card)
    {
        Card = card;
    }

    public ContactCard Card
    {
        get => _card;
        set
        {
            this.RaiseAndSetIfChanged(ref _card, value);
            Name = Card.Name;
            Avatar = Card.Avatar;
            Status = Card.Status;
        }
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ByteString Avatar
    {
        get => _avatar;
        set => this.RaiseAndSetIfChanged(ref _avatar, value);
    }

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }
}