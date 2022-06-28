using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Pchat;

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
    }

    private void RejectFriend_OnTapped(object? sender, RoutedEventArgs e)
    {
    }

    public ICollection<FriendRequest> FriendRequests
    {
        get => GetValue(FriendRequestsProperty);
        set => SetValue(FriendRequestsProperty, value);
    }

    public static readonly StyledProperty<ICollection<FriendRequest>> FriendRequestsProperty =
        AvaloniaProperty.Register<FriendRequestsPanel, ICollection<FriendRequest>>(nameof(FriendRequests),
            new List<FriendRequest>());
}