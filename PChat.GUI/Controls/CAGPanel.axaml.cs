using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DynamicData.Binding;
using Google.Protobuf;
using Pchat;
using PChat.API.Client;
using PChat.Extensions;
using ReactiveUI;

namespace PChat.GUI.Controls;

// ReSharper disable once InconsistentNaming, PartialTypeWithSinglePart
public partial class CAGPanel : UserControl
{
    public CAGPanel()
    {
        InitializeComponent();
        RemoveContactCommand = ReactiveCommand.Create(async () =>
        {
            if (SelectedContact is {Card: { }})
                await new EasyApiClient(true).RemoveContact(SelectedContact.Card.Id);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #region Properties

    public ObservableCollectionExtended<ContactViewModel> ContactsPreview { get; set; } = new()
    {
        {
            new ContactViewModel(new ContactCard
            {
                Id = ByteStringExtensions.RandomByteString(16),
                Name = "Test",
                Avatar = ByteString.Empty,
                Status = "testing"
            })
        }
    };

    public ObservableCollection<ContactViewModel> Contacts
    {
        get => GetValue(ContactsProperty);
        set => SetValue(ContactsProperty, value);
    }

    public ContactViewModel? SelectedContact
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

    public string AddContactHexId
    {
        get => _addContactHexId;
        set => SetAndRaise(AddContactHexIdProperty, ref _addContactHexId, value);
    }

    public static readonly DirectProperty<CAGPanel, string> AddContactHexIdProperty =
        AvaloniaProperty.RegisterDirect<CAGPanel, string>(nameof(AddContactHexId),
            o => o.AddContactHexId,
            (o, v) => o.AddContactHexId = v);

    public static readonly StyledProperty<ObservableCollection<ContactViewModel>> ContactsProperty =
        AvaloniaProperty.Register<CAGPanel, ObservableCollection<ContactViewModel>>(nameof(Contacts));

    public static readonly StyledProperty<ContactViewModel?> SelectedContactProperty =
        AvaloniaProperty.Register<CAGPanel, ContactViewModel?>(nameof(SelectedContact));

    public static readonly StyledProperty<ObservableCollection<Group>> GroupsProperty =
        AvaloniaProperty.Register<CAGPanel, ObservableCollection<Group>>(nameof(Groups));

    public static readonly StyledProperty<Group> SelectedGroupProperty =
        AvaloniaProperty.Register<CAGPanel, Group>(nameof(SelectedGroup));

    private static string _addContactHexId;

    #endregion

    public ICommand RemoveContactCommand { get; set; }

    #region Event Handlers

    private static void FlipVisibility(params IControl[] controls)
    {
        foreach (var control in controls)
            control.IsVisible = !control.IsVisible;
    }

    private static void ChangeToContentTemporarily(IContentControl control, object? content, TimeSpan timeSpan)
    {
        var oldContent = control.Content;
        control.Content = content;
        var timer = new DispatcherTimer {Interval = timeSpan, IsEnabled = true};
        timer.Tick += (_, _) =>
        {
            control.Content = oldContent;
            timer.Stop();
        };
    }

    // ReSharper disable once UnusedParameter.Local
    private async void AddContact_Tapped(object? sender, RoutedEventArgs e)
    {
        var addContactBox = this.FindControl<Border>("AddContactBox");
        var addContactButton = this.FindControl<Button>("AddContactButton");
        FlipVisibility(addContactBox, addContactButton);

        if (sender is Button btn && Equals(btn, addContactButton) || string.IsNullOrWhiteSpace(AddContactHexId)) return;
        var id = HexString.ToByteString(AddContactHexId);
        AddContactHexId = string.Empty;
        if (id == null) return;
        await new EasyApiClient(true).AddContact(id);
        ChangeToContentTemporarily(addContactButton, "✔️", TimeSpan.FromSeconds(1));
    }

    #endregion

    // ReSharper disable UnusedParameter.Local
    private async void RemoveContact_OnClick(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine($"[DEBUG] {nameof(RemoveContact_OnClick)} called");
        if (SelectedContact == null)
        {
            Console.WriteLine("\t...but nothing was selected.");
            return;
        }

        if (SelectedContact.Card == null)
        {
            Console.WriteLine("\t...Card of SelectedContact is null!");
            return;
        }

        var id = SelectedContact.Card.Id;
        SelectedContact = null;
        await new EasyApiClient(true).RemoveContact(id);
    }

    private void CopyId_OnClick(object? sender, RoutedEventArgs e)
    {
        if (SelectedContact is {Card: { }})
            CopyToClipboard(SelectedContact.Card.Id.ToHexString());
    }

    private void CopyName_OnClick(object? sender, RoutedEventArgs e)
    {
        if (SelectedContact != null)
            CopyToClipboard(SelectedContact.Name);
    }

    private static void CopyToClipboard(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        try
        {
            Application.Current.Clipboard.SetTextAsync(text);
        }
        catch (Exception)
        {
            // do nothing
        }
    }

    private void AddContactCancel_Tapped(object? sender, RoutedEventArgs e)
    {
        var addContactBox = this.FindControl<Border>("AddContactBox");
        var addContactButton = this.FindControl<Button>("AddContactButton");
        FlipVisibility(addContactBox, addContactButton);
        AddContactHexId = string.Empty;
    }
}