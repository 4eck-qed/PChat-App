# PChat
What is that?\
It's a P2P (peer to peer) messaging application.\
The whole project consists of 3 elements.\
Main app (this repo), API and Lookup Server.\
The API provides calls for all communication and if necessary connectes to the Lookup Server.\
The Lookup Server contains all user endpoints matched to their IDs so users can add other users by simply providing the ID.

# PChat-App
This is sort of the entry point for the whole project. 
The repository solely contains the frontend program that contains the GUI, thus the program which the user will start.


# Get Started
This is a P2P (peer to peer) chat client so you need to open the required port in your firewall and router.\
The required port for P2P traffic is 50053.

To receive P2P connections you need to (1) add a rule to your firewall and (2) open the port on your router.

## (1) Add A Rule To Your Firewall

### Windows

Open the Windows Defender:
go to "Advanced settings" -> "Inbound Rules" -> "New Rule.."

- [x] Port\
[Next]
- [x] TCP
- Specific local ports: 50053\
[Next]
- [x] Allow the connection\
[Next]\
[Next]\
Enter some desired name\
[Finish]
 
### Linux
 
Open the Terminal and enter:
```
sudo iptables -A INPUT -p tcp --dport 50053 -j ACCEPT
sudo iptables-save
```

## (2) Open the port in your router

Find out what your standard gateway is e.g. 192.168.1.1
- In Windows: Open CMD and enter: ```ipconfig```
- In Linux: Open the Terminal and enter: ```ip a```

Open a web browser and enter the ip in the URL field - this will open your router's interface.\
From there it differs based on what model your router is because every interface looks way different.\
There could be something called port forwarding or virtual server where you can add the rule.\
Same as for (1) the port you want to forward is 50053.\
The rule could look something like this:

| Service    | External Port | Internal IP   | Internal Port | Protocol |
| :--------: | :-----------: | :-----------: | :-----------: | :------: |
| PChat P2P  |     50053     | 192.168.1.103 |     50053     |   TCP    |

If you don't find the option there is maybe an option to show advanced settings.
