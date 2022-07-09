using System.Windows.Input;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace PChat.GUI.Controls;

public partial class EditableTextBlock : UserControl
{
    private TextBlock _textBlock;
    private TextBox _textBox;
    private bool _isEditable;

    public EditableTextBlock()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _textBlock = new TextBlock();
        _textBlock.DoubleTapped += (sender, args) => IsEditable = true;
        _textBox = new TextBox();
        _textBox.KeyDown += (sender, args) =>
        {
            if (args.Key != Key.Enter) return;
            SetValue(TextProperty, _textBox.Text);
            IsEditable = false;
            TextChangedCommand?.Execute(null);
        };
        Content = _textBlock;
    }

    public bool IsEditable
    {
        get => _isEditable;
        set
        {
            _isEditable = value;
            if (value)
                Content = _textBox;
            else
                Content = _textBlock;
        }
    }

    public static readonly DirectProperty<EditableTextBlock, bool> IsEditableProperty = AvaloniaProperty
        .RegisterDirect<EditableTextBlock, bool>(nameof(IsEditable), o => o.IsEditable, (o, v) => o.IsEditable = v);
    
    public ICommand TextChangedCommand { get; set; }

    public static readonly DirectProperty<EditableTextBlock, ICommand> TextChangedCommandProperty = AvaloniaProperty
        .RegisterDirect<EditableTextBlock, ICommand>(nameof(TextChangedCommand), o => o.TextChangedCommand, (o, v) => o.TextChangedCommand = v);

    #region Overwritten Properties

    public new static DirectProperty<EditableTextBlock, HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, HorizontalAlignment>(nameof(HorizontalContentAlignment),
            o => o.HorizontalContentAlignment,
            (o, v) =>
            {
                o.HorizontalContentAlignment = v;
                o._textBox.HorizontalContentAlignment = v;
            });

    public new static DirectProperty<EditableTextBlock, VerticalAlignment> VerticalContentAlignmentProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, VerticalAlignment>(nameof(VerticalContentAlignment),
            o => o.VerticalContentAlignment,
            (o, v) =>
            {
                o.VerticalContentAlignment = v;
                o._textBox.VerticalContentAlignment = v;
            });

    public new static DirectProperty<EditableTextBlock, IBrush?> BackgroundProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IBrush?>(nameof(Background),
            o => o.Background,
            (o, v) =>
            {
                o.Background = v;
                o._textBlock.Background = v;
                o._textBox.Background = v;
            });

    public new static DirectProperty<EditableTextBlock, IBrush?> BorderBrushProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IBrush?>(nameof(BorderBrush),
            o => o.BorderBrush,
            (o, v) =>
            {
                o.BorderBrush = v;
                o._textBox.BorderBrush = v;
            });

    public new static DirectProperty<EditableTextBlock, Thickness> BorderThicknessProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Thickness>(nameof(BorderThickness),
            o => o.BorderThickness,
            (o, v) =>
            {
                o.BorderThickness = v;
                o._textBox.BorderThickness = v;
            });

    public new static DirectProperty<EditableTextBlock, CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, CornerRadius>(nameof(CornerRadius),
            o => o.CornerRadius,
            (o, v) =>
            {
                o.CornerRadius = v;
                o._textBox.CornerRadius = v;
            });

    public new static DirectProperty<EditableTextBlock, FontFamily> FontFamilyProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, FontFamily>(nameof(FontFamily),
            o => o.FontFamily,
            (o, v) =>
            {
                o.FontFamily = v;
                o._textBlock.FontFamily = v;
                o._textBox.FontFamily = v;
            });

    public new static DirectProperty<EditableTextBlock, double> FontSizeProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(FontSize),
            o => o.FontSize,
            (o, v) =>
            {
                o.FontSize = v;
                o._textBlock.FontSize = v;
                o._textBox.FontSize = v;
            });

    public new static DirectProperty<EditableTextBlock, FontStyle> FontStyleProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, FontStyle>(nameof(FontStyle),
            o => o.FontStyle,
            (o, v) =>
            {
                o.FontStyle = v;
                o._textBlock.FontStyle = v;
                o._textBox.FontStyle = v;
            });

    public new static DirectProperty<EditableTextBlock, FontWeight> FontWeightProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, FontWeight>(nameof(FontWeight),
            o => o.FontWeight,
            (o, v) =>
            {
                o.FontWeight = v;
                o._textBlock.FontWeight = v;
                o._textBox.FontWeight = v;
            });

    public new static DirectProperty<EditableTextBlock, IBrush?> ForegroundProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IBrush?>(nameof(Foreground),
            o => o.Foreground,
            (o, v) =>
            {
                o.Foreground = v;
                o._textBlock.Foreground = v;
                o._textBox.Foreground = v;
            });

    public new static DirectProperty<EditableTextBlock, Thickness> PaddingProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Thickness>(nameof(Padding),
            o => o.Padding,
            (o, v) =>
            {
                o.Padding = v;
                o._textBlock.Padding = v;
                o._textBox.Padding = v;
            });

    public new static DirectProperty<EditableTextBlock, IControlTemplate?> TemplateProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IControlTemplate?>(nameof(Template),
            o => o.Template,
            (o, v) =>
            {
                o.Template = v;
                o._textBox.Template = v;
            });

    public new static DirectProperty<EditableTextBlock, ITemplate<IControl>?> FocusAdornerProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, ITemplate<IControl>?>(nameof(FocusAdorner),
            o => o.FocusAdorner,
            (o, v) =>
            {
                o.FocusAdorner = v;
                o._textBlock.FocusAdorner = v;
                o._textBox.FocusAdorner = v;
            });

    public new static DirectProperty<EditableTextBlock, object?> TagProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, object?>(nameof(Tag),
            o => o.Tag,
            (o, v) =>
            {
                o.Tag = v;
                o._textBlock.Tag = v;
                o._textBox.Tag = v;
            });

    public new static DirectProperty<EditableTextBlock, ContextMenu?> ContextMenuProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, ContextMenu?>(nameof(ContextMenu),
            o => o.ContextMenu,
            (o, v) =>
            {
                o.ContextMenu = v;
                o._textBlock.ContextMenu = v;
                o._textBox.ContextMenu = v;
            });

    public new static DirectProperty<EditableTextBlock, FlyoutBase?> ContextFlyoutProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, FlyoutBase?>(nameof(ContextFlyout),
            o => o.ContextFlyout,
            (o, v) =>
            {
                o.ContextFlyout = v;
                o._textBlock.ContextFlyout = v;
                o._textBox.ContextFlyout = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> FocusableProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(Focusable),
            o => o.Focusable,
            (o, v) =>
            {
                o.Focusable = v;
                o._textBlock.Focusable = v;
                o._textBox.Focusable = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> IsEnabledProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(IsEnabled),
            o => o.IsEnabled,
            (o, v) =>
            {
                o.IsEnabled = v;
                o._textBlock.IsEnabled = v;
                o._textBox.IsEnabled = v;
            });

    public new static DirectProperty<EditableTextBlock, Cursor?> CursorProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Cursor?>(nameof(Cursor),
            o => o.Cursor,
            (o, v) =>
            {
                o.Cursor = v;
                o._textBlock.Cursor = v;
                o._textBox.Cursor = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> IsHitTestVisibleProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(IsHitTestVisible),
            o => o.IsHitTestVisible,
            (o, v) =>
            {
                o.IsHitTestVisible = v;
                o._textBlock.IsHitTestVisible = v;
                o._textBox.IsHitTestVisible = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> IsTabStopProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(IsTabStop),
            o => o.IsTabStop,
            (o, v) =>
            {
                o.IsTabStop = v;
                o._textBlock.IsTabStop = v;
                o._textBox.IsTabStop = v;
            });

    public new static DirectProperty<EditableTextBlock, int> TabIndexProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, int>(nameof(TabIndex),
            o => o.TabIndex,
            (o, v) =>
            {
                o.TabIndex = v;
                o._textBlock.TabIndex = v;
                o._textBox.TabIndex = v;
            });

    public new static DirectProperty<EditableTextBlock, double> WidthProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(Width),
            o => o.Width,
            (o, v) =>
            {
                o.Width = v;
                o._textBlock.Width = v;
                o._textBox.Width = v;
            });

    public new static DirectProperty<EditableTextBlock, double> HeightProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(Height),
            o => o.Height,
            (o, v) =>
            {
                o.Height = v;
                o._textBlock.Height = v;
                o._textBox.Height = v;
            });

    public new static DirectProperty<EditableTextBlock, double> MinWidthProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(MinWidth),
            o => o.MinWidth,
            (o, v) =>
            {
                o.MinWidth = v;
                o._textBlock.MinWidth = v;
                o._textBox.MinWidth = v;
            });

    public new static DirectProperty<EditableTextBlock, double> MaxWidthProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(MaxWidth),
            o => o.MaxWidth,
            (o, v) =>
            {
                o.MaxWidth = v;
                o._textBlock.MaxWidth = v;
                o._textBox.MaxWidth = v;
            });

    public new static DirectProperty<EditableTextBlock, double> MinHeightProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(MinHeight),
            o => o.MinHeight,
            (o, v) =>
            {
                o.MinHeight = v;
                o._textBlock.MinHeight = v;
                o._textBox.MinHeight = v;
            });

    public new static DirectProperty<EditableTextBlock, double> MaxHeightProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(MaxHeight),
            o => o.MaxHeight,
            (o, v) =>
            {
                o.MaxHeight = v;
                o._textBlock.MaxHeight = v;
                o._textBox.MaxHeight = v;
            });

    public new static DirectProperty<EditableTextBlock, Thickness> MarginProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Thickness>(nameof(Margin),
            o => o.Margin,
            (o, v) =>
            {
                o.Margin = v;
                o._textBlock.Margin = v;
                o._textBox.Margin = v;
            });

    public new static DirectProperty<EditableTextBlock, HorizontalAlignment> HorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, HorizontalAlignment>(nameof(HorizontalAlignment),
            o => o.HorizontalAlignment,
            (o, v) =>
            {
                o.HorizontalAlignment = v;
                o._textBlock.HorizontalAlignment = v;
                o._textBox.HorizontalAlignment = v;
            });

    public new static DirectProperty<EditableTextBlock, VerticalAlignment> VerticalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, VerticalAlignment>(nameof(VerticalAlignment),
            o => o.VerticalAlignment,
            (o, v) =>
            {
                o.VerticalAlignment = v;
                o._textBlock.VerticalAlignment = v;
                o._textBox.VerticalAlignment = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> UseLayoutRoundingProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(UseLayoutRounding),
            o => o.UseLayoutRounding,
            (o, v) =>
            {
                o.UseLayoutRounding = v;
                o._textBlock.UseLayoutRounding = v;
                o._textBox.UseLayoutRounding = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> ClipToBoundsProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(ClipToBounds),
            o => o.ClipToBounds,
            (o, v) =>
            {
                o.ClipToBounds = v;
                o._textBlock.ClipToBounds = v;
                o._textBox.ClipToBounds = v;
            });

    public new static DirectProperty<EditableTextBlock, Geometry?> ClipProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Geometry?>(nameof(Clip),
            o => o.Clip,
            (o, v) =>
            {
                o.Clip = v;
                o._textBlock.Clip = v;
                o._textBox.Clip = v;
            });

    public new static DirectProperty<EditableTextBlock, bool> IsVisibleProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(IsVisible),
            o => o.IsVisible,
            (o, v) =>
            {
                o.IsVisible = v;
                o._textBlock.IsVisible = v;
                o._textBox.IsVisible = v;
            });

    public new static DirectProperty<EditableTextBlock, double> OpacityProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(Opacity),
            o => o.Opacity,
            (o, v) =>
            {
                o.Opacity = v;
                o._textBlock.Opacity = v;
                o._textBox.Opacity = v;
            });

    public new static DirectProperty<EditableTextBlock, IBrush?> OpacityMaskProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IBrush?>(nameof(OpacityMask),
            o => o.OpacityMask,
            (o, v) =>
            {
                o.OpacityMask = v;
                o._textBlock.OpacityMask = v;
                o._textBox.OpacityMask = v;
            });

    public new static DirectProperty<EditableTextBlock, ITransform?> RenderTransformProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, ITransform?>(nameof(RenderTransform),
            o => o.RenderTransform,
            (o, v) =>
            {
                o.RenderTransform = v;
                o._textBlock.RenderTransform = v;
                o._textBox.RenderTransform = v;
            });

    public new static DirectProperty<EditableTextBlock, RelativePoint> RenderTransformOriginProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, RelativePoint>(nameof(RenderTransformOrigin),
            o => o.RenderTransformOrigin,
            (o, v) =>
            {
                o.RenderTransformOrigin = v;
                o._textBlock.RenderTransformOrigin = v;
                o._textBox.RenderTransformOrigin = v;
            });

    public new static DirectProperty<EditableTextBlock, int> ZIndexProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, int>(nameof(ZIndex),
            o => o.ZIndex,
            (o, v) =>
            {
                o.ZIndex = v;
                o._textBlock.ZIndex = v;
                o._textBox.ZIndex = v;
            });

    public new static DirectProperty<EditableTextBlock, object?> DataContextProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, object?>(nameof(DataContext),
            o => o.DataContext,
            (o, v) =>
            {
                o.DataContext = v;
                o._textBlock.DataContext = v;
                o._textBox.DataContext = v;
            });

    public new static DirectProperty<EditableTextBlock, string?> NameProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, string?>(nameof(Name),
            o => o.Name,
            (o, v) =>
            {
                o.Name = v;
                o._textBlock.Name = v;
                o._textBox.Name = v;
            });

    public new static DirectProperty<EditableTextBlock, IClock> ClockProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IClock>(nameof(Clock),
            o => o.Clock,
            (o, v) =>
            {
                o.Clock = v;
                o._textBlock.Clock = v;
                o._textBox.Clock = v;
            });

    public new static DirectProperty<EditableTextBlock, Transitions?> TransitionsProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, Transitions?>(nameof(Transitions),
            o => o.Transitions,
            (o, v) =>
            {
                o.Transitions = v;
                o._textBlock.Transitions = v;
                o._textBox.Transitions = v;
            });

    #endregion

    #region Shared Properties

    public TextAlignment TextAlignment { get; set; }

    public static readonly DirectProperty<EditableTextBlock, TextAlignment> TextAlignmentProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, TextAlignment>(nameof(TextAlignment),
            o => o.TextAlignment,
            (o, v) =>
            {
                o.TextAlignment = v;
                o._textBlock.TextAlignment = v;
                o._textBox.TextAlignment = v;
            });


    public TextWrapping TextWrapping { get; set; }

    public static readonly DirectProperty<EditableTextBlock, TextWrapping> TextWrappingProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, TextWrapping>(nameof(TextWrapping),
            o => o.TextWrapping,
            (o, v) =>
            {
                o.TextWrapping = v;
                o._textBlock.TextWrapping = v;
                o._textBox.TextWrapping = v;
            });


    public string Text { get; set; }

    public static readonly DirectProperty<EditableTextBlock, string> TextProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, string>(nameof(Text),
            o => o.Text,
            (o, v) =>
            {
                o.Text = v;
                o._textBlock.Text = v;
                o._textBox.Text = v;
            });

    #endregion

    #region TextBlock Properties

    public TextDecorationCollection TextDecorations { get; set; }

    public static readonly DirectProperty<EditableTextBlock, TextDecorationCollection> TextDecorationsProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, TextDecorationCollection>(nameof(TextDecorations),
            o => o.TextDecorations,
            (o, v) =>
            {
                o.TextDecorations = v;
                o._textBlock.TextDecorations = v;
            });

    public TextTrimming TextTrimming { get; set; }

    public static readonly DirectProperty<EditableTextBlock, TextTrimming> TextTrimmingProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, TextTrimming>(nameof(TextTrimming),
            o => o.TextTrimming,
            (o, v) =>
            {
                o.TextTrimming = v;
                o._textBlock.TextTrimming = v;
            });

    public int MaxLines { get; set; }

    public static readonly DirectProperty<EditableTextBlock, int> MaxLinesProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, int>(nameof(MaxLines),
            o => o.MaxLines,
            (o, v) =>
            {
                o.MaxLines = v;
                o._textBlock.MaxLines = v;
            });

    public double LineHeight { get; set; }

    public static readonly DirectProperty<EditableTextBlock, double> LineHeightProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, double>(nameof(LineHeight),
            o => o.LineHeight,
            (o, v) =>
            {
                o.LineHeight = v;
                o._textBlock.LineHeight = v;
            });

    #endregion

    #region TextBox Properties

    public bool AcceptsReturn { get; set; }

    public static readonly DirectProperty<EditableTextBlock, bool> AcceptsReturnProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(AcceptsReturn),
            o => o.AcceptsReturn,
            (o, v) =>
            {
                o.AcceptsReturn = v;
                o._textBox.AcceptsReturn = v;
            });

    public bool AcceptsTab { get; set; }

    public static readonly DirectProperty<EditableTextBlock, bool> AcceptsTabProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, bool>(nameof(AcceptsTab),
            o => o.AcceptsTab,
            (o, v) =>
            {
                o.AcceptsTab = v;
                o._textBox.AcceptsTab = v;
            });

    public IBrush CaretBrush { get; set; }

    public static readonly DirectProperty<EditableTextBlock, IBrush> CaretBrushProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, IBrush>(nameof(CaretBrush),
            o => o.CaretBrush,
            (o, v) =>
            {
                o.CaretBrush = v;
                o._textBox.CaretBrush = v;
            });

    public int CaretIndex { get; set; }

    public static readonly DirectProperty<EditableTextBlock, int> CaretIndexProperty =
        AvaloniaProperty.RegisterDirect<EditableTextBlock, int>(nameof(CaretIndex),
            o => o.CaretIndex,
            (o, v) =>
            {
                o.CaretIndex = v;
                o._textBox.CaretIndex = v;
            });

    #endregion
}