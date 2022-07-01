using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pchat;

namespace PChat.GUI.Controls;

public partial class StatusBar : UserControl
{
    private string? _lastLogin;

    public StatusBar()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ConcurrentQueue<TextMessage> MessageQueue
    {
        get => GetValue(MessageQueueProperty);
        set => SetValue(MessageQueueProperty, value);
    }

    public ObservableCollection<TextMessage> Notifications
    {
        get => GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    public ObservableCollection<DateTime> LoginHistory
    {
        get => GetValue(LoginHistoryProperty);
        set
        {
            SetValue(LoginHistoryProperty, value);
            SetValue(LastLoginProperty, value.Count == 0 ? "--" : value.Last().ToString(CultureInfo.CurrentCulture));
        }
    }

    public string? LastLogin
    {
        get => LoginHistory.Count == 0 ? "--" : LoginHistory.Last().ToString(CultureInfo.CurrentCulture);
        set => SetValue(LastLoginProperty, value);
    }

    public static readonly StyledProperty<ConcurrentQueue<TextMessage>> MessageQueueProperty =
        AvaloniaProperty.Register<StatusBar, ConcurrentQueue<TextMessage>>(nameof(MessageQueue));

    public static readonly StyledProperty<ObservableCollection<TextMessage>> NotificationsProperty =
        AvaloniaProperty.Register<StatusBar, ObservableCollection<TextMessage>>(nameof(MessageQueue));

    public static readonly StyledProperty<ObservableCollection<DateTime>> LoginHistoryProperty =
        AvaloniaProperty.Register<StatusBar, ObservableCollection<DateTime>>(nameof(MessageQueue));

    private static readonly StyledProperty<string?> LastLoginProperty =
        AvaloniaProperty.Register<StatusBar, string?>(nameof(LastLogin));
}