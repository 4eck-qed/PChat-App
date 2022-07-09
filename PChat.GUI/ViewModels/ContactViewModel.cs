using Google.Protobuf;
using JetBrains.Annotations;
using Pchat;
using PChat.Shared;
using ReactiveUI;

namespace PChat.GUI;

/// <summary>
/// Necessary because changes of ContactCards do not update the view.
/// </summary>
public class ContactViewModel : ViewModelBase
{
    private ContactCard _card;

    public ContactViewModel(ContactCard card)
    {
        Card = card;
        EventBus.Instance.Subscribe(this);
    }

    [UsedImplicitly]
    public void OnEvent(OnObjectChangedEvent e)
    {
        if (e.ObjectName != nameof(Session.Contacts)) return;
        this.RaisePropertyChanged(nameof(Name));
        this.RaisePropertyChanged(nameof(Avatar));
        this.RaisePropertyChanged(nameof(Status));
    }

    public ContactCard Card
    {
        get => _card;
        set
        {
            this.RaiseAndSetIfChanged(ref _card, value);
            this.RaisePropertyChanged(nameof(Name));
            this.RaisePropertyChanged(nameof(Avatar));
            this.RaisePropertyChanged(nameof(Status));
        }
    }

    public string Name => Card.Name;

    public ByteString Avatar => Card.Avatar;

    public string Status => Card.Status;
}