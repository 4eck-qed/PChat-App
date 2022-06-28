using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pchat;

namespace PChat.GUI.Controls;

public partial class FriendList : UserControl
{
    public FriendList()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ICollection<ContactCard> Contacts
    {
        get => GetValue(ContactsProperty);
        set => SetValue(ContactsProperty, value);
    }

    public ContactCard SelectedContact
    {
        get => GetValue(SelectedContactProperty);
        set => SetValue(SelectedContactProperty, value);
    }

    public static readonly StyledProperty<ICollection<ContactCard>> ContactsProperty =
        AvaloniaProperty.Register<FriendList, ICollection<ContactCard>>(nameof(Contacts));

    public static readonly StyledProperty<ContactCard> SelectedContactProperty =
        AvaloniaProperty.Register<FriendList, ContactCard>(nameof(SelectedContact));
}