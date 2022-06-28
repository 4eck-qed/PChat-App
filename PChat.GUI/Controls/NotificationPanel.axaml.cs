using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pchat;

namespace PChat.GUI.Controls;

public partial class NotificationPanel : UserControl
{
    public NotificationPanel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ObservableCollection<TextMessage> Notifications
    {
        get => GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    public TextMessage SelectedNotification
    {
        get => GetValue(SelectedNotificationProperty);
        set => SetValue(SelectedNotificationProperty, value);
    }

    public ICommand SelectCommand
    {
        get => GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<TextMessage>> NotificationsProperty =
        AvaloniaProperty.Register<NotificationPanel, ObservableCollection<TextMessage>>(nameof(Notifications));

    public static readonly StyledProperty<ICommand> SelectCommandProperty =
        AvaloniaProperty.Register<NotificationPanel, ICommand>(nameof(SelectCommand));
    
    public static readonly StyledProperty<TextMessage> SelectedNotificationProperty =
        AvaloniaProperty.Register<NotificationPanel, TextMessage>(nameof(SelectedNotification));
}