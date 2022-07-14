using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly CancellationToken _cancellationToken;
        private Dictionary<string, string> _sessionProperties;

        #region Fields

        private ObservableCollection<TextMessage> _notifications;
        private TextMessage _selectedNotification;

        private ObservableCollection<ChatViewModel> _chats;
        private ChatViewModel? _selectedChat;

        private ContactViewModel _selectedContact;

        private Group _selectedGroup;
        private ObservableCollection<ContactViewModel> _contacts;
        
        private ObservableCollection<TextMessage> _queuedMessages;
        private ObservableCollection<DateTime> _loginHistory;

        #endregion


        public MainWindowViewModel(CancellationToken cancellationToken)
        {
            Console.WriteLine("Initializing main window view model..");
            InitProperties();
            
            Profile = new ProfileViewModel(Session.Account);
            _cancellationToken = cancellationToken;

            Task.Run(async () =>
                {
                    await EasyApiClient.Instance.LoadContacts();
                    await EasyApiClient.Instance.LoadFriendRequests();
                })
                .ContinueWith(_ => Contacts =
                    new ObservableCollection<ContactViewModel>(Session.Contacts.Select(x => new ContactViewModel(x))));

            InitCommands();

            this.WhenAnyValue(x => x.Notifications)
                .Subscribe(notifications => Console.WriteLine("Notifications updated ({0}).", notifications.Count));

            EventBus.Instance.Subscribe(this);
            
            Task.Run(async () =>
            {
                QueuedMessages = new ObservableCollection<TextMessage>(await EasyApiClient.Instance.GetQueuedMessages());
                LoginHistory = new ObservableCollection<DateTime>(await EasyApiClient.Instance.GetLoginHistory());
            });
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

        private void InitProperties()
        {
            Notifications = new ObservableCollection<TextMessage>();
            Chats = new ObservableCollection<ChatViewModel>();
            SessionProperties = new Dictionary<string, string>
            {
                {nameof(Account), nameof(Session.Account)},
                {nameof(Contacts), nameof(Session.Contacts)},
                {nameof(Conversations), nameof(Session.Conversations)},
                {nameof(Groups), nameof(Session.Groups)},
                {nameof(FriendRequests), nameof(Session.FriendRequests)}
            };
        }

        public MainWindowViewModel() : this(new CancellationToken())
        {
            Console.WriteLine("[TEST] MainWindowViewModel was initialized.");
        }

        [UsedImplicitly]
        public void OnEvent(OnObjectChangedEvent e)
        {
            if (e.ObjectName == nameof(Session.Contacts))
            {
                var contactsIds = Session.Contacts.Select(x => x.Id);
                var chatsIds = Chats.Select(x => x.Contact.Id);
                var notFriendsChats = Chats.Where(x => !contactsIds.Contains(x.Contact.Id)).ToList();
                var notFriendsIds = notFriendsChats.Select(x => x.Contact.Id);
                var newFriendsCards = Session.Contacts.Where(x => !chatsIds.Contains(x.Id)).ToList();
                Chats.RemoveMany(notFriendsChats);
                Chats.Add(newFriendsCards.Select(x => new ChatViewModel(x)));
                if (!Chats.Select(x => x.Contact.Id).Contains(SelectedChat?.Contact.Id))
                    SelectedChat = null;
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (Contacts == null) return;
                if (Contacts.Any())
                    Contacts.RemoveMany(Contacts.Where(x => notFriendsIds.Contains(x.Card?.Id)));
                Contacts.Add(newFriendsCards.Select(x => new ContactViewModel(x)));
                foreach (var contact in Contacts)
                {
                    var card = Session.Contacts.FirstOrDefault(x => x.Id == contact.Card?.Id);
                    contact.Card = card!;
                }
            }

            foreach (var (k, v) in SessionProperties.Where(x => e.ObjectName == x.Value))
            {
                Console.WriteLine($"[DEBUG] '{k}' ('{v}') changed");
                this.RaisePropertyChanged(k);
            }

            this.RaisePropertyChanged(e.ObjectName);
        }

        #region Private Methods

        private void OpenChat(ByteString contactId)
        {
            var contact = Contacts.FirstOrDefault(x => x.Card != null && x.Card.Id.Equals(contactId));
            if (contact == null) // TODO Display a red message in place of chat.
            {
                Console.WriteLine("Contact not found. You are probably not friends with them.");
                return;
            }

            SelectedContact = contact;
            SelectedChat = Chats.FirstOrDefault(chat => chat.Contact.Equals(contact.Card));
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

        /// <summary>
        /// Used for collectively calling property changed events. 
        /// </summary>
        private Dictionary<string, string> SessionProperties
        {
            get => _sessionProperties;
            set => this.RaiseAndSetIfChanged(ref _sessionProperties, value);
        }

        public Account Account => Session.Account;

        public ObservableCollection<ContactViewModel> Contacts
        {
            get => _contacts;
            set => this.RaiseAndSetIfChanged(ref _contacts, value);
        }

        public ObservableCollection<Conversation> Conversations => Session.Conversations;

        public ObservableCollection<ContactCard> Groups => Session.Groups;

        public ObservableCollection<FriendRequest> FriendRequests => Session.FriendRequests;

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
            set => this.RaiseAndSetIfChanged(ref _selectedChat, value);
        }

        public ContactViewModel? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedContact, value);
                SelectedChat = Chats.FirstOrDefault(chat => chat.Contact.Equals(value.Card));

                Console.WriteLine($"Selected Contact: {value.Card?.Name}");
                foreach (var notification in Notifications.ToArray())
                {
                    if (notification.SenderId != SelectedContact?.Card?.Id) continue;
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

        public ProfileViewModel Profile { get; }
        
        public ObservableCollection<TextMessage> QueuedMessages
        {
            get => _queuedMessages;
            set => this.RaiseAndSetIfChanged(ref _queuedMessages, value);
        }

        public ObservableCollection<DateTime> LoginHistory
        {
            get => _loginHistory;
            set => this.RaiseAndSetIfChanged(ref _loginHistory, value);
        }

        #endregion

        #region Commands

        public ICommand ClearNotificationsCommand { get; set; }
        public ICommand SelectNotificationCommand { get; set; }
        public ICommand SelectContactCommand { get; set; }
        public ICommand AcceptFriendCommand { get; set; }

        #endregion
    }
}