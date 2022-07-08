using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PChat.API.Client;
using ReactiveUI;

namespace PChat.GUI.Controls;

public partial class CAGPanel : UserControl
{
    private readonly object _addContactButtonContent;

    public CAGPanel()
    {
        InitializeComponent();
        _addContactButtonContent = this.FindControl<Button>("AddContactButton").Content;
        RemoveContactCommand =
            ReactiveCommand.Create(async () => await new EasyApiClient(true).RemoveContact(SelectedContact.Card.Id));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #region Properties

    public ObservableCollection<ContactViewModel> Contacts
    {
        get => GetValue(ContactsProperty);
        set => SetValue(ContactsProperty, value);
    }

    public ContactViewModel SelectedContact
    {
        get => GetValue(SelectedContactProperty);
        set => SetValue(SelectedContactProperty, value);
    }

    public ObservableCollection<Group> Groups
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

    public static readonly StyledProperty<ObservableCollection<ContactViewModel>> ContactsProperty =
        AvaloniaProperty.Register<CAGPanel, ObservableCollection<ContactViewModel>>(nameof(Contacts));

    public static readonly StyledProperty<ContactViewModel> SelectedContactProperty =
        AvaloniaProperty.Register<CAGPanel, ContactViewModel>(nameof(SelectedContact));

    public static readonly StyledProperty<ObservableCollection<Group>> GroupsProperty =
        AvaloniaProperty.Register<CAGPanel, ObservableCollection<Group>>(nameof(Groups));

    public static readonly StyledProperty<Group> SelectedGroupProperty =
        AvaloniaProperty.Register<CAGPanel, Group>(nameof(SelectedGroup));

    #endregion

    public ICommand RemoveContactCommand { get; set; }

    #region Event Handlers

    private async void AddContact_Tapped(object? sender, RoutedEventArgs e)
    {
        var addContactBox = this.FindControl<Border>("AddContactBox");
        addContactBox.IsVisible = !addContactBox.IsVisible;
        var addContactButton = this.FindControl<Button>("AddContactButton");
        addContactButton.IsVisible = !addContactButton.IsVisible;
        if (sender is Button btn && Equals(btn, addContactButton)) return;
        addContactButton.Content = "✔️";

        var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
        timer.Tick += (_, _) =>
        {
            addContactButton.Content = _addContactButtonContent;
            timer.Stop();
        };

        if (string.IsNullOrWhiteSpace(AddContactIdHexString)) return;
        await new EasyApiClient(true).AddContact(HexString.ToByteString(AddContactIdHexString)!);
    }

    #endregion

    private async void RemoveContact_OnClick(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine($"[DEBUG] {nameof(RemoveContact_OnClick)} called");
        await new EasyApiClient(true).RemoveContact(SelectedContact.Card.Id);
    }
}