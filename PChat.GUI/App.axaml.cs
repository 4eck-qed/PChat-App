using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using JetBrains.Annotations;
using PChat.API.Client;
using PChat.Notify;

namespace PChat.GUI
{
    public class App : Application
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly NotifyServer _notifyServer = new();
        private LoginWindow _loginWindow;
        private IClassicDesktopStyleApplicationLifetime _desktop;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

            _desktop = desktop;
            _loginWindow = new LoginWindow {DataContext = new LoginWindowViewModel(_cancellationTokenSource.Token)};
            _desktop.MainWindow = _loginWindow;
            _desktop.ShutdownRequested += OnExitOrShutdown;
            _desktop.Exit += OnExitOrShutdown;

            EventBus.Instance.Subscribe(this);
            base.OnFrameworkInitializationCompleted();
        }

        private async void OnExitOrShutdown(object? sender, EventArgs args)
        {
            Console.WriteLine("[DEBUG] OnExitOrShutdown called");
            await EasyApiClient.Instance.Kill();
            _cancellationTokenSource.Cancel();
            await _notifyServer.ShutdownAsync();
        }

        [UsedImplicitly]
        public void OnEvent(OnLoginEvent e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _notifyServer.Start();
                _desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(_cancellationTokenSource.Token)
                };
                _desktop.MainWindow.Show();
                _loginWindow.Close();

                base.OnFrameworkInitializationCompleted();
            });
        }
    }
}