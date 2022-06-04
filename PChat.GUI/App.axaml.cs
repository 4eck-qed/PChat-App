using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace PChat.GUI
{
    public class App : Application
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoginWindow
                {
                    DataContext = new LoginWindowViewModel(_cancellationTokenSource.Token),
                };
                desktop.ShutdownRequested += (sender, args) => _cancellationTokenSource.Cancel();
                desktop.Exit += (sender, args) => _cancellationTokenSource.Cancel();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}