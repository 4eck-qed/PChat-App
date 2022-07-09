using Google.Protobuf;
using JetBrains.Annotations;
using Pchat;
using PChat.Shared;
using ReactiveUI;

namespace PChat.GUI;

public class ProfileViewModel : ViewModelBase
{
    private Account _account;
    private string _name;
    private ByteString _avatar;
    private string _status;

    public ProfileViewModel(Account account)
    {
        Account = account;
    }

    [UsedImplicitly]
    public void OnEvent(OnObjectChangedEvent e)
    {
        if (e.ObjectName is not (nameof(Session.Account) and nameof(Account))) return;
        Account = Session.Account;
    }

    public Account? Account
    {
        get => _account;
        private set
        {
            if (value is null) return;
            Name = value.Name;
            Avatar = value.Avatar;
            Status = value.Status;
            this.RaiseAndSetIfChanged(ref _account, value);
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (Account != null) Account.Name = value;
            this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public ByteString Avatar
    {
        get => _avatar;
        set
        {
            if (Account != null) Account.Avatar = value;
            this.RaiseAndSetIfChanged(ref _avatar, value);
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            if (Account != null) Account.Status = value;
            this.RaiseAndSetIfChanged(ref _status, value);
        }
    }
}