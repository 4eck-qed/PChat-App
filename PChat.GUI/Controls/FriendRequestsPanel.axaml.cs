using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Pchat;
using PChat.API.Client;

namespace PChat.GUI.Controls;

public partial class FriendRequestsPanel : UserControl
{
    public FriendRequestsPanel()
    {
        Header = "Friend Requests (0)";
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AcceptFriend_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button {DataContext: FriendRequest friendRequest}) return;
        Task.Run(async () => await EasyApiClient.Instance.AcceptFriendRequest(friendRequest));
    }

    private void RejectFriend_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button {DataContext: FriendRequest friendRequest}) return;
        Task.Run(async () => await EasyApiClient.Instance.RejectFriendRequest(friendRequest));
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

    public string Header
    {
        get => _header;
        set => SetAndRaise(HeaderProperty, ref _header, value);
    }

    public int FriendRequestsCount
    {
        get => _friendRequestsCount;
        set
        {
            _friendRequestsCount = value;
            Header = $"Friend Requests ({value})";
        }
    }

    public static readonly DirectProperty<FriendRequestsPanel, string> HeaderProperty =
        AvaloniaProperty.RegisterDirect<FriendRequestsPanel, string>(nameof(Header),
            o => o.Header,
            (o, v) => o.Header = v);

    public static readonly StyledProperty<ICollection<FriendRequest>> FriendRequestsProperty =
        AvaloniaProperty.Register<FriendRequestsPanel, ICollection<FriendRequest>>(nameof(FriendRequests),
            new ObservableCollection<FriendRequest>());

    public static readonly StyledProperty<IBrush> ExpanderColorProperty =
        AvaloniaProperty.Register<FriendRequestsPanel, IBrush>(nameof(ExpanderColor));

    private string _header;
    private int _friendRequestsCount;
}