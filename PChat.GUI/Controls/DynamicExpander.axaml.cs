using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PChat.GUI.Controls;

public partial class DynamicExpander : UserControl
{
    public DynamicExpander()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public bool IsCollapsed { get; set; } // TODO returns true if expander is collapsed
    
    public ICollection<object> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly StyledProperty<ICollection<object>> ItemsProperty =
        AvaloniaProperty.Register<MainWindow, ICollection<object>>(nameof(Items));
}