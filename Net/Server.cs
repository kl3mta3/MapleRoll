using System;
using System.Configuration;
using System.Net.Sockets;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using MapleRoll;
using MapleRoll.Connect;
using MapleRoll.Net.IO;

namespace MapleRoll.Net
{
    public class Server
    {
        public event Action connectedEvent;
        public event Action disconnectedEvent;
        public event Action groupIdSentEvent;
        public event Action messageReceivedEvent;
        public event Action rollEvent;
        public event Action endRollEvent;
        public event Action endNeedGreedRollEvent;
        public event Action systemMessageEvent;
        public event Action migrateUserEvent;
        public event Action needGreedRollEvent;
        public event Action privateMessagelEvent;



        internal static MapleRollConnect connectWindow { get; set; }

        public TcpClient client;
        public PacketBuilder packetBuilder;
        public PacketReader packetReader;
        private string testserverIP = "127.0.0.1";
        
        private string serverIP = "99.160.195.3";

        private int serverPort = 42069;

        public Server()
        {

             client = new TcpClient();

            client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 300);
            client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 300);
            //client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 100);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            connectWindow = new MapleRollConnect();
        }

        public void ConnectToServer(string userName,string groupID)
        {

            
            if (!client.Connected)
            {
       
                    //Console.WriteLine($"[{DateTime.Now}]:Trying to connect.");
                try
                {
                    client.Connect(serverIP, serverPort);
                    
     

                    if (client.Connected)
                    {
                        //Console.WriteLine($"[{DateTime.Now}]:Connected Successfully to {client.Client.RemoteEndPoint.ToString()}.");
                        packetReader = new PacketReader(client.GetStream());
                        var connectPacket = new PacketBuilder();
                        string message = $"{userName}:{groupID}";
                        connectPacket.WriteOpCode(0);
                        connectPacket.WriteMessage(message);
                        //Console.WriteLine(message);
                        client.Client.Send(connectPacket.GetPacketBytes());
                        ReadPackets();
                       
                    }
                    else
                    {
                        MainWindow.connectWindow.ConnectionError();
                        MessageBox.Show($"[{DateTime.Now}]:Failed to Connect to {client.Client.RemoteEndPoint.ToString()}.");
                        //Console.WriteLine($"[{DateTime.Now}]:Failed to Connect to {client.Client.RemoteEndPoint.ToString()}.");
                    }
                }
                catch (Exception e) 
                {
                    MessageBox.Show(e.Message);
                }

               
            }

        }

        public void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {

                    var opcode = packetReader.ReadByte();
                 
                    switch (opcode)
                    {
                        case 1:
                            
                            try
                            {
                                if (opcode == 1)
                                {
                                    var groupId = packetReader.ReadMessage();
                                    var uid= packetReader.ReadMessage();
                                    //var username= packetReader.ReadMessage();
                                    MapleRollConnect.groupId = groupId;
                                    MapleRollConnect.uId = uid;
                                    MapleRollConnect.thisUser.UID = uid;
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved Client Connection info GroupID: {groupId}    UId: {uid}");
                                    groupIdSentEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            break;
                        case 2:
                            
                            try
                            {
                                if (opcode == 2)
                                {
                                                                      
                             
                                    connectedEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            break;

                        case 5:
                            try
                            {
                                if (opcode == 5)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved MessageEvent");
                                    var message = packetReader.ReadMessage();
                                    MapleRollConnect.MessageReceived(message,5);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            break;

                        case 7:
                            try
                            {
                                if (opcode == 7)
                                {
                                    var groupID = packetReader.ReadMessage();
                                    var uid = packetReader.ReadMessage();
                                    var roll = packetReader.ReadMessage();
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved RollEvent recieved for group {groupID} user{uid} rolled a {roll}");
                                    
                                    MapleRollConnect.ProcessRoll(groupID,uid,roll);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            break;

                        case 10:
                            try
                            {
                                if (opcode == 10)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved DisconnectedEvent");
                                    disconnectedEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;
                            }
                            break;

                        case 12:

                            try 
                            { 
                                if (opcode == 12)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved Vote to end RollEvent");
                                    endRollEvent?.Invoke();
                                }
                                    
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 15:

                            try
                            {
                                if (opcode == 15)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved Need RollEvent");
                                    needGreedRollEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 16:

                            try
                            {
                                if (opcode == 16)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved Flip Event");
                                    var message = packetReader.ReadMessage();
                                    MapleRollConnect.MessageReceived(message, 16);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 17:

                            try
                            {
                                if (opcode == 17)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved RPS Event");
                                    var message = packetReader.ReadMessage();
                                    MapleRollConnect.MessageReceived(message, 17);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;


                        case 18:

                            try
                            {
                                if (opcode == 18)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved Vote to end NeedGreedRollEvent");
                                    endNeedGreedRollEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 20:

                            try
                            {
                                if (opcode == 20)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved System Message Event");
                                    var uid= packetReader.ReadMessage();
                                    var message = packetReader.ReadMessage();
                                    if (uid == MapleRollConnect.uId)
                                    {
                                        MapleRollConnect.SystemMessageRecieved($"[System]: {message}");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 21:

                            try
                            {
                                if (opcode == 21)
                                {
                                    var groupId = packetReader.ReadMessage();
                                    var uid = packetReader.ReadMessage();
                                    MapleRollConnect.groupId = groupId;
                                    MapleRollConnect.mainWindow.groupId = groupId;
                                    Application.Current.Dispatcher.Invoke((() =>
                                    {
                                        MapleRollConnect.mainWindow.lbl_GroupID.Content = groupId;
                                        MapleRollConnect.SystemMessageRecieved($"[System]: This Group was migrated to a new Group with a new ID.");
                                    }));

                                    //migrateUserEvent?.Invoke();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 22:

                            try
                            {
                                if (opcode == 22)
                                {
                                    //Console.WriteLine($"[{DateTime.Now}]: Recieved System DC Event");
                                    var uid= packetReader.ReadMessage();
                                    var message = packetReader.ReadMessage();
                                    if (uid == MapleRollConnect.uId)
                                    {
                                        MapleRollConnect.SystemMessageRecieved($"[System]: {message}.");
                                        //disconnectedEvent?.Invoke();
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;

                        case 25:

                            try
                            {
                                if (opcode == 25)
                                {
                                    var senderUid = packetReader.ReadMessage();
                                    var msg = packetReader.ReadMessage();
                                    User user= MapleRollConnect.GetUserbyUID(senderUid);
                                    string message = $"[{user.Username} whispers]: {msg}";
                                    MapleRollConnect.MessageReceived(message, 25);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                break;

                            }
                            break;


                        default:
                           // Console.WriteLine("Yeah yeah yeah...");
                            break;
                    }



                }



            });
        }

        public void SendMessageToServerGroup(string groupID, string msg)
        {
            var messagePacket = new PacketBuilder();
            //Console.WriteLine($" Sending Message:[{msg}] to group ID [{groupID}] ");
            string message = $"{groupID}:{MapleRollConnect.userName} Says: {msg}   ";
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
            Console.WriteLine($"[{DateTime.Now}]: Message Sent to server Group {message}.");


        }

        public void SendPrivateMessageToServerGroup(string groupID, string receiverUid,string msg)
        {
            var messagePacket = new PacketBuilder();
            Console.WriteLine($" Sending PM:to [{receiverUid}] ");
            string message = $"{groupID}:{MapleRollConnect.uId}:{receiverUid}:{msg}   ";
            messagePacket.WriteOpCode(25);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
            Console.WriteLine($"[{DateTime.Now}]: Message Sent to server Group {message}.");


        }
        public void SendCoinFlipToServerGroup(string groupID, string msg)
        {
            var messagePacket = new PacketBuilder();    
            string message = $"{groupID}:[Coin] {msg}.   ";
            messagePacket.WriteOpCode(16);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
            Console.WriteLine($"[{DateTime.Now}]: Coin Flip Sent to server Group {message}.");


        }
        public void SendRPSToServerGroup(string groupID, string msg)
        {
            var messagePacket = new PacketBuilder();
            string message = $"{groupID}:[RPS] {msg}.   ";
            messagePacket.WriteOpCode(17);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
            Console.WriteLine($"[{DateTime.Now}]: RPS sent to server Group {message}.");


        }
    }
}
