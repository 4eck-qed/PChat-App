using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PChat.GUI.Controls;

public partial class GroupList : UserControl
{
    public GroupList()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public ICollection<Group> Groups
    {
        get => GetValue(GroupsProperty);
        set => SetValue(GroupsProperty, value);
    }

    public Group SelectedGroup
    {
        get => GetValue(SelectedGroupProperty);
        set => SetValue(SelectedGroupProperty, value);
    }

    public static readonly StyledProperty<ICollection<Group>> GroupsProperty =
        AvaloniaProperty.Register<MainWindow, ICollection<Group>>(nameof(Groups));

    public static readonly StyledProperty<Group> SelectedGroupProperty =
        AvaloniaProperty.Register<MainWindow, Group>(nameof(SelectedGroup));
}