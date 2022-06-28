using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PChat.GUI.Controls;

public partial class MessageBox : UserControl
{
    public MessageBox()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}