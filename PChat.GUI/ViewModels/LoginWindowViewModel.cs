using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Google.Protobuf;
using Pchat;
using PChat.API.Client;
using PChat.Extensions;
using PChat.GUI.Models;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

public class LoginWindowViewModel : ViewModelBase
{
    private ErrorText _errorText;
    private bool _saveLoginChecked;
    private string _saveLoginContent;

    public LoginWindowViewModel()
    {
    }

    public LoginWindowViewModel(CancellationToken cancellationToken)
    {
        CreateCommands();
        ReadSavedLogin();
    }

    private void CreateCommands()
    {
        LoginCommand = ReactiveCommand.Create(() =>
        {
            if (string.IsNullOrEmpty(HexId))
            {
                ErrorText = new ErrorText("Id missing!", Colors.Yellow);
                return;
            }

            if (string.IsNullOrEmpty(HexKey))
            {
                ErrorText = new ErrorText("Key missing!", Colors.Yellow);
                return;
            }

            var loggedIn = false;
            Task.Run(async () => loggedIn = await EasyApiClient.Instance.Login(new Credentials
                {
                    Id = HexString.ToByteString(HexId),
                    Key = HexString.ToByteString(HexKey)
                }) is not null)
                .ContinueWith(o =>
                {
                    if (!loggedIn)
                    {
                        ErrorText = new ErrorText("Invalid login!", Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    if (SaveLoginChecked)
                        SaveLogin(HexString.ToByteString(HexId), HexString.ToByteString(HexKey));
                    Thread.Sleep(100);
                    EventBus.Instance.PostEvent(new OnLoginEvent());
                });
        });

        CreateNewAccountCommand = ReactiveCommand.Create(() =>
        {
            ErrorText = new ErrorText("Generating new login..", Colors.LawnGreen);
            var account = new Account();
            Task.Run(async () => account = await EasyApiClient.Instance.CreateAccount())
                .ContinueWith(o =>
                {
                    if (!o.IsCompletedSuccessfully)
                    {
                        ErrorText = new ErrorText("Failed", Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    if (SaveLoginChecked)
                        SaveLogin(account.Id, account.Key);
                    Thread.Sleep(100);
                    EventBus.Instance.PostEvent(new OnLoginEvent());
                });
        });
    }

    private static void SaveLogin(ByteString id, ByteString key)
    {
        var file = "./data/savedlogin";
        File.WriteAllLines(file, new[] {"yes", id.ToHexString(), key.ToHexString()});
    }

    private void ReadSavedLogin()
    {
        var file = "./data/savedlogin";
        if (!File.Exists(file))
        {
            SaveLoginChecked = false;
            return;
        }

        var lines = File.ReadAllLines(file);
        if (lines.First() != "yes" || lines.Length < 3)
        {
            SaveLoginChecked = false;
            return;
        }

        SaveLoginChecked = true;
        HexId = lines[1];
        HexKey = lines[2];
    }

    private static void WipeSavedLogin()
    {
        var file = "./data/savedlogin";
        File.WriteAllLines(file, new[] {"no"});
    }

    public ErrorText ErrorText
    {
        get => _errorText;
        private set => this.RaiseAndSetIfChanged(ref _errorText, value);
    }

    public string HexId { get; set; }
    public string HexKey { get; set; }

    public ICommand LoginCommand { get; set; }
    public ICommand CreateNewAccountCommand { get; set; }

    public bool SaveLoginChecked
    {
        get => _saveLoginChecked;
        set
        {
            if (value == false)
            {
                SaveLoginContent = "no";
                WipeSavedLogin();
            }
            else
            {
                SaveLoginContent = "yes";
            }

            this.RaiseAndSetIfChanged(ref _saveLoginChecked, value);
        }
    }

    public string SaveLoginContent
    {
        get => _saveLoginContent;
        set => this.RaiseAndSetIfChanged(ref _saveLoginContent, value);
    }
}