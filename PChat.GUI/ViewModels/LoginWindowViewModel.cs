using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Threading;
using Pchat;
using PChat.API.Client;
using PChat.GUI.Models;
using PChat.Notify;
using PChat.Shared;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

public class LoginWindowViewModel : ViewModelBase
{
    private ErrorText _errorText;

    public LoginWindowViewModel()
    {
    }

    public LoginWindowViewModel(CancellationToken cancellationToken)
    {
        LoginCommand = ReactiveCommand.Create(() =>
        {
            if (IdHexString == null || KeyHexString == null)
            {
                ErrorText = new ErrorText("Empty login!", Colors.Yellow);
                return;
            }

            var id = HexString.ToByteString(IdHexString);
            var key = HexString.ToByteString(KeyHexString);
            var apiClient = new EasyApiClient(true);
            var loggedIn = false;
            Task.Run(async () => loggedIn = await apiClient.Login(new Credentials {Id = id, Key = key}))
                .ContinueWith(o =>
                {
                    if (loggedIn)
                    {
                        ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                        var account = new Account {Id = id, Key = key};
                        Thread.Sleep(100);
                        OpenMainWindow(account, cancellationToken);
                    }

                    ErrorText = new ErrorText("Invalid login!", Colors.Red);
                });
        });

        CreateNewAccountCommand = ReactiveCommand.Create(() =>
        {
            ErrorText = new ErrorText("Generating new login..", Colors.LawnGreen);
            Task.Run(async () => await new EasyApiClient(true).CreateAccount())
                .ContinueWith(o =>
                {
                    if (!o.IsCompletedSuccessfully)
                    {
                        ErrorText = new ErrorText("Failed", Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    Thread.Sleep(100);

                    OpenMainWindow(o.Result, cancellationToken);
                });
        });
    }

    private void OpenMainWindow(Account account, CancellationToken cancellationToken)
    {
        SessionContent.Account = account;
        new NotifyServer().Start(Array.Empty<string>());
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(cancellationToken)
            };
            mainWindow.Show();
            MainWindowOpened = true;
        });
    }

    public ErrorText ErrorText
    {
        get => _errorText;
        set => this.RaiseAndSetIfChanged(ref _errorText, value);
    }

    public string IdHexString { get; set; }
    public string KeyHexString { get; set; }

    public ICommand LoginCommand { get; set; }
    public ICommand CreateNewAccountCommand { get; set; }

    public bool MainWindowOpened { get; private set; }
}