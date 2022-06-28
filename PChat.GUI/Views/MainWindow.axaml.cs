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

        private void CopyToClipboard(string text)
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

        private void IdTextBlock_OnDoubleTapped(object? sender, RoutedEventArgs e)
        {
            this.FindControl<TextBlock>("IdCopied").Text = "copied";
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
            timer.Tick += (_, _) =>
            {
                this.FindControl<TextBlock>("IdCopied").Text = string.Empty;
                timer.Stop();
            };
            CopyFromTextBlock(sender, e);
        }

        private void KeyTextBlock_OnDoubleTapped(object? sender, RoutedEventArgs e)
        {
            this.FindControl<TextBlock>("KeyCopied").Text = "copied";
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2), IsEnabled = true};
            timer.Tick += (_, _) =>
            {
                this.FindControl<TextBlock>("KeyCopied").Text = string.Empty;
                timer.Stop();
            };
            CopyFromTextBlock(sender, e);
        }

        private void CopyFromTextBlock(object? sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var textBlock = (TextBlock) sender;
            CopyToClipboard(textBlock.Text);
        }
    }
}