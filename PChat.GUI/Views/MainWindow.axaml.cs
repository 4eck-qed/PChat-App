using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

// ReSharper disable once CheckNamespace
namespace PChat.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Console.WriteLine("Initializing main window..");
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}