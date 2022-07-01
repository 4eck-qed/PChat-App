using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using DynamicData;
using Google.Protobuf;
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

        private ObservableCollection<FriendRequest> _friendRequests;
        private ObservableCollection<ContactCard> _contacts;
        private ContactCard _selectedContact;

        private ObservableCollection<Group> _groups;
        private Group _selectedGroup;

        #endregion


        public MainWindowViewModel(CancellationToken cancellationToken)
        {
            Console.WriteLine("Initializing main window view model..");
            _cancellationToken = cancellationToken;

            Notifications = new ObservableCollection<TextMessage>();
            FriendRequests = new ObservableCollection<FriendRequest>();
            Contacts = new ObservableCollection<ContactCard>();
            Chats = new ObservableCollection<ChatViewModel>();
            Groups = new ObservableCollection<Group>();

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

            this.WhenAnyValue(x => x.Notifications)
                .Subscribe(notifications => Console.WriteLine("Notifications updated."));

            var earl = new ContactCard
            {
                Id = SessionContent.Account.Id,
                Avatar = ByteString.CopyFromUtf8("avares://PChat.GUI/Assets/Images/samples/earl.png"),
                Name = "Earl",
                Status = "My name is Earl!"
            };
            var credentials = new Credentials {Id = SessionContent.Account.Id, Key = SessionContent.Account.Key};
            Shared = new SharedViewModel(new Client(credentials, true));
            _metaViewModel = new MetaViewModel(Shared, cancellationToken);

            var randy = new ContactCard
            {
                Id = SessionContent.Account.Id,
                Avatar = ByteString.CopyFromUtf8("avares://PChat.GUI/Assets/Images/samples/randy.png"),
                Name = "Randy",
                Status = ".."
            };
            Contacts.Add(randy);
            FriendRequests.Add(new FriendRequest
            {
                Id = SessionContent.Account.Id,
                SenderId = SessionContent.Account.Id,
                TargetId = SessionContent.Account.Id,
                Status = FriendRequestStatus.Pending
            });
            Chats.Add(new ChatViewModel(Shared, randy));
        }

        public MainWindowViewModel() : this(new CancellationToken())
        {
            Console.WriteLine("[TEST] MainWindowViewModel was initialized.");
        }

        #region Private Methods

        private void OpenChat(ByteString contactId)
        {
            var contact = SessionContent.ContactList.FirstOrDefault(c => c.Id.Equals(contactId));
            if (contact == null) // TODO Display a red message in place of chat.
            {
                Console.WriteLine("Contact not found. You are probably not friends with them.");
                return;
            }

            SelectedContact = contact;
            SelectedChat = Chats.FirstOrDefault(chat => chat.Receiver.Equals(contact));
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

        public ObservableCollection<FriendRequest> FriendRequests
        {
            get => _friendRequests;
            set => _friendRequests = value;
        }

        public ObservableCollection<ContactCard> Contacts

        {
            get => _contacts;
            set => _contacts = value;
        }

        public ContactCard? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedContact, value);
                SelectedChat = Chats.FirstOrDefault(chat => chat.Receiver.Equals(value));

                Console.WriteLine($"Selected Contact: {value.Name}");
                foreach (var notification in Notifications.ToArray())
                {
                    if (notification.SenderId != SelectedContact?.Id) continue;
                    // remove notifications for this contact, since we are already looking at this chat
                    Notifications.RemoveMany(SessionContent.Messages[SelectedChat?.Receiver.Id!]);
                }
            }
        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => _groups = value;
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

        public SessionContent Session => SessionContent.Singleton;

        #endregion

        #region Commands

        public ICommand ClearNotificationsCommand { get; set; }
        public ICommand SelectNotificationCommand { get; set; }
        public ICommand SelectContactCommand { get; set; }
        public ICommand AcceptFriendCommand { get; set; }

        #endregion
    }
}