using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using PChat.API.Client;
using ReactiveUI;

namespace PChat.GUI.Controls;

public partial class ProfileBox : UserControl
{
    public ProfileBox()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        ChangeNameCommand = ReactiveCommand.Create(async () => await new EasyApiClient(true).UpdateAccount());
        ChangeStatusCommand = ReactiveCommand.Create(async () => await new EasyApiClient(true).UpdateAccount());
    }


    public ICommand ChangeNameCommand
    {
        get => GetValue(ChangeNameCommandProperty);
        set => SetValue(ChangeNameCommandProperty, value);
    }
        
    public static readonly StyledProperty<ICommand> ChangeNameCommandProperty =
        AvaloniaProperty.Register<ProfileBox, ICommand>(nameof(ChangeNameCommand));

    public ICommand ChangeStatusCommand
    {
        get => GetValue(ChangeStatusCommandProperty);
        set => SetValue(ChangeStatusCommandProperty, value);
    }
    
    public static readonly StyledProperty<ICommand> ChangeStatusCommandProperty =
        AvaloniaProperty.Register<ProfileBox, ICommand>(nameof(ChangeStatusCommand));

    public ProfileViewModel Profile
    {
        get => GetValue(ProfileProperty);
        set => SetValue(ProfileProperty, value);
    }

    public static readonly StyledProperty<ProfileViewModel> ProfileProperty =
        AvaloniaProperty.Register<ProfileBox, ProfileViewModel>(nameof(Profile));

    private ICommand _changeNameCommand;
    private ICommand _changeStatusCommand;

    #region Private Methods

    private static void CopyFromTextBlock(object? sender, RoutedEventArgs e)
    {
        if (sender == null) return;
        var textBlock = (TextBlock) sender;
        CopyToClipboard(textBlock.Text);
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

    #endregion

    #region Event Handlers

    private void IdTextBlock_OnDoubleTapped(object? sender, RoutedEventArgs e)
    {
        this.FindControl<TextBlock>("IdCopyIndicator").Text = "📋";
        var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
        timer.Tick += (_, _) =>
        {
            this.FindControl<TextBlock>("IdCopyIndicator").Text = string.Empty;
            timer.Stop();
        };
        CopyFromTextBlock(sender, e);
    }

    private void KeyTextBlock_OnDoubleTapped(object? sender, RoutedEventArgs e)
    {
        this.FindControl<TextBlock>("KeyCopyIndicator").Text = "📋";
        var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
        timer.Tick += (_, _) =>
        {
            this.FindControl<TextBlock>("KeyCopyIndicator").Text = string.Empty;
            timer.Stop();
        };
        CopyFromTextBlock(sender, e);
    }

    #endregion
}