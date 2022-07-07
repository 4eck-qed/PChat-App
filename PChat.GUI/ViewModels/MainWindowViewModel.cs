using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using DynamicData;
using Google.Protobuf;
using JetBrains.Annotations;
using Pchat;
using PChat.API.Client;
using PChat.Extensions;
using PChat.Shared;

// ReSharper disable once CheckNamespace
namespace PChat.GUI
{
    /// <summary>
    /// Contains everything you see in the main window. <br/>
    /// Everything else is contained as further view models.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly SharedViewModel _shared;
        private readonly MetaViewModel _metaViewModel;
        private readonly CancellationToken _cancellationToken;

        #region Fields

        private ObservableCollection<TextMessage> _notifications;
        private TextMessage _selectedNotification;

        private ObservableCollection<ChatViewModel> _chats;
        private ChatViewModel _selectedChat;

        private ContactCard _selectedContact;

        private Group _selectedGroup;

        #endregion


        public MainWindowViewModel(CancellationToken cancellationToken)
        {
            Console.WriteLine("Initializing main window view model..");
            _cancellationToken = cancellationToken;
            Notifications = new ObservableCollection<TextMessage>();
            Chats = new ObservableCollection<ChatViewModel>();
            InitCommands();

            this.WhenAnyValue(x => x.Notifications)
                .Subscribe(notifications => Console.WriteLine("Notifications updated."));

            Shared = new SharedViewModel(new EasyApiClient(true));
            _metaViewModel = new MetaViewModel(Shared, cancellationToken);

            EventBus.Instance.Subscribe(this);
        }

        private void InitCommands()
        {
            ClearNotificationsCommand = ReactiveCommand.Create(() =>
            {
                Notifications.Clear();
                this.RaisePropertyChanged(nameof(Notifications));
            });

            SelectNotificationCommand = ReactiveCommand.Create(() =>
            {
                if (SelectedNotification == null) return;
                Console.WriteLine($"Selected Notification from {SelectedNotification.SenderId.ToHexString()} " +
                                  $"with Message \"{SelectedNotification.Content}\"");

                OpenChat(SelectedNotification.SenderId);
                Notifications.Remove(SelectedNotification);
            });

            AcceptFriendCommand = ReactiveCommand.Create(() =>
            {
                // Friends.Add(SelectedFriendRequest);
                // FriendRequests.Remove(SelectedFriendRequest);
            });
        }

        public MainWindowViewModel() : this(new CancellationToken())
        {
            Console.WriteLine("[TEST] MainWindowViewModel was initialized.");
        }

        [UsedImplicitly]
        public void OnEvent(OnObjectChangedEvent e)
        {
            if (e.ObjectName == nameof(SessionContent.Contacts))
            {
                var contactsIds = SessionContent.Contacts.Select(x => x.Id);
                var chatsIds = Chats.Select(x => x.Contact.Id);
                var notFriendsAnymore = Chats.Where(x => !contactsIds.Contains(x.Contact.Id));
                var newFriends = SessionContent.Contacts.Where(x => !chatsIds.Contains(x.Id));
                Chats.RemoveMany(notFriendsAnymore);
                Chats.Add(newFriends.Select(x => new ChatViewModel(Shared, x)));
            }

            this.RaisePropertyChanged(nameof(e.ObjectName));
        }

        #region Private Methods

        private void OpenChat(ByteString contactId)
        {
            var contact = SessionContent.Contacts.FirstOrDefault(c => c.Id.Equals(contactId));
            if (contact == null) // TODO Display a red message in place of chat.
            {
                Console.WriteLine("Contact not found. You are probably not friends with them.");
                return;
            }

            SelectedContact = contact;
            SelectedChat = Chats.FirstOrDefault(chat => chat.Contact.Equals(contact));
        }

        private void ReplaceNotification(ByteString senderId, TextMessage newTextMessage)
        {
            foreach (var notification in Notifications)
            {
                if (notification.SenderId != senderId) continue;
                notification.Content = newTextMessage.Content;
                notification.Time = newTextMessage.Time;
                break;
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<TextMessage> Notifications
        {
            get => _notifications;
            set => this.RaiseAndSetIfChanged(ref _notifications, value);
        }

        public TextMessage? SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedNotification, value);
            }
        }

        public ObservableCollection<ChatViewModel> Chats
        {
            get => _chats;
            set => this.RaiseAndSetIfChanged(ref _chats, value);
        }

        public ChatViewModel? SelectedChat
        {
            get => _selectedChat;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedChat, value);
            }
        }

        public ContactCard? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedContact, value);
                SelectedChat = Chats.FirstOrDefault(chat => chat.Contact.Equals(value));

                Console.WriteLine($"Selected Contact: {value.Name}");
                foreach (var notification in Notifications.ToArray())
                {
                    if (notification.SenderId != SelectedContact?.Id) continue;
                    // remove notifications for this contact, since we are already looking at this chat
                    Notifications.RemoveMany(Notifications.Where(x => x.SenderId == SelectedChat?.Contact.Id));
                }
            }
        }

        public Group? SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedGroup, value);
            }
        }

        public SharedViewModel Shared
        {
            get => _shared;
            private init => this.RaiseAndSetIfChanged(ref _shared, value);
        }

        public static SessionContent Session => SessionContent.Singleton;

        #endregion

        #region Commands

        public ICommand ClearNotificationsCommand { get; set; }
        public ICommand SelectNotificationCommand { get; set; }
        public ICommand SelectContactCommand { get; set; }
        public ICommand AcceptFriendCommand { get; set; }

        #endregion
    }
}