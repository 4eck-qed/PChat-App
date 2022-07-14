using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Google.Protobuf;
using ReactiveUI;
using Pchat;
using PChat.API.Client;
using PChat.Config;
using PChat.Extensions;
using PChat.GUI.Models;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

public class LoginWindowViewModel : ViewModelBase
{
    private readonly CancellationToken _cancellationToken;
    private ErrorText _errorText;
    private bool _saveLoginChecked;
    private string _saveLoginContent;
    private const string SaveLoginFile = $"{Global.DataDir}/saved";

    private enum Errors
    {
        Offline = 0,
        InvalidLogin = 1,
        IdFieldEmpty = 2,
        KeyFieldEmpty = 3,
    }

    private readonly Dictionary<Errors, string> _errorDict = new()
    {
        {Errors.Offline, "API or Lookup Server is Offline!"},
        {Errors.InvalidLogin, "Invalid Login!"},
        {Errors.IdFieldEmpty, "Id Field is Empty!"},
        {Errors.KeyFieldEmpty, "Key Field is Empty!"},
    };

    public LoginWindowViewModel(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        CreateCommands();
        ReadSavedLogin();
    }

    private void CreateCommands()
    {
        LoginCommand = ReactiveCommand.Create(() =>
        {
            if (string.IsNullOrEmpty(HexId))
            {
                ErrorText = new ErrorText(_errorDict[Errors.IdFieldEmpty], Colors.Yellow);
                return;
            }

            if (string.IsNullOrEmpty(HexKey))
            {
                ErrorText = new ErrorText(_errorDict[Errors.KeyFieldEmpty], Colors.Yellow);
                return;
            }

            var loggedIn = false;
            Task.Run(async () => loggedIn = await EasyApiClient.Instance.Login(new Credentials
                {
                    Id = HexString.ToByteString(HexId),
                    Key = HexString.ToByteString(HexKey)
                }) is not null, _cancellationToken)
                .ContinueWith(_ =>
                {
                    if (!loggedIn)
                    {
                        ErrorText = new ErrorText(_errorDict[Errors.InvalidLogin], Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    if (SaveLoginChecked)
                        SaveLogin(HexString.ToByteString(HexId), HexString.ToByteString(HexKey));
                    Thread.Sleep(100);
                    EventBus.Instance.PostEvent(new OnLoginEvent());
                }, _cancellationToken);
        });

        CreateNewAccountCommand = ReactiveCommand.Create(() =>
        {
            ErrorText = new ErrorText("Generating new login..", Colors.LawnGreen);
            var account = new Account();
            Task.Run(async () => account = await EasyApiClient.Instance.CreateAccount(), _cancellationToken)
                .ContinueWith(o =>
                {
                    if (!o.IsCompletedSuccessfully || account is null)
                    {
                        ErrorText = new ErrorText(_errorDict[Errors.Offline], Colors.Red);
                        return;
                    }

                    ErrorText = new ErrorText("Success!", Colors.LawnGreen);
                    if (SaveLoginChecked)
                        SaveLogin(account.Id, account.Key);
                    Thread.Sleep(100);
                    EventBus.Instance.PostEvent(new OnLoginEvent());
                }, _cancellationToken);
        });
    }

    private static void SaveLogin(ByteString id, ByteString key)
    {
        var hexId = id.ToHexString();
        var heyKey = key.ToHexString();
        File.WriteAllLines(SaveLoginFile, new[] {hexId, heyKey});
    }

    private void ReadSavedLogin()
    {
        if (!File.Exists(SaveLoginFile))
        {
            SaveLoginChecked = false;
            return;
        }

        var lines = File.ReadAllLines(SaveLoginFile);
        if (lines.Length < 2)
        {
            SaveLoginChecked = false;
            return;
        }

        SaveLoginChecked = true;
        HexId = lines[0];
        HexKey = lines[1];
    }

    private static void WipeSavedLogin() => File.Delete(SaveLoginFile);

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