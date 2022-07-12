using System;
using System.Linq;
using System.Globalization;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using JetBrains.Annotations;
using Pchat;
using PChat.API.Client;
using PChat.GUI.Core;
using PChat.Extensions;
using PChat.Shared;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

/// <summary>
/// Contains everything specific to a chat.
/// </summary>
public class ChatViewModel : ViewModelBase
{
    #region Fields

    // private ObservableCollection<Message> _messages;
    private TextMessage _selectedMessage;
    private ISolidColorBrush _isOnlineIndicator = Brushes.Red;
    private Conversation _conversation;

    #endregion

    public ChatViewModel(ContactCard contact)
    {
        Contact = contact;
        InitializeCommands();

        EventBus.Instance.Subscribe(this);
        Task.Run(async () =>
        {
            await UpdateOnlineStatus();
            Conversation = await EasyApiClient.Instance.LoadConversation(contact);
        });
    }

    [UsedImplicitly]
    public void OnEvent(OnObjectChangedEvent e)
    {
        if (e.ObjectName == nameof(Session.Conversations))
        {
            Conversation = Session.Conversations.FirstOrDefault(x => x.Contact.Id == Contact.Id)!;
        }

        this.RaisePropertyChanged(e.ObjectName);
    }

    #region Private Methods

    private void InitializeCommands()
    {
        SendCommand = ReactiveCommand.Create<object>(input =>
        {
            switch (input)
            {
                case string text:
                    if (string.IsNullOrEmpty(text)) return;
                    var message = new TextMessage
                    {
                        Id = ByteStringExtensions.RandomByteString(16),
                        SenderId = Session.Account.Id,
                        ReceiverId = Contact.Id,
                        Content = text,
                        Time = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    };

                    Task.Run(async () => await EasyApiClient.Instance.SendMessage(message));
                    break;

                default:
                    Console.WriteLine("Only text messages are currently supported!");
                    break;
            }
        });

        RecordCommand = ReactiveCommand.Create(() =>
        {
            CoreLogic.OpenUrl("https://www.youtube.com/watch?v=AL38L9iesJw");
        });

        VideoCallCommand = ReactiveCommand.Create(() =>
        {
            CoreLogic.OpenUrl("https://www.youtube.com/watch?v=T3rXdeOvhNE");
        });

        VoiceCallCommand = ReactiveCommand.Create(() =>
        {
            CoreLogic.OpenUrl("https://www.youtube.com/watch?v=eXz_HCnzBAI");
        });

        EmoteCommand = ReactiveCommand.Create(() =>
        {
            CoreLogic.OpenUrl("https://www.youtube.com/watch?v=mBgIBF9Y6PE");
        });

        AttachCommand = ReactiveCommand.Create(() =>
        {
            CoreLogic.OpenUrl("https://www.acronymfinder.com/To-Be-Implemented-(TBI).html");
        });

        ContextMessageCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedMessage?.Content == null)
                return;

            Console.WriteLine($"Requested Context Menu on Message \"{SelectedMessage.Content}\"");
        });

        SelectMessageCommand = ReactiveCommand.Create<MainWindowViewModel, Unit>(o => Unit.Default);
    }

    private async Task UpdateOnlineStatus()
    {
        if (await EasyApiClient.Instance.Ping(Contact))
        {
            IsOnlineIndicator = Brushes.LawnGreen;
            return;
        }

        IsOnlineIndicator = Brushes.Red;
    }

    #endregion

    #region Properties

    public ContactCard Contact { get; }

    public TextMessage SelectedMessage
    {
        get => _selectedMessage;
        set => this.RaiseAndSetIfChanged(ref _selectedMessage, value);
    }

    public Conversation Conversation
    {
        get => _conversation;
        set => this.RaiseAndSetIfChanged(ref _conversation, value);
    }

    public ISolidColorBrush IsOnlineIndicator
    {
        get => _isOnlineIndicator;
        set => this.RaiseAndSetIfChanged(ref _isOnlineIndicator, value);
    }

    #endregion

    #region Commands

    public ICommand ContextMessageCommand { get; set; }
    public ICommand SelectMessageCommand { get; set; }
    public ICommand VoiceCallCommand { get; set; }
    public ICommand VideoCallCommand { get; set; }
    public ICommand RecordCommand { get; set; }
    public ICommand AttachCommand { get; set; }
    public ICommand EmoteCommand { get; set; }
    public ICommand SendCommand { get; set; }

    #endregion
}