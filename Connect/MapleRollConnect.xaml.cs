using MapleRoll.Net;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using MapleRoll;
using MapleRoll.Net.IO;
using System.Net.Sockets;
using System.Windows.Shell;
using System.Windows.Controls;
using System.Windows.Media;
using MapleRoll.Roll;
using System.Windows.Input;
using System.IO;
using System.IO.Packaging;
using System.Windows.Shapes;
using System.Media;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using MapleRoll.Greed;
using System.Drawing;
using System.Windows.Interop;
using System.Reflection.PortableExecutable;

namespace MapleRoll.Connect
{

    public partial class MapleRollConnect : Window
    {

        public static List<User> groupMembers = new List<User>();
        public static User thisUser = new User();
        public ConnectionError connectionError = new ConnectionError();
        public static Dictionary<User, int> currentRoll = new Dictionary<User, int>();
        public static Server _server { get; set; }
        public static MainWindow mainWindow { get; set; }
        public static RollWindow rollWindow { get; set; }
        public static NeedGreedWindow needGreedWindow { get; set; }
        PacketReader packetReader { get; set; }
        public static string userName { get; set; }
        public static string groupId { get; set; }
        public static string uId { get; set; }
        public bool isConnectedToServer = false;
        public static int userSoundProfile = 1;
        //roll data
        public static int winningRoll = 0;
        public static int tiedRoll = 0;
        public static User winningRollUser = new User();
        public static int userRoll = 0;
        public static bool hasRolled = false;
        public static bool rollStarted = false;
        public static List<User> tiedRollUsers = new List<User>();

        //Stats data
        public static decimal totalUserRollCount = 0;
        public static decimal userTotalRollSum = 0;
        public static decimal totalPerfectRollCount = 0;
        public static decimal totalOneRolls = 0;
        public static decimal totalWinningRolls = 0;
        private static string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string statsSavePath = System.IO.Path.Combine(systemPath, @"MapleRoll");
        private static string fileName = "stats.txt";
        private static string filePath = System.IO.Path.Combine(statsSavePath, $"{fileName}");

        //Vote to end roll info
        public static int votesForEndingRoll = 0;
      
        public static bool voteToEndRollStarted=false;
        public static bool userHasVotedToEndRoll = false;
        public static bool rollToEndVoteDone = false;
        public static List<string> endRollVotes = new List<string>();

        //need > greed

        public static Dictionary<User, int> needRolls = new Dictionary<User, int>();
        public static Dictionary<User, int> greedRolls = new Dictionary<User, int>();
        public static User winningNeedUser= new User();
        public static User winningGreedUser = new User();
        public static User winningNeedGreedUser = new User();
        public static bool hasNeedGreedRolled = false;
        public static bool needGreedRollStarted = false;
        public static bool isNeedRoll = false;
        public static bool isGreedRoll = false;
        public static bool someoneRolledNeed = false;
        public static bool userRolledNeed = false;
        public static bool userRolledGreed = false;
        public static int winningNeedRoll = 0;
        public static int winningGreedRoll = 0;
        public static int winningNeedGreedRoll = 0;
        public static int tiedGreedRoll = 0;
        public static int tiedNeedRoll = 0;
        public static int userNeedGreedRoll = 0;

        public static bool rollToEndNeedGreedVoteDone = true;
        public static List<string> endNeedGreedRollVotes= new List<string>();
        public static int votesForEndingNeedGreedRoll = 0;
        public static bool userHasVotedToEndNeedGreedRoll = false;


        public MapleRollConnect()
        {
            InitializeComponent();
             

        }
        public static void Disconnect()
        {
            if (_server.client.Connected)
            {
                _server.client.GetStream().Close();
                _server.client.Close();
            }
            groupMembers.Clear();
            Application.Current.Dispatcher.Invoke((() => { MainWindow.connectWindow.Show(); })); ;

            Application.Current.Dispatcher.Invoke((() => { mainWindow.Hide(); }));


        }
        public static void LoadUserStats()
        {

            Console.WriteLine("Checking for save file");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);


                Console.WriteLine("Save Found Loading Data");



                decimal _totalUserRollCount = decimal.Parse(lines[1]);
                decimal stat1 = DecryptStat(_totalUserRollCount);
                totalUserRollCount = stat1 ;

                decimal _userTotalRollSum = decimal.Parse(lines[2]);
                decimal stat2 = DecryptStat(_userTotalRollSum);
                userTotalRollSum = stat2;
                Console.WriteLine($"userTotalRollSum = {userTotalRollSum}");

                decimal _totalWinningRolls = decimal.Parse(lines[3]);
                decimal stat3 = DecryptStat(_totalWinningRolls);
                totalWinningRolls = stat3;
                Console.WriteLine($"totalWinningRollsm = {totalWinningRolls}");

                decimal _totalOneRolls = decimal.Parse(lines[4]);
                decimal stat4 = DecryptStat(_totalOneRolls);
                totalOneRolls = stat4;
                Console.WriteLine($"totalOneRolls = {totalOneRolls}");

                decimal _totalPerfectRollCount = decimal.Parse(lines[5]);
                decimal stat5 = DecryptStat(_totalPerfectRollCount);
                totalPerfectRollCount = stat5;
                Console.WriteLine($"totalPerfectRollCount = {totalPerfectRollCount}");

                mainWindow.UpdateUserStatDisplay();
                Console.WriteLine("Loaded Data");
            }
            else
            {

                Console.WriteLine("No Save file Found.");
            }
        }
        public static void SaveUserStats()
        {
            Console.WriteLine("Saving Roll Data");
            if (File.Exists(filePath))
            {
                Console.WriteLine(" Old save found, deleting it.");
                File.Delete(filePath);
            }
            else
            {
                if (!Directory.Exists(statsSavePath))
                {
                    Directory.CreateDirectory(statsSavePath);

                }

            }
            //FileStream fs = File.CreateText(filePath);
            using (StreamWriter sw = new StreamWriter(filePath))
            {

                    sw.WriteLine(DateTime.Now.ToString());
                if (totalUserRollCount>0)
                {

                    decimal stat1 = EncryptStat(totalUserRollCount);
                    sw.WriteLine($"{stat1}");
                }
                else
                {
                    sw.WriteLine($"{totalUserRollCount}");

                }

            if(userTotalRollSum>0)
            {
                    decimal stat2 = EncryptStat(userTotalRollSum);
                    sw.WriteLine($"{stat2}");

             }
            else 
             { 
                
                sw.WriteLine($"{userTotalRollSum}");
            }

                if (totalWinningRolls>0)
                {
                    decimal stat3 = EncryptStat(totalWinningRolls);
                    sw.WriteLine($"{stat3}");

                }
                else 
                { 
              
                sw.WriteLine($"{totalWinningRolls}");
                }

                if (totalOneRolls>0)
                {
                    decimal stat4 = EncryptStat(totalOneRolls);
                    sw.WriteLine($"{stat4}");
                }
                else
                {
                    sw.WriteLine($"{totalOneRolls}");
                }

                if (totalPerfectRollCount>0)
                {
                    decimal stat5 = EncryptStat(totalPerfectRollCount);
                    sw.WriteLine($"{stat5}");
                }
                else
                {
                   
                    sw.WriteLine($"{totalPerfectRollCount}");
                }
                    sw.WriteLine($"~END~");

                }
            mainWindow.UpdateUserStatDisplay();
            Console.WriteLine("Data Saved.");
        }
        public static decimal EncryptStat(decimal stat)
        {
            if (stat>0)
            {
                decimal number = ((stat * 69) / 2);
                return number;
            }
            else
            {
                return 0;
            }
        }
        public static decimal DecryptStat(decimal stat)
        {
            if (stat > 0)
            {
                decimal number = ((stat * 2) / 69);
                return number;
            }
            else
            {
                return 0;
            }

        }
        public void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txb_UserName.Text) && !string.IsNullOrEmpty(txb_GroupId.Text))
            {

                _server = new Server();
                mainWindow = new MainWindow();

                userName = txb_UserName.Text;
                thisUser.Username = userName.ToUpper();
                groupId = txb_GroupId.Text;
                _server.ConnectToServer(userName, groupId);
                _server.connectedEvent += UserConnected;
                _server.groupIdSentEvent += ConnectionSuccessful;
                _server.disconnectedEvent += UserDisconnected;
                _server.endRollEvent += VoteToEndRollRecieved;
                _server.needGreedRollEvent += ProcessNeedGreedRoll;
                _server.endNeedGreedRollEvent += VoteToEndNeedGreedRollRecieved;
                _server.migrateUserEvent += MigrateUser;
                //_server.rollEvent += ProcessRoll;VoteToEndNeedGreedRollRecieved
                //_server.messageReceivedEvent += MessageReceived;



            }
            else
            {
                MessageBox.Show("Please Enter a Username and GroupID");

            }
        }
        private void btn_ConnectNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txb_UserName.Text))
            {
                _server = new Server();
                mainWindow = new MainWindow();
                userName = txb_UserName.Text;
                groupId = "";

                _server.ConnectToServer(userName, groupId);
                _server.connectedEvent += UserConnected;
                _server.groupIdSentEvent += ConnectionSuccessful;
                _server.disconnectedEvent += UserDisconnected;
                _server.endRollEvent += VoteToEndRollRecieved;
                _server.needGreedRollEvent += ProcessNeedGreedRoll;
                _server.endNeedGreedRollEvent += VoteToEndNeedGreedRollRecieved;
                _server.migrateUserEvent += MigrateUser;

            }
            else
            {
                if (!string.IsNullOrEmpty(txb_UserName.Text))
                {
                    MessageBox.Show("Please Enter a Username");
                }

            }
        }
        public static void ProcessRoll(string groupID, string uid, string _roll)
        {

            var roll = int.Parse(_roll);
            Console.WriteLine($"Start user by UID search");
            User user = GetUserbyUID(uid);
            string username = GetUsernamebyUID(uid);
            if(!rollStarted)
            { 
                rollStarted = true;

                if (MainWindow.playSoundEnabled && groupMembers.Count>1)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.roll_default);

                    player.Load();
                    player.Play();
                }
                
            }
            if (!hasRolled)
            {
                rollWindow.UpdateUserRollData("", 2);
            }

            if (winningRoll !=0 && winningRoll == roll)
            {
                tiedRoll = roll;
                winningRollUser = new User();
                tiedRollUsers.Add(user);
                //SolidColorBrush white = new SolidColorBrush(Colors.White);
                rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"{winningRollUser.Username} is winning.", 1);

            }

            if (winningRoll < roll&& winningRoll != roll)
            {
                winningRoll = roll;
                winningRollUser = user;
                tiedRollUsers.Add(user);
                //SolidColorBrush white = new SolidColorBrush(Colors.White);
                rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"{winningRollUser.Username} is winning.", 1);

            }


            Console.WriteLine($"This Client UID{uId} The roller UID {uid}");
            if (uid == uId)
            {
                //clients roll update roll window.
                userRoll = roll;
                if (groupMembers.Count >= 2)
                {
                    if (userRoll == 100)
                    {
                        totalPerfectRollCount++;
                       
                    }
                    else if (userRoll == 1)
                    {
                        totalOneRolls++;
                        if (MainWindow.playSoundEnabled)
                        {
                            SoundBoard("1");
                        }

                    }
                    totalUserRollCount++;
                    userTotalRollSum += userRoll;
                }
               

                rollWindow.UpdateUserRollData(userRoll.ToString(), 1);

            }

            currentRoll.Add(user, roll);
            if(roll==100)
            {

                if(uid!=uId)
                {
                    // they win
                    if (MainWindow.playSoundEnabled)
                    {
                        SoundBoard("they100");
                    }
                    mainWindow.SendMessageToConsole($"[Roll]: {username} is a Wizzard! They rolled a {roll.ToString()}.",1);
                }
                else
                {
                    if (MainWindow.playSoundEnabled)
                    {
                        SoundBoard("you100");
                    }
                    mainWindow.SendMessageToConsole($"[Roll]: You're a Wizzard! You rolled a {roll.ToString()}.", 1);
                }


            }
            else
            {

                mainWindow.SendMessageToConsole($"[Roll]: {username} rolls a {roll.ToString()}.",6);
            }
          
            if (currentRoll.Count >= groupMembers.Count)
            {
                if(tiedRoll!=0)
                {
                    rollWindow.UpdateUserRollData(userRoll.ToString(), 2);

                    rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"You have tied!!!", 2);

                }
               else if (winningRollUser.UID == uId && tiedRoll == 0)
                {
                    if (groupMembers.Count >= 2)
                    {
                        totalWinningRolls++;
                    }
                    rollWindow.UpdateUserRollData(userRoll.ToString(), 2);

                    rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"You win!!!", 2);
                    
                    if(userName=="STICKY")
                    {
                        if (MainWindow.playSoundEnabled && userRoll != 1 && winningRoll != 100 && winningRoll != 69)
                        {
                            SoundPlayer player = new SoundPlayer(Properties.Resources.wow_c_101soundboards);
                          
                            player.Load();
                            player.Play();
                        }
                    }
                    else
                    {
                        if (MainWindow.playSoundEnabled && userRoll != 1 && winningRoll != 100 && winningRoll != 69)
                        {
                            SoundBoard("win");
                        }
                    }
                }
                else 
                {
                    rollWindow.UpdateUserRollData(userRoll.ToString(), 3);
                    rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"{winningRollUser.Username} wins!!!", 1);
                    if (MainWindow.playSoundEnabled && userRoll != 1 && winningRoll != 100 && winningRoll != 69)
                    {
                        SoundBoard("lose");
                    }

                }

                mainWindow.SendMessageToConsole($"[Roll]: {winningRollUser.Username} wins with a roll of {winningRoll.ToString()}.",7);
                if(userRoll==(winningRoll-5) || userRoll == (winningRoll - 4) || userRoll == (winningRoll - 3) || userRoll == (winningRoll - 2) || userRoll == (winningRoll - 1) && groupMembers.Count>=2)
                {
                    if (MainWindow.playSoundEnabled)
                    {
                        //play emotional damage
                        SoundPlayer player = new SoundPlayer(Properties.Resources.emotional_damage);
                        
                        player.Load();
                        player.Play();

                        //SoundBoard("emo");
                    }

                }
                winningRollUser = new User();
                currentRoll.Clear();
                tiedRollUsers.Clear();
                winningRoll = 0;
                tiedRoll = 0;
                hasRolled = false;
                rollStarted = false;
                Application.Current.Dispatcher.Invoke(() => rollWindow.btn_VoteDone_White.Visibility = Visibility.Visible);
                Application.Current.Dispatcher.Invoke(() => rollWindow.btn_VoteDone_Red.Visibility = Visibility.Hidden);
                SaveUserStats();
            }
        }
        public void ResetRollonDisconnect(string username)
        {
            if (rollStarted)
            {
                CancelRoll();
                mainWindow.SendMessageToConsole($"[Roll] Roll canceled {username} Disconnected.", 1);
                rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"Roll Canceled!!!", 1);
            }

        }
        public void ResetRollonUserJoin(string username)
        {
            if (rollStarted)
            {
                 CancelRoll();
                mainWindow.SendMessageToConsole($"[Roll] Roll canceled {username} Joined.",1);
                rollWindow.UpdateWinningRollData(winningRoll.ToString(), $"Roll Canceled!!!", 1);
            }

            }
        public void ResetNeedGreedRollonUserJoin(string username)
        {
            if (needGreedRollStarted)
            {
                CancelNeedGreedRoll();
                mainWindow.SendMessageToConsole($"[N>G]: Roll canceled {username} Joined.",1);
                rollWindow.UpdateWinningRollData("", $"Roll Canceled!!!", 1);
            }

        }
        public void ResetNeedGreedRollonUserDisconnect(string username)
        {
            if (needGreedRollStarted)
            {
                CancelNeedGreedRoll();
                mainWindow.SendMessageToConsole($"[N>G]: Roll canceled {username} Disconnected.",1);
                rollWindow.UpdateWinningRollData("", $"Roll Canceled!!!", 1);
            }

        }
        public void CancelRoll()
        {
            if (rollStarted)
            {
                winningRollUser = new User();
                currentRoll.Clear();
                winningRoll = 0;
                tiedRoll = 0;
                tiedRollUsers.Clear();
                hasRolled = false;
                rollStarted = false;
               Application.Current.Dispatcher.Invoke(() => rollWindow.btn_VoteDone_White.Visibility = Visibility.Visible);
                Application.Current.Dispatcher.Invoke(() => rollWindow.btn_VoteDone_Red.Visibility = Visibility.Hidden);
            
                //vote to cancel things needing reset
                rollToEndVoteDone = true;
                endRollVotes.Clear();
                votesForEndingRoll = 0;
                userHasVotedToEndRoll = false;
            }
        }
        public static void SendRollRequestToServer()
        {
            var rollPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}   ";
            message = message.Replace("\0", "");
            rollPacket.WriteOpCode(6);
            rollPacket.WriteMessage(message);
            _server.client.Client.Send(rollPacket.GetPacketBytes());
            Console.WriteLine($" Roll Request sent to server.");


        }
        public static User GetUserbyUID(string uid)
        {
            User user = new User();
            // Console.WriteLine($"Lookign for user with UID {uid}.");
            for (int i = 0; i < groupMembers.Count; i++)
            {
                if (groupMembers[i].UID == uid)
                {
                    user = groupMembers[i];
                    //Console.WriteLine($"User Found");
                    return user;
                }
            }

            return user;
        }
        public static string GetUsernamebyUID(string uid)
        {
            User user = new User();
            Console.WriteLine($"Lookign for user with UID {uid}.");
            for (int i = 0; i < groupMembers.Count; i++)
            {
                if (groupMembers[i].UID == uid)
                {
                    user = groupMembers[i];
                    string name = groupMembers[i].Username;
                    Console.WriteLine($"User Found");
                    return name;
                }
            }

            return null;
        }
        private void UserDisconnected()
        {
            var uid = _server.packetReader.ReadMessage();
            var user = groupMembers.Where(x => x.UID.ToString() == uid).FirstOrDefault();

            this.Dispatcher.Invoke(() => groupMembers.Remove(user));
            Console.WriteLine($"The Group now has {groupMembers.Count} members.");


            if (MainWindow.playSoundEnabled)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.door_shuts);
                //SoundPlayer player = new SoundPlayer(@"Mapleroll\Connect\door_shuts.wav");
               
                player.Load();
                player.Play();
            }

            mainWindow.BuildMembersListView();
            ResetRollonDisconnect(user.Username);
            ResetNeedGreedRollonUserDisconnect(user.Username);
            mainWindow.SendMessageToConsole($"{user.Username} has left the group.",1);
        }
        public void UserConnected()
        {
            //Console.WriteLine("User Connected Invoke Recieved");
            var uid = _server.packetReader.ReadMessage();
            Console.WriteLine($"User Connected UID: {uid}");
            var _groupID = _server.packetReader.ReadMessage();
            Console.WriteLine($"User Connected GroupID: {_groupID}");
            var username = _server.packetReader.ReadMessage();
            Console.WriteLine($"User Connected Username: {username}");
            if (!UserExistsInGroup(uid))
            {
                var user = new User();
                // Console.WriteLine("New User Created");
                user.UID = uid;
                var groupID = _groupID;
                user.Username = username;



                this.Dispatcher.Invoke(() => groupMembers.Add(user));


                this.Dispatcher.Invoke(() =>
                {
                    {

                        ListViewItem item = new ListViewItem();
                        //ContextMenu menu = (ContextMenu)this.FindResource("ItemContextMenu");

                        item.FontWeight = FontWeights.Bold;

                        item.Visibility = Visibility.Visible;
                        item.Width = 90;

                        item.Name = user.Username;
                        item.Content = user.Username;
                       
                        item.VerticalContentAlignment = VerticalAlignment.Center;
                        item.HorizontalContentAlignment = HorizontalAlignment.Center;
                        item.Background = Brushes.Transparent;
                        item.BorderBrush = Brushes.White;
                        item.BorderThickness = new Thickness(.5, .5, .5, .5);
                        item.Foreground = new SolidColorBrush(Colors.White);
                        item.FontSize = 9;
                        item.IsTabStop = false;
                        item.Uid = user.UID;
                        item.Focusable = true;
                        item.ContextMenu = mainWindow.lst_GroupMembers.Resources["MembersContextMenu"] as ContextMenu;
                        //item.MouseMove += mainWindow.Window_MouseMove;
                        item.MouseRightButtonUp += mainWindow.ContextMenuOpening;
                        Console.WriteLine($"Textbox parrent  {item.Parent}");

                        mainWindow.lst_GroupMembers.Items.Add(item);
                    }
                });









                //this.Dispatcher.Invoke(() => mainWindow.lst_GroupMembers.Items.Add(new TextBox {
                //    FontWeight = FontWeights.Bold,

                //    Visibility = Visibility.Visible,
                //    Width = 90,
                //    Name = user.Username,
                //    Text = user.Username,
                //    TextWrapping = TextWrapping.NoWrap,
                //    VerticalContentAlignment = VerticalAlignment.Center,
                //    HorizontalContentAlignment = HorizontalAlignment.Center,
                //    Background = this.Background,
                //    Foreground = new SolidColorBrush(Colors.White),
                //    FontSize = 12,
                //    IsTabStop = false,
                //    IsReadOnly = true
                //})); ;




                if (user.Username != userName)
                {
                    mainWindow.SendMessageToConsole($"{user.Username} joined the group.",1);
                    if (MainWindow.playSoundEnabled)
                    {
                        SoundPlayer player = new SoundPlayer(Properties.Resources.knocking_on_door);

                        player.Load();
                        player.Play();
                    }
                }


                ResetRollonUserJoin(user.Username);
                ResetNeedGreedRollonUserJoin(user.Username);

            }
        }

        public static void SendVoteToEndRollRequestToServer()
        {
            var rollPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}   ";
            message = message.Replace("\0", "");
            rollPacket.WriteOpCode(12);
            rollPacket.WriteMessage(message);
            _server.client.Client.Send(rollPacket.GetPacketBytes());
            Console.WriteLine($" Vote to end Roll Request sent to server.");


        }
        public void VoteToEndRollRecieved()
        {
                //Console.WriteLine("User Connected Invoke Recieved");
                var uid = _server.packetReader.ReadMessage();
                Console.WriteLine($"User Voted To end Vote UID: {uid}");
                var _groupID = _server.packetReader.ReadMessage();
                Console.WriteLine($"User Voted GroupID: {_groupID}");
            if (rollStarted)
            {
                Console.WriteLine($"Checking if user has voted to cancel.");
                if (!UserVotedToEndRollAlready(uid))
                {
                    endRollVotes.Add(uid);
                    votesForEndingRoll++;
                    Console.WriteLine($"Adding Vote to cancel.");
                }

                double percent = .6;
                decimal grouppercent = MapleRollConnect.groupMembers.Count * (int)percent;
                int voteSucceedPoint = (int)Math.Floor(grouppercent);
                Console.WriteLine($"Current Votes To cancel are {votesForEndingRoll}  votes needed are {voteSucceedPoint}.");
                if (votesForEndingRoll >= voteSucceedPoint)
                {
                    Console.WriteLine($"Vote to cancel passed.");
                    CancelRoll();
                    rollWindow.UpdateWinningRollData("--", $"Roll Canceled!", 1);
                    rollWindow.UpdateUserRollData("--", 1);
                    mainWindow.SendMessageToConsole($"[Roll] The vote to cancel the Roll has PASSED!", 1);
                    rollToEndVoteDone = true;
                    endRollVotes.Clear();
                    votesForEndingRoll = 0;
                    userHasVotedToEndRoll = false;
                }

            }
            
         }
        public bool UserVotedToEndRollAlready(string uid)
        {

            foreach (var item in endRollVotes)
            {
                if(item==uid)
                {

                    Console.WriteLine($"User Voted to cancel already.");
                    return true;
                }


            }

            Console.WriteLine($"User has not voted to cancel already.");
            return false;
        }



        public static void SendVoteToEndNeedGreedRollRequestToServer()
        {
            var rollPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}   ";
            message = message.Replace("\0", "");
            rollPacket.WriteOpCode(18);
            rollPacket.WriteMessage(message);
            _server.client.Client.Send(rollPacket.GetPacketBytes());
            Console.WriteLine($" Vote to end Need Greed Roll Request sent to server.");


        }
        public void VoteToEndNeedGreedRollRecieved()
        {
            //Console.WriteLine("User Connected Invoke Recieved");
            var uid = _server.packetReader.ReadMessage();
            Console.WriteLine($"User Voted To end N>G Vote UID: {uid}");
            var _groupID = _server.packetReader.ReadMessage();
            Console.WriteLine($"User Voted N>G GroupID: {_groupID}");
            if (needGreedRollStarted)
            {
                Console.WriteLine($"Checking if user has voted to cancel.");
                if (!UserVotedToEndNeedGreedRollAlready(uid))
                {
                    endNeedGreedRollVotes.Add(uid);
                    votesForEndingNeedGreedRoll++;
                    Console.WriteLine($"Adding Vote to cancel.");
                }

                double percent = .6;
                decimal grouppercent = MapleRollConnect.groupMembers.Count * (int)percent;
                int voteSucceedPoint = (int)Math.Floor(grouppercent);
                Console.WriteLine($"Current Votes To cancel are {votesForEndingNeedGreedRoll}  votes needed are {voteSucceedPoint}.");
                if (votesForEndingNeedGreedRoll >= voteSucceedPoint)
                {
                    Console.WriteLine($"Vote to cancel passed.");
                    CancelRoll();
                    needGreedWindow.UpdateWinningRollData("--", $"Roll Canceled!.", 1, "Need");
                    needGreedWindow.UpdateWinningRollData("--", $"Roll Canceled!.", 1, "Greed");
                    needGreedWindow.UpdateUserRollData("--", 1);
                    mainWindow.SendMessageToConsole($"[N>G]: The vote to cancel the Roll has PASSED!", 1);

                    rollToEndNeedGreedVoteDone = true;
                    endNeedGreedRollVotes.Clear();
                    votesForEndingNeedGreedRoll = 0;
                    userHasVotedToEndNeedGreedRoll = false;
                }

            }
        }
        public bool UserVotedToEndNeedGreedRollAlready(string uid)
        {

            foreach (var item in endNeedGreedRollVotes)
            {
                if (item == uid)
                {

                    Console.WriteLine($"User Voted to cancel already.");
                    return true;
                }


            }

            Console.WriteLine($"User has not voted to cancel already.");
            return false;
        }




        
        public bool UserExistsInGroup(string uid)
        {
            for (int i = 0; i < groupMembers.Count; i++)
            {
                if (groupMembers[i].UID==uid)
                {

                    return true;
                }
            }
            return false;
        }
        public void ConnectionSuccessful()
        {

            LoadUserStats();
            Console.WriteLine("Connection Successful");

                MainWindow.connectWindow = this;
                rollWindow = MainWindow.rollWindow;
                needGreedWindow=MainWindow.needGreedWindow;
                string temp = userName.ToUpper();
                this.Dispatcher.Invoke((() => {mainWindow.Show();}));;
                mainWindow.SendMessageToConsole($"May RNGsus be with you {temp}!",1);
                mainWindow.UpdateConnectionInfo( uId, temp, groupId);
                MainWindow.server = _server;
                this.Dispatcher.Invoke((() =>{this.Hide();}));
            


        }

        public void MigrateUser()
        {

            mainWindow.UpdateConnectionInfo(uId, userName.ToUpper(), groupId);


        }
        public void ConnectionError()
        {

            connectionError.Show();

        }
        public static void MessageReceived(string msg, int color)
        {
            mainWindow.SendMessageToConsole(msg, color);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        public static void SoundBoard(string sound)
        {
            switch (sound)
            {

                case "win":
                    switch (userSoundProfile)
                    {
                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.oh_yeah_Default);
                            player1.Load();
                            player1.Play();
                            break;

                            //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.coin_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.yeah_bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.ha_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.item_Zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.holy_moly_southpark);
                            player6.Load();
                            player6.Play();
                            break;



                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.mario_yahoo);
                            _player.Load();
                            _player.Play();

                            break;
                    }
                    break;
               
                case "lose":
                    switch (userSoundProfile)
                    {                         //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.oh_man_deafult);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.bro_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.pass_Bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.ha_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.low_hp_Zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.and_its_gone_Southpark);
                            player6.Load();
                            player6.Play();
                            break;



                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.mario_yahoo);
                            _player.Load();
                            _player.Play();

                            break;
                    }


                    break;

                case "you100":
                    switch (userSoundProfile)
                    {
                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.yer_a_wizard_harry);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.its_mario_time);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.oh_my_god_Bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.jackpot_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.chest1_zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.yeah_yes_south);
                            player6.Load();
                            player6.Play();
                            break;



                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.yer_a_wizard_harry);
                            _player.Load();
                            _player.Play();

                            break;
                    }


                    break;

                case "1":
                    switch (userSoundProfile)
                    {                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.dissapointment);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.oh_uh_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.ah_crap_Bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.shut_up_meg_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.link_fall_zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.geez_thats_terrible_south_park);
                            player6.Load();
                            player6.Play();
                            break;



                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.dissapointment);
                            _player.Load();
                            _player.Play();

                            break;
                    }


                    break;

                case "69":
                    switch (userSoundProfile)
                    {
                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.niiice);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.mama_mia_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.whoa_mama_Bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.bow_chicka_wow_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.bounce_Zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.nice_Southpark);
                            player6.Load();
                            player6.Play();
                            break;


                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.niiice);
                            _player.Load();
                            _player.Play();

                            break;
                    }

                    break;

                case "they100":
                    switch (userSoundProfile)
                    {
                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.harry_potter_theme);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.star_theme_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.futurama_theme_bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.congratulations_Familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.prayer_zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.intro_Southpark);
                            player6.Load();
                            player6.Play();
                            break;

                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.harry_potter_theme);
                            _player.Load();
                            _player.Play();

                            break;
                    }

                    break;

                case "Emo":
                    switch (userSoundProfile)
                    {
                        //Defaut 
                        case 1:
                            SoundPlayer player1 = new SoundPlayer(Properties.Resources.emotional_damage);
                            player1.Load();
                            player1.Play();
                            break;

                        //mario
                        case 2:
                            SoundPlayer player2 = new SoundPlayer(Properties.Resources.wahhhhh_mario);
                            player2.Load();
                            player2.Play();
                            break;

                        // Bender
                        case 3:
                            SoundPlayer player3 = new SoundPlayer(Properties.Resources.you_bastard_bender);
                            player3.Load();
                            player3.Play();
                            break;

                        // Family Guy
                        case 4:
                            SoundPlayer player4 = new SoundPlayer(Properties.Resources.almost_familyguy);
                            player4.Load();
                            player4.Play();
                            break;

                        // Zelda
                        case 5:
                            SoundPlayer player5 = new SoundPlayer(Properties.Resources.link_death_zelda);
                            player5.Load();
                            player5.Play();
                            break;

                        // Southpark
                        case 6:
                            SoundPlayer player6 = new SoundPlayer(Properties.Resources.omg_no_way_Southpark);
                            player6.Load();
                            player6.Play();
                            break;

                        default:
                            SoundPlayer _player = new SoundPlayer(Properties.Resources.harry_potter_theme);
                            _player.Load();
                            _player.Play();

                            break;
                    }

                    break;

                case "join":
                   
                            SoundPlayer player7 = new SoundPlayer(Properties.Resources.knocking_on_door);
                            player7.Load();
                            player7.Play();
                          

                    break;

                case "leave":
                    
                   
                      SoundPlayer player8 = new SoundPlayer(Properties.Resources.door_shuts);
                      player8.Load();
                      player8.Play();
                       
                    break;

                case "roll":


                    SoundPlayer player9 = new SoundPlayer(Properties.Resources.roll_default);
                    player9.Load();
                    player9.Play();

                    break;
            }
        }

        public static void CancelNeedGreedRoll()
        {
            if(needGreedRollStarted)
            {

                winningNeedGreedUser = new User();
                winningGreedUser = new User();
                winningNeedUser = new User();
                winningNeedRoll = 0;
                winningGreedRoll = 0;
                winningNeedGreedRoll = 0;
                tiedGreedRoll = 0;
                tiedNeedRoll = 0;
                userNeedGreedRoll = 0;


                needRolls.Clear();
                greedRolls.Clear();

                isNeedRoll = false;
                isGreedRoll = false;
                someoneRolledNeed = false;
                hasNeedGreedRolled = false;
                needGreedRollStarted = false;

                Application.Current.Dispatcher.Invoke(() => needGreedWindow.btn_VoteDone_White.Visibility = Visibility.Visible);
                Application.Current.Dispatcher.Invoke(() => needGreedWindow.btn_VoteDone_Red.Visibility = Visibility.Hidden);

            }


        }

        public static void ProcessNeedGreedRoll()
        {
            var _rollType = _server.packetReader.ReadMessage();
            var groupId=_server.packetReader.ReadMessage();
            var uid = _server.packetReader.ReadMessage();
            var _roll = _server.packetReader.ReadMessage();
            int roll = int.Parse(_roll);
            int rollType = int.Parse(_rollType);

            User user = GetUserbyUID(uid);
            string username = GetUsernamebyUID(uid);
            if (!needGreedRollStarted)
            {
                needGreedWindow.UpdateUserRollData("".ToString(), 1);
                needGreedRollStarted = true;
            }
            if (rollType==1)
            {
                isNeedRoll=true;
                isGreedRoll = false;
                if(!someoneRolledNeed)
                {
                    winningNeedGreedRoll = 0;
                    winningNeedGreedUser= new User();
                    winningGreedRoll = 0;
                    winningGreedUser = new User();
                }
                someoneRolledNeed = true;
                Application.Current.Dispatcher.Invoke((() => needGreedWindow.lbl_WinningNeedTitle.Visibility= Visibility.Visible ));
                Application.Current.Dispatcher.Invoke((() => needGreedWindow.lbl_WinningGreedTitle.Visibility = Visibility.Hidden));
                
            }
            else if (rollType==2)
            {

                isGreedRoll=true;
                isNeedRoll = false;
                if(!someoneRolledNeed)
                {
                Application.Current.Dispatcher.Invoke((() => needGreedWindow.lbl_WinningNeedTitle.Visibility = Visibility.Hidden));
                Application.Current.Dispatcher.Invoke((() => needGreedWindow.lbl_WinningGreedTitle.Visibility = Visibility.Visible));
                }
            }

            if (uid == uId)
            {
                userNeedGreedRoll = roll;
                if (groupMembers.Count >= 2)
                {
                    if (roll == 100)
                    {
                        totalPerfectRollCount++; 
                    }
                    else if (roll == 1)
                    {
                        totalOneRolls++;
                        if (MainWindow.playSoundEnabled)
                        {
                            SoundBoard("1");
                        }
                    }

                    totalUserRollCount++;
                    userTotalRollSum += roll;
                }
                if (roll == 69)
                {

                    if (MainWindow.playSoundEnabled)
                    {
                        SoundBoard("69");
                    }
                }

                needGreedWindow.UpdateUserRollData(roll.ToString(), 1);
            }

            //need roll
            if (isNeedRoll)
            {
                needRolls.Add(user, roll);

                if (winningNeedGreedRoll != 0 && winningNeedGreedRoll == roll)
                {
                    tiedNeedRoll = roll;

                    winningNeedUser = new User();
                    winningNeedGreedUser = new User();
                    needGreedWindow.UpdateWinningRollData(tiedNeedRoll.ToString(), $"There is a tie.", 1, "Need");
                }

                if (roll>winningNeedGreedRoll && winningNeedGreedRoll !=roll)
                {
                    winningNeedGreedRoll = roll;
                    winningNeedGreedUser = user;

                   needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"{winningNeedGreedUser.Username} is winning.", 1, "Need");
                }
            }

            //greed roll
            if (isGreedRoll )
            {
                greedRolls.Add(user, roll);

                if (winningNeedGreedRoll != 0 && winningNeedGreedRoll == roll && !someoneRolledNeed)
                {
                    tiedGreedRoll = roll;
                    winningNeedGreedUser = new User();
                   

                    needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"{winningNeedGreedUser.Username} is Tied.", 1, "Greed");
                }
                    


                if (roll > winningNeedGreedRoll && winningNeedGreedRoll != roll && !someoneRolledNeed)
                {
                    winningNeedGreedRoll = roll;
                    
                    winningNeedGreedUser = user;

                    if (!someoneRolledNeed)
                    {
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"{winningNeedGreedUser.Username} is winning.", 1, "Greed");
                    }
                    
                }
            }

            if (roll == 100)
            {
                if (uid != uId)
                {
                    // they win
                    if (MainWindow.playSoundEnabled)
                    {
                        SoundBoard("they100");
                    }
                    mainWindow.SendMessageToConsole($"[N>G]: {username} is a Wizzard! They rolled a {roll.ToString()}.",1);
                }
                else
                {
                    if (MainWindow.playSoundEnabled)
                    {
                        SoundBoard("you100");
                    }
                    mainWindow.SendMessageToConsole($"[N>G]: You're a Wizzard! You rolled a {roll.ToString()}.",1);
                }
            }
            else
            {
                if (isGreedRoll)
                {
                    mainWindow.SendMessageToConsole($"[N>G]: {username} Greed rolls a {roll.ToString()}.",15);
                }
                else if(isNeedRoll)
                {
                    mainWindow.SendMessageToConsole($"[N>G]: {username} Need rolls a {roll.ToString()}.",15);
                }
            }

            //all rolls in calculate winner
            if (needRolls.Count + greedRolls.Count >= groupMembers.Count)
            {
                //if tied
                if (tiedNeedRoll != 0 || tiedGreedRoll != 0)
                {
                    needGreedWindow.UpdateUserRollData(userNeedGreedRoll.ToString(), 2);
                    if (isGreedRoll && !someoneRolledNeed)
                    {
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"You have tied!!!", 2, "Greed");

                    }
                    else if (isNeedRoll)
                    {
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"There is a  tied!!!", 2, "Need");

                    }

                }
                //if we win and Tied is Zero (No Tie)
                else if (winningNeedGreedUser.UID == uId && tiedNeedRoll == 0 && tiedGreedRoll == 0)
                {
                    if (groupMembers.Count >= 2)
                    {
                        totalWinningRolls++;
                    }
                    //emo damage check
                    if (userNeedGreedRoll == (winningNeedGreedRoll - 5) || userNeedGreedRoll == (winningNeedGreedRoll - 4) || userNeedGreedRoll == (winningNeedGreedRoll - 3) || userNeedGreedRoll == (winningNeedGreedRoll - 2) || userNeedGreedRoll == (winningNeedGreedRoll - 1) && groupMembers.Count >= 2)
                    {
                        if (MainWindow.playSoundEnabled)
                        {
                            SoundBoard("emo");
                        }

                    }
                    needGreedWindow.UpdateUserRollData(userNeedGreedRoll.ToString(), 2);

                    if (!someoneRolledNeed)
                    {
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"You win!!!", 2, "Greed");
                        mainWindow.SendMessageToConsole($"[N>G]: {winningNeedGreedUser.Username} wins with a Greed roll of {winningNeedGreedRoll.ToString()}.", 7);
                    }
                    else if (someoneRolledNeed)
                    {

                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"You win!!!", 2, "Need");
                        mainWindow.SendMessageToConsole($"[N>G]: {winningNeedGreedUser.Username} wins with a Need roll of {winningNeedGreedRoll.ToString()}.", 7);
                    }

                    //if the user is sticky play special sound 
                    if (userName == "STICKY")
                    {
                        if (MainWindow.playSoundEnabled && winningNeedGreedRoll != 1 && winningNeedGreedRoll != 100)
                        {
                            SoundPlayer player = new SoundPlayer(Properties.Resources.wow_c_101soundboards);

                            player.Load();
                            player.Play();
                        }
                    }
                    //else  play use win sound.
                    else
                    {
                        if (MainWindow.playSoundEnabled && winningNeedGreedRoll != 1 && winningNeedGreedRoll != 100)
                        {
                            SoundBoard("win");
                        }
                    }
                }
                //we lose
                else
                {


                    if (!someoneRolledNeed)
                    {
                        needGreedWindow.UpdateUserRollData(userNeedGreedRoll.ToString(), 3);
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"{winningNeedGreedUser.Username} wins!!!", 1, "Greed");
                        mainWindow.SendMessageToConsole($"[N>G]: {winningNeedGreedUser.Username} wins with a Greed roll of {winningNeedGreedRoll.ToString()}.",7);
                    }
                    else if (someoneRolledNeed)
                    {

                        needGreedWindow.UpdateUserRollData(userNeedGreedRoll.ToString(), 3);
                        needGreedWindow.UpdateWinningRollData(winningNeedGreedRoll.ToString(), $"{winningNeedGreedUser.Username} wins!!!", 1, "Need");
                        mainWindow.SendMessageToConsole($"[N>G]: {winningNeedGreedUser.Username} wins with a Need roll of {winningNeedGreedRoll.ToString()}.",7);


                    }

                    SoundBoard("lose");
                }

                ////emo damage check
                //if (userNeedGreedRoll == (winningNeedGreedRoll - 5) || userNeedGreedRoll == (winningNeedGreedRoll - 4) || userNeedGreedRoll == (winningNeedGreedRoll - 3) || userNeedGreedRoll == (winningNeedGreedRoll - 2) || userNeedGreedRoll == (winningNeedGreedRoll - 1) && groupMembers.Count >= 2)
                //{
                //    if (MainWindow.playSoundEnabled)
                //    {
                //        SoundBoard("emo");
                //    }

                //}

                //reset all
                winningNeedGreedUser = new User();
                winningGreedUser = new User();
                winningNeedUser = new User();
                winningNeedRoll = 0;
                winningGreedRoll = 0;
                winningNeedGreedRoll = 0;
                tiedGreedRoll = 0;
                tiedNeedRoll = 0;
                userNeedGreedRoll = 0;


                needRolls.Clear();
                greedRolls.Clear();

                isNeedRoll = false;
                isGreedRoll = false;
                someoneRolledNeed = false;
                hasNeedGreedRolled = false;
                needGreedRollStarted = false;
                Application.Current.Dispatcher.Invoke(() => SaveUserStats());
                
            }
        }

        public static void SendVoteToKickToServer(string uid)
        {
           
            var kickPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}:{uid}   ";
            message = message.Replace("\0", "");
            kickPacket.WriteOpCode(24);
            kickPacket.WriteMessage(message);
            _server.client.Client.Send(kickPacket.GetPacketBytes());

            User user= GetUserbyUID(uid);
            mainWindow.SendMessageToConsole($"You have voted to kick {user.Username} from the group.",1);

        }
        public static void SendGreedRollToServer()
        {

            var rollPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}   ";
            message = message.Replace("\0", "");
            rollPacket.WriteOpCode(13);
            rollPacket.WriteMessage(message);
            _server.client.Client.Send(rollPacket.GetPacketBytes());
            //Console.WriteLine($" Greed Roll Request sent to server.");
        }

        public static void SendNeedRollToServer()
        {
            var rollPacket = new PacketBuilder();
            string message = $"{groupId}:{uId}   ";
            message = message.Replace("\0", "");
            rollPacket.WriteOpCode(14);
            rollPacket.WriteMessage(message);
            _server.client.Client.Send(rollPacket.GetPacketBytes());
            //Console.WriteLine($" Need Roll Request sent to server.");

        }

        public static void SystemMessageRecieved(string msg)
        {
            
            mainWindow.SendMessageToConsole(msg, 20);
        }

        private void txb_UserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            btn_ConnectNew_Click(sender, e);
        }

        private void txb_GroupId_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            btn_Connect_Click(sender, e);
        }
    }
}
