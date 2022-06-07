using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using PChat.GUI.Core;
using PChat.Extensions;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace PChat.GUI;

/// <summary>
/// Contains everything specific to a chat.
/// </summary>
public class ChatViewModel : ViewModelBase
{
    protected SharedViewModel Shared { get; }

    #region Fields

    private string _messageInput;
    private ObservableCollection<Message> _messages;
    private Message _selectedMessage;
    private ISolidColorBrush _isOnlineIndicator = Brushes.Red;

    #endregion

    public ChatViewModel(SharedViewModel shared, ContactCard receiver)
    {
        Shared = shared;
        Receiver = receiver;
        InitializeCommands();

        Task.Run(async () =>
        {
            var messages = await Shared.ApiClient.GetMessages(receiver.Id);
            Messages = new ObservableCollection<Message>(messages.OrderBy(m => m.Time));
        });
    }

    #region Private Methods

    private void InitializeCommands()
    {
        SendCommand = ReactiveCommand.Create(() =>
        {
            if (string.IsNullOrEmpty(MessageInput)) return;
            var message = new Message
            {
                Id = ByteStringExtensions.RandomByteString(16),
                Sender = Shared.Profile,
                Receiver = Receiver,
                Content = MessageInput,
                IsNativeOrigin = true,
                Time = DateTime.Now
            };

            Task.Run(async () =>
            {
                MessageInput = "";
                await UpdateOnlineStatus();
                await Send(message);
            });
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

    private async Task Send(Message message)
    {
        Messages.Add(message);
        this.RaisePropertyChanged(nameof(Messages));

        var outgoingMessage = new Message(message) {IsNativeOrigin = false};

        await Shared.ApiClient.Ping(Receiver).ContinueWith(async pingTask =>
        {
            if (!pingTask.IsCompletedSuccessfully)
            {
                Shared.MessageQueue.Enqueue(outgoingMessage);
                return;
            }

            await Shared.ApiClient.Send(outgoingMessage).ContinueWith(sendTask =>
            {
                if (!sendTask.IsCompletedSuccessfully)
                {
                    Shared.MessageQueue.Enqueue(outgoingMessage);
                }
            });
        });
    }

    private async Task UpdateOnlineStatus()
    {
        if (await Shared.ApiClient.Ping(Receiver))
        {
            IsOnlineIndicator = Brushes.LawnGreen;
            return;
        }

        IsOnlineIndicator = Brushes.Red;
    }

    #endregion

    #region Properties

    public string MessageInput
    {
        get => _messageInput;
        set => this.RaiseAndSetIfChanged(ref _messageInput, value);
    }

    public ContactCard Receiver { get; }

    public ObservableCollection<Message> Messages
    {
        get => _messages;
        set => this.RaiseAndSetIfChanged(ref _messages, value);
    }

    public Message SelectedMessage
    {
        get => _selectedMessage;
        set => this.RaiseAndSetIfChanged(ref _selectedMessage, value);
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