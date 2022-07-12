using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace PChat.GUI.Controls;

public partial class DynamicExpander : UserControl
{
    private ContentPresenter _expandableContent;
    private Button _expanderButton;

    public enum EDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public DynamicExpander()
    {
        InitializeComponent();
        HeaderProperty.Changed
            .Subscribe(x => UpdateButtonContent(Direction));
    }

    private void UpdateButtonContent(EDirection direction)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_expanderButton is null) return;
        _expanderButton.Content = IsExpanded
            ? ParseButtonContent(direction, false)
            : ParseButtonContent(ParseOpposite(direction), true);
        _expanderButton.Background = ButtonColor;
    }

    private void BuildContent(EDirection direction)
    {
        var stackPanel = new StackPanel();
        _expanderButton = new Button
        {
            Content = IsExpanded
                ? ParseButtonContent(direction, false)
                : ParseButtonContent(ParseOpposite(direction), true),
            Background = ButtonColor
        };
        _expanderButton.Tapped += ExpanderButton_OnTapped;
        _expandableContent = new ContentPresenter
        {
            Content = ExpandableContent,
            IsVisible = IsExpanded
        };
        switch (direction)
        {
            case EDirection.Left:
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(_expandableContent);
                stackPanel.Children.Add(_expanderButton);
                break;
            case EDirection.Right:
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(_expanderButton);
                stackPanel.Children.Add(_expandableContent);
                break;
            case EDirection.Up:
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.Children.Add(_expandableContent);
                stackPanel.Children.Add(_expanderButton);
                break;
            case EDirection.Down:
                stackPanel.Orientation = Orientation.Vertical;
                stackPanel.Children.Add(_expanderButton);
                stackPanel.Children.Add(_expandableContent);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        Content = stackPanel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private object ParseButtonContent(EDirection direction, bool isCollapsed)
    {
        var stackedContent = new StackPanel();
        var symbol = new TextBlock
        {
            Text = ParseSymbol(direction),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        var headerTextBlock = new TextBlock
        {
            Text = Header,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        var fontSizeInPx = 0d;
        var contentWidth = 0d;
        switch (direction)
        {
            case EDirection.Left:
                stackedContent.Orientation = Orientation.Horizontal;
                headerTextBlock.RenderTransform = new RotateTransform(90);

                if (!(isCollapsed && HeaderOnlyWhenCollapsed))
                    stackedContent.Children.Add(symbol);
                stackedContent.Children.Add(headerTextBlock);

                fontSizeInPx = headerTextBlock.FontSize * 1.3;
                contentWidth = stackedContent.Children.Count * fontSizeInPx;
                headerTextBlock.Margin = new Thickness(-50, 0, -50, 0);
                stackedContent.Height = Header.Length * headerTextBlock.FontSize;
                stackedContent.Width = contentWidth;
                break;
            case EDirection.Right:
                stackedContent.Orientation = Orientation.Horizontal;
                headerTextBlock.RenderTransform = new RotateTransform(-90);
                stackedContent.Children.Add(headerTextBlock);
                if (!(isCollapsed && HeaderOnlyWhenCollapsed))
                    stackedContent.Children.Add(symbol);

                fontSizeInPx = headerTextBlock.FontSize * 1.3;
                contentWidth = stackedContent.Children.Count * fontSizeInPx;
                headerTextBlock.Margin = new Thickness(-50, 0, -50, 0);
                stackedContent.Height = Header.Length * headerTextBlock.FontSize;
                stackedContent.Width = contentWidth;
                break;
            case EDirection.Up:
                stackedContent.Orientation = Orientation.Vertical;
                if (!(isCollapsed && HeaderOnlyWhenCollapsed))
                    stackedContent.Children.Add(symbol);
                stackedContent.Children.Add(headerTextBlock);
                break;
            case EDirection.Down:
                stackedContent.Orientation = Orientation.Vertical;
                stackedContent.Children.Add(headerTextBlock);
                if (!(isCollapsed && HeaderOnlyWhenCollapsed))
                    stackedContent.Children.Add(symbol);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return stackedContent;
    }

    private string ParseSymbol(EDirection direction)
    {
        return direction switch
        {
            EDirection.Left => "←",
            EDirection.Right => "→",
            EDirection.Up => "↑",
            EDirection.Down => "↓",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private EDirection ParseOpposite(EDirection direction)
    {
        return direction switch
        {
            EDirection.Left => EDirection.Right,
            EDirection.Right => EDirection.Left,
            EDirection.Up => EDirection.Down,
            EDirection.Down => EDirection.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set
        {
            SetValue(IsExpandedProperty, value);
            _expandableContent.IsVisible = value;
        }
    }

    public ICollection<object> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public object ExpandableContent
    {
        get => GetValue(ExpandableContentProperty);
        set
        {
            SetValue(ExpandableContentProperty, value);
            BuildContent(Direction);
        }
    }

    public EDirection Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public IBrush ButtonColor
    {
        get => GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }

    public bool HeaderOnlyWhenCollapsed
    {
        get => GetValue(HeaderOnlyWhenCollapsedProperty);
        set => SetValue(HeaderOnlyWhenCollapsedProperty, value);
    }

    public static readonly StyledProperty<ICollection<object>> ItemsProperty =
        AvaloniaProperty.Register<DynamicExpander, ICollection<object>>(nameof(Items));

    public static readonly StyledProperty<object> ExpandableContentProperty =
        AvaloniaProperty.Register<DynamicExpander, object>(nameof(ExpandableContent));

    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<DynamicExpander, bool>(nameof(IsExpanded));

    public static readonly StyledProperty<EDirection> DirectionProperty =
        AvaloniaProperty.Register<DynamicExpander, EDirection>(nameof(Direction));

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<DynamicExpander, string>(nameof(Header), "");

    public static readonly StyledProperty<IBrush> ButtonColorProperty =
        AvaloniaProperty.Register<DynamicExpander, IBrush>(nameof(ButtonColor));

    public static readonly StyledProperty<bool> HeaderOnlyWhenCollapsedProperty =
        AvaloniaProperty.Register<DynamicExpander, bool>(nameof(HeaderOnlyWhenCollapsed));

    private void ExpanderButton_OnTapped(object? sender, RoutedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        var button = (Button) sender!;
        button.Content = IsExpanded
            ? ParseButtonContent(Direction, false)
            : ParseButtonContent(ParseOpposite(Direction), true);
    }
}