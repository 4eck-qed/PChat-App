using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Pchat;
using PChat.API.Client;
using PChat.Shared;

namespace PChat.GUI.Controls;

public partial class FriendRequestsPanel : UserControl
{
    public FriendRequestsPanel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AcceptFriend_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button {DataContext: FriendRequest friendRequest}) return;
        Task.Run(async () => await new EasyApiClient(true).AcceptFriendRequest(friendRequest));
    }

    private void RejectFriend_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button {DataContext: FriendRequest friendRequest}) return;
        Task.Run(async () => await new EasyApiClient(true).RejectFriendRequest(friendRequest));
    }

    public ICollection<FriendRequest> FriendRequests
    {
        get => GetValue(FriendRequestsProperty);
        set => SetValue(FriendRequestsProperty, value);
    }

    public IBrush ExpanderColor
    {
        get => GetValue(ExpanderColorProperty);
        set => SetValue(ExpanderColorProperty, value);
    }

    public static readonly StyledProperty<ICollection<FriendRequest>> FriendRequestsProperty =
        AvaloniaProperty.Register<FriendRequestsPanel, ICollection<FriendRequest>>(nameof(FriendRequests),
            new List<FriendRequest>());

    public static readonly StyledProperty<IBrush> ExpanderColorProperty =
        AvaloniaProperty.Register<FriendRequestsPanel, IBrush>(nameof(ExpanderColor));
}