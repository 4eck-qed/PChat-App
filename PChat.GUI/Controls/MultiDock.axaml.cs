using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;

namespace PChat.GUI.Controls;

public partial class MultiDock : UserControl
{
    public MultiDock()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public Avalonia.Controls.Controls Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<Avalonia.Controls.Controls> ItemsProperty =
        AvaloniaProperty.Register<MultiDock, Avalonia.Controls.Controls>(nameof(Items));

    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<MultiDock, Orientation>(nameof(OrientationProperty));
}