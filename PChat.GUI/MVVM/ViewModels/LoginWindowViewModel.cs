using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Threading;
using PChat.API.Client;
using PChat.GUI.MVVM.Models;
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
            if (id == null || key == null)
            {
                ErrorText = new ErrorText("Invalid login!", Colors.Red);
                return;
            }
            
            ErrorText = new ErrorText("Success!", Colors.LawnGreen);
            Thread.Sleep(100);

            var login = new Login {Id = id, Key = key};
            
            OpenMainWindow(login, cancellationToken);
        });

        CreateNewAccountCommand = ReactiveCommand.Create(() =>
        {
            ErrorText = new ErrorText("Generating new login..", Colors.LawnGreen);
            Task.Run(async () => await new ApiClient(true).GetCredentials())
                .ContinueWith(loginResult =>
                {
                    if (!loginResult.IsCompletedSuccessfully)
                    {
                        ErrorText = new ErrorText("Failed", Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    Thread.Sleep(100);

                    OpenMainWindow(loginResult.Result, cancellationToken);
                });
        });
    }

    private void OpenMainWindow(Login login, CancellationToken cancellationToken)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(login, cancellationToken)
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