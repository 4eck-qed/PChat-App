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
    public StatusBar()
    {
        InitializeComponent();
        LoginHistoryProperty.Changed
            .Subscribe(x =>
            {
                if (LoginHistory.Count < 2)
                {
                    LastLogin = "--";
                    return;
                }
                var secondLast = LoginHistory.Count - 2;
                LastLogin = LoginHistory[secondLast].ToLocalTime().ToString(CultureInfo.CurrentCulture);
            });
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
        set => SetValue(LoginHistoryProperty, value);
    }

    private string LastLogin
    {
        get => GetValue(LastLoginProperty);
        set => SetValue(LastLoginProperty, value);
    }

    public static readonly StyledProperty<ConcurrentQueue<TextMessage>> MessageQueueProperty =
        AvaloniaProperty.Register<StatusBar, ConcurrentQueue<TextMessage>>(nameof(MessageQueue));

    public static readonly StyledProperty<ObservableCollection<TextMessage>> NotificationsProperty =
        AvaloniaProperty.Register<StatusBar, ObservableCollection<TextMessage>>(nameof(Notifications));

    public static readonly StyledProperty<ObservableCollection<DateTime>> LoginHistoryProperty =
        AvaloniaProperty.Register<StatusBar, ObservableCollection<DateTime>>(nameof(LoginHistory));

    private static readonly StyledProperty<string> LastLoginProperty =
        AvaloniaProperty.Register<StatusBar, string>(nameof(LastLogin));
}