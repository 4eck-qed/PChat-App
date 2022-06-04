using ReactiveUI;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia.Media;
using DynamicData;
using Google.Protobuf;
using PChat.API.Client;
using PChat.Extensions;
using PChat.Log;

// ReSharper disable once CheckNamespace
namespace PChat.GUI
{
    /// <summary>
    /// Contains everything you see in the main window. <br/>
    /// Everything else is contained as further view models.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        protected SharedViewModel Shared { get; }

        private readonly MetaViewModel _metaViewModel;
        private readonly CancellationToken _cancellationToken;

        #region Fields

        private ObservableCollection<Message> _notifications;
        private Message _selectedNotification;

        private ObservableCollection<ChatViewModel> _chats;
        private ChatViewModel _selectedChat;

        private ObservableCollection<ContactCard> _friendRequests;
        private ObservableCollection<ContactCard> _friends;
        private ContactCard _selectedFriend;

        private ObservableCollection<Group> _groups;
        private Group _selectedGroup;

        #endregion


        public MainWindowViewModel(Login login, CancellationToken cancellationToken)
        {
            Console.WriteLine("Initializing main window view model..");
            _cancellationToken = cancellationToken;
            Notifications = new ObservableCollection<Message>();
            FriendRequests = new ObservableCollection<ContactCard>();
            Friends = new ObservableCollection<ContactCard>();
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
                Console.WriteLine($"Selected Notification from {SelectedNotification.Sender.Name} " +
                                  $"with Message \"{SelectedNotification.Content}\"");

                OpenChat(SelectedNotification.Sender);
                Notifications.Remove(SelectedNotification);
            });

            AddFriendCommand = ReactiveCommand.Create(() =>
            {
                if (string.IsNullOrWhiteSpace(AddContactIdHexString)) return;

                // Shared.ApiClient.AddContact(HexString.ToByteString(AddContactIdHexString));
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
                Id = login.Id,
                AvatarImageSource = $"avares://PChat.GUI/Assets/Images/samples/earl.png",
                Name = "Earl",
                NameColor = Colors.Cornsilk.ToString(),
                Status = "My name is Earl!"
            };
            Shared = new SharedViewModel(login, earl, new ApiClient(login, true), new ConcurrentQueue<Message>());
            _metaViewModel = new MetaViewModel(Shared, cancellationToken);

            var randy = new ContactCard
            {
                Id = ByteString.CopyFrom(1, 2, 3, 4, 5),
                AvatarImageSource = $"avares://PChat.GUI/Assets/Images/samples/randy.png",
                Name = "Randy",
                NameColor = Colors.Maroon.ToString(),
                Status = ".."
            };
            Friends.Add(randy);
            FriendRequests.Add(randy);
            Chats.Add(new ChatViewModel(Shared, randy));
        }

        public MainWindowViewModel() : this(new Login(), new CancellationToken())
        {
            Console.WriteLine("[TEST] MainWindowViewModel was initialized.");
        }

        #region Private Methods

        private void AddContact(ContactCard contact)
        {
            Friends.Add(contact);
            Chats.Add(new ChatViewModel(Shared, contact));
        }

        private void OpenChat(ContactCard? contact)
        {
            if (contact == null)
            {
                PLogger.Singleton.ToConsole("OpenChat for null requested!");
                return;
            }

            SelectedFriend = contact;
            SelectedChat = Chats.FirstOrDefault(chat => chat.Receiver == contact);
        }

        private void ReplaceNotification(ByteString senderId, Message newMessage)
        {
            foreach (var notification in Notifications)
            {
                if (notification.Sender.Id != senderId) continue;
                notification.Replace(newMessage);
                break;
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<Message> Notifications
        {
            get => _notifications;
            set => this.RaiseAndSetIfChanged(ref _notifications, value);
        }

        public Message? SelectedNotification
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

        public ObservableCollection<ContactCard> FriendRequests
        {
            get => _friendRequests;
            set => _friendRequests = value;
        }

        public ObservableCollection<ContactCard> Friends
        {
            get => _friends;
            set => _friends = value;
        }

        public ContactCard? SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref _selectedFriend, value);
                SelectedChat = Chats.FirstOrDefault(chat => chat.Receiver == value);

                Console.WriteLine($"Selected Contact: {value.Name}");
                foreach (var notification in Notifications.ToArray())
                {
                    if (notification.Sender != SelectedFriend) continue;
                    // remove notifications for this contact, since we are already looking at this chat
                    if (SelectedChat?.Messages != null)
                    {
                        Notifications.RemoveMany(SelectedChat.Messages);
                    }
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

        public DateTime LastLogin => _metaViewModel.GetLastLogin();

        public string IdHexString => Shared.ApiClient.Login.Id.ToHexString();
        public string KeyHexString => Shared.ApiClient.Login.Key.ToHexString();

        public string AddContactIdHexString { get; set; }

        #endregion

        #region Commands

        public ICommand ClearNotificationsCommand { get; set; }
        public ICommand SelectNotificationCommand { get; set; }
        public ICommand SelectContactCommand { get; set; }

        public ICommand AddFriendCommand { get; set; }
        public ICommand AcceptFriendCommand { get; set; }

        #endregion
    }
}