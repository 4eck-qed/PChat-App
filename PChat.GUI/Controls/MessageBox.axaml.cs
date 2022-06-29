using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace PChat.GUI.Controls;

public partial class MessageBox : UserControl
{
    public MessageBox()
    {
        CommandWrapper = ReactiveCommand.Create<object>(param =>
        {
            Command.Execute(param);
            Input = string.Empty;
        });
        
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public string Input
    {
        get => GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public ICommand CommandWrapper { get; set; }

    public static readonly StyledProperty<string> InputProperty =
        AvaloniaProperty.Register<MessageBox, string>(nameof(Input));

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<MessageBox, ICommand>(nameof(Command));

    private void EmoteButton_OnTapped(object? sender, RoutedEventArgs e)
    {
        // TODO Display a selection of emotes
        var emote = new byte();
        throw new System.NotImplementedException();
    }

    private void MicButton_OnTapped(object? sender, RoutedEventArgs e)
    {
        // TODO Display a wave diagram for input and record it
        // Probably utilize the bound command and call it with this as parameter
        var memo = new byte();
        Command.Execute(memo);
        throw new System.NotImplementedException();
    }

    private void AttachButton_OnTapped(object? sender, RoutedEventArgs e)
    {
        // TODO Display a file manager
        var file = new byte();
        Command.Execute(file);
        throw new System.NotImplementedException();
    }
}