using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Pchat;
using PChat.Shared;

namespace PChat.GUI.Controls;

public partial class CAGPanel : UserControl
{
    private object _addContactButtonContent;
    
    public CAGPanel()
    {
        InitializeComponent();
        _addContactButtonContent = this.FindControl<Button>("AddContactButton").Content;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #region Properties

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

    public ICollection<Group> Groups
    {
        get => GetValue(GroupsProperty);
        set => SetValue(GroupsProperty, value);
    }

    public Group SelectedGroup
    {
        get => GetValue(SelectedGroupProperty);
        set => SetValue(SelectedGroupProperty, value);
    }

    public static string AddContactIdHexString { get; set; }

    public static readonly StyledProperty<ICollection<ContactCard>> ContactsProperty =
        AvaloniaProperty.Register<CAGPanel, ICollection<ContactCard>>(nameof(Contacts));

    public static readonly StyledProperty<ContactCard> SelectedContactProperty =
        AvaloniaProperty.Register<CAGPanel, ContactCard>(nameof(SelectedContact));

    public static readonly StyledProperty<ICollection<Group>> GroupsProperty =
        AvaloniaProperty.Register<CAGPanel, ICollection<Group>>(nameof(Groups));

    public static readonly StyledProperty<Group> SelectedGroupProperty =
        AvaloniaProperty.Register<CAGPanel, Group>(nameof(SelectedGroup));

    #endregion

    #region Event Handlers

    private async void AddContact_Tapped(object? sender, RoutedEventArgs e)
    {
        var addContactBox = this.FindControl<Border>("AddContactBox");
        addContactBox.IsVisible = !addContactBox.IsVisible;
        var addContactButton = this.FindControl<Button>("AddContactButton");
        addContactButton.IsVisible = !addContactButton.IsVisible;
        addContactButton.Content = "✔️";
        
        var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
        timer.Tick += (_, _) =>
        {
            addContactButton.Content = _addContactButtonContent;
            timer.Stop();
        };
        
        // Friends.Add(contact);
        // Chats.Add(new ChatViewModel(Shared, contact));
        if (string.IsNullOrWhiteSpace(AddContactIdHexString)) return;
        await SessionContent.Client.AddContact(HexString.ToByteString(AddContactIdHexString)!);
        // WhenAnyMixin
        //     .WhenAnyValue(
        //         x => AddContactIdHexString, HexString.IsValid).AsObservable());
    }

    #endregion
}