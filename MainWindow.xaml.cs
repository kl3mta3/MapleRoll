using MapleRoll.Coin;
using MapleRoll.Connect;
using MapleRoll.Greed;
using MapleRoll.Net;
using MapleRoll.Roll;
using MapleRoll.RPS;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
//using static System.Net.Mime.MediaTypeNames;



namespace MapleRoll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static ObservableCollection<string> Messages { get; set; }
        public static ObservableCollection<string> Messages { get; set; }
        public static ObservableCollection<User> GroupMembers { get; set; }
        //Connect Data
        internal static bool isConnectedToServer = false;
        internal bool isPrivateMessage = false;
        internal string userName { get; set; }
        internal string UID { get; set; }
        internal string groupId { get; set; }
        internal static Server server { get; set; }
        //rollWindow data
        public int rollWindowUserRoll;
        public int rollWindowWinningRoll;
        public string rollWindowWinnerName;

        public bool rollRadialChecked = false;
        public bool rollWindowCanRoll = false;
        public bool rollWindowIsOpen = false;

        public bool connectWindowIsOpen = false;
        public bool coinFlipWindowIsOpen = false;
        public bool coinRadialChecked = false;

        public bool rpsWindowIsOpen = false;
        public static bool playSoundEnabled = true;
        public bool soundIsChecked = true;
        public bool rpsRadialChecked = false;

        public string privateMessageReceiverUid = "";

        public bool greedWindowIsOpen = false;
        public bool greedRadialChecked = false;

        internal static RollWindow rollWindow { get; set; }
        internal static RPSWindow rpsWindow { get; set; }
        internal static MapleRollConnect connectWindow { get; set; }
        internal static CoinFlipWindow coinFlipWindow { get; set; }
        internal static SoundProfileWindow soundProfileWindow { get; set; }
        internal static NeedGreedWindow needGreedWindow { get; set; }
        internal static ContextWindow context { get; set; }
        internal static Point mouseLocation { get; set; }
        internal bool soundWindowOpen = false;
        internal bool contextOpen = false;
        public MainWindow()
        {

            try
            {
                InitializeComponent();
                MapleRollConnect.mainWindow = this;
                coinFlipWindow = new CoinFlipWindow();
                rollWindow = new RollWindow();
                rpsWindow = new RPSWindow();
                needGreedWindow = new NeedGreedWindow();
                soundProfileWindow = new SoundProfileWindow();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SendMessageToConsole(string message, int color)
        {

            switch (color)
            {
                //System
                case 1:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.Salmon);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff0055");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                //Winning

                case 7:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.GreenYellow);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#40ff00");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                //Message 
                case 5:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ddff");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                //Roll
                case 6:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.MediumVioletRed);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff00ff");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                //needGreed
                case 15:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.MediumOrchid);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffa200");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));

                    break;

                //RPS
                case 17:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.Yellow);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#9000ff");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;
                //Flip
                case 16:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.Moccasin);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#005eff");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                case 20:
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.Moccasin);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffe600");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;

                case 25: //private message
                    this.Dispatcher.Invoke((() =>
                    {
                        Run run = new Run(message);
                        //run.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
                        run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ff91");
                        rtb_Console.Document.Blocks.Add(new Paragraph(run));
                        rtb_Console.ScrollToEnd();
                    }));
                    break;
            }


            Console.WriteLine("Completed Message sent to Text box");
        }


        public void UpdateConnectionInfo(string uid, string _user, string _groupId)
        {

            userName = _user.ToUpper();
            groupId = _groupId;
            UID = uid;
            this.Dispatcher.Invoke((() =>
            {
                lbl_UserName.Content = userName;
                lbl_GroupID.Content = groupId;
                lbl_ClientId.Content = UID;
            }));

        }

        private void btn_SendChat_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txb_ChatInput.Text))
            {
                if (!isPrivateMessage)
                {
                    string message = txb_ChatInput.Text;
                    server.SendMessageToServerGroup(groupId, message);
                    txb_ChatInput.Clear();
                }
                else if (isPrivateMessage)
                {
                    string msg = txb_ChatInput.Text;
                    var splitmessage = msg.Split(':', 2);

                    string privateTag = splitmessage[0].ToUpper();
                    string message = splitmessage[1];

                    server.SendPrivateMessageToServerGroup(groupId, privateMessageReceiverUid, message);
                    txb_ChatInput.Clear();
                    isPrivateMessage = false;
                    
                    string consoleMsg = $"[{privateTag}]: {message}";
                    SendMessageToConsole(consoleMsg,25);
                    privateMessageReceiverUid = "";
                    txb_ChatInput.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                }
            }

        }

        public void BuildMembersListView()
        {
            this.Dispatcher.Invoke(() => lst_GroupMembers.Items.Clear());
            foreach (var user in MapleRollConnect.groupMembers)
            {

                this.Dispatcher.Invoke(() =>
                {
                    {

                        ListViewItem item = new ListViewItem();
                        //ContextMenu menu = (ContextMenu)this.FindResource("ItemContextMenu");

                        item.FontWeight = FontWeights.Bold;

                        item.Visibility = Visibility.Visible;
                        item.Width = 90;
                        //textBox.Height = 25;
                        item.Name = user.Username;
                        item.Content = user.Username;
                        //textBox.TextWrapping = TextWrapping.NoWrap;
                        item.VerticalContentAlignment = VerticalAlignment.Center;
                        item.HorizontalContentAlignment = HorizontalAlignment.Center;
                        item.Background =  Brushes.Transparent;
                        item.BorderBrush= Brushes.White;
                        item.BorderThickness = new Thickness(.5,.5,.5,.5);
                        item.Foreground = new SolidColorBrush(Colors.White);
                        item.FontSize = 9;
                        item.IsTabStop = false;
                        //textBox.IsReadOnly = true;
                        item.Uid = user.UID;
                        item.ContextMenu = lst_GroupMembers.Resources["MembersContextMenu"] as ContextMenu;
                        
                        //item.MouseMove += Window_MouseMove;
                        item.Focusable = true;


                        item.MouseRightButtonUp += ContextMenuOpening;
                        lst_GroupMembers.Items.Add(item);
                    }
                });

            }

        }



        public void kickOnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Kick clicked userToKick:");
            this.Dispatcher.Invoke(() =>
            {
                {

                    ListViewItem item = new ListViewItem();

                    ContextMenu contextMenu = sender as ContextMenu;

                    if (contextMenu != null)
                    {
                        item = contextMenu.PlacementTarget as ListViewItem;
                        if (item != null)
                        {
                            // Attach the ListViewItem to the context menu as a Tag
                            contextMenu.Tag = item;

                            string userToKick = item.Uid;
                            //MapleRollConnect.SendVoteToKickToServer(userToKick);
                            Console.WriteLine($" userToKick: {userToKick}");
                        }
                    }


                }
            });
        }
        public void messageOnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"PrivateMessage Clicked");
            this.Dispatcher.Invoke(() =>
            {
                {
                    isPrivateMessage = true;
                    Console.WriteLine($"IsPrivateMessage");
                    ListViewItem item = new ListViewItem();
                    ContextMenu contextMenu = sender as ContextMenu;


                    if (contextMenu != null)
                    {
                        Console.WriteLine($"Context Menu not null");
                        item = contextMenu.PlacementTarget as ListViewItem;
                        if (item != null)
                        {
                            // Attach the ListViewItem to the context menu as a Tag
                            contextMenu.Tag = item;

                            Console.WriteLine($"Message clicked Send message.  ");
                            txb_ChatInput.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#e600ff");
                            txb_ChatInput.Text = $"@{item.Name}: ";
                            txb_ChatInput.Tag = txb_ChatInput.Text.Length.ToString();
                            txb_ChatInput.Focus();
                            txb_ChatInput.CaretIndex = int.Parse(txb_ChatInput.Tag.ToString());
                        }
                    }
                }
            });
        }


        public void kickOnClickList(ListViewItem item)
        {
            this.Dispatcher.Invoke(() =>
            {
                {

                    string userToKick = item.Uid;
                    MapleRollConnect.SendVoteToKickToServer(userToKick);
                    Console.WriteLine($"Kick clicked userToKick: {userToKick}");
                    context.Close();
                    contextOpen = false;
                }
            });
        }
        public void messageOnClickList(ListViewItem item)
        {
            this.Dispatcher.Invoke(() =>
            {
                {
                    isPrivateMessage = true;

                    privateMessageReceiverUid = item.Uid;

                    Console.WriteLine($"Message clicked Send message.  ");

                    txb_ChatInput.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#00ff91");
                    txb_ChatInput.Text = $"@{item.Name}: ";
                    txb_ChatInput.Tag = txb_ChatInput.Text.Length.ToString();
                    txb_ChatInput.Focus();
                    txb_ChatInput.CaretIndex = int.Parse(txb_ChatInput.Tag.ToString());
                    context.Close();
                    contextOpen = false;
                }
            });
        }


        public void ContextMenuOpening(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (context != null)
            {
                context.Close();
            }
            this.Dispatcher.Invoke(() =>
             {
                 {
                     ListViewItem item = sender as ListViewItem;
                     if (item != null)
                     {
                         Console.WriteLine($"RightClicked.{item.Name}");



                         ContextWindow contextWindow = new ContextWindow();

                         contextWindow.Owner = this;
                         contextWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                         contextWindow.mainWindow = this;
                         contextWindow.selectedUser = item;
                         //contextWindow.Focus();
                         context = contextWindow;
                         contextWindow.Show();
                         contextOpen=true;
                         //contextWindow.Deactivated += (sender, args) => { contextWindow.Hide(); };


                     }

                 }

             });

        }

        public void UpdateUserStatDisplay()
        {

            decimal averageRoll = new decimal();
            decimal winLoss = new decimal();

            decimal totalSum = MapleRollConnect.userTotalRollSum;
            decimal totalRolls = MapleRollConnect.totalUserRollCount;
            decimal winningRolls = MapleRollConnect.totalWinningRolls;

            if (totalSum > 0 && totalRolls > 0)
            {
                averageRoll = totalSum / totalRolls;
            }
            else
            {
                averageRoll = 0;
            }

            if (winningRolls > 0 && totalRolls > 0)
            {
                winLoss = winningRolls / totalRolls;

            }
            else

            {

                winLoss = 0;
            }





            Application.Current.Dispatcher.Invoke((() =>
            {
                lbl_AverageRoll.Content = averageRoll.ToString(".00");
                lbl_WinLoss.Content = winLoss.ToString(".00");
                lbl_100Roll.Content = MapleRollConnect.totalPerfectRollCount.ToString();
                lbl_1Roll.Content = MapleRollConnect.totalOneRolls.ToString();
                lbl_TotalRolls.Content = MapleRollConnect.totalUserRollCount.ToString();
            }));
        }

        private void rbn_Roll_Click(object sender, RoutedEventArgs e)
        {
            rollRadialChecked = !rollRadialChecked;
            try
            {

                if (rollRadialChecked)
                {
                    rbn_Roll.IsChecked = true;
                    rollWindowIsOpen = true;
                    rollWindow.Show();

                }
                else if (!rollRadialChecked)
                {
                    rbn_Roll.IsChecked = false;
                    rollWindowIsOpen = false;
                    rollWindow.Hide();
                    if (RollWindow.privateRollWindow.IsEnabled)
                    {
                        RollWindow.privateRollWindow.Hide();
                        rollWindow.privateRollwindowOpen = false;
                        this.Dispatcher.Invoke((() => rollWindow.rbt_PrivateRoll.IsChecked = false));
                    }


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        private void SignOutbtn_Click(object sender, RoutedEventArgs e)
        {
            if (rollWindow.IsEnabled)
            {
                if (RollWindow.privateRollWindow.IsEnabled)
                {
                    RollWindow.privateRollWindow.Close();
                    rollWindow.privateRollwindowOpen = false;
                    this.Dispatcher.Invoke((() => rollWindow.rbt_PrivateRoll.IsChecked = false));
                }
                rollWindow.Close();

            }
            MapleRollConnect.Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rollWindow.IsEnabled)
            {

                if (RollWindow.privateRollWindow.IsEnabled)
                {
                    RollWindow.privateRollWindow.Close();
                    rollWindow.privateRollwindowOpen = false;
                    this.Dispatcher.Invoke((() => rollWindow.rbt_PrivateRoll.IsChecked = false));
                }
                rollWindow.Close();

            }
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (contextOpen)
            {
                if (!context.mouseOver)
                {
                    context.Close();
                }
            }
            if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
        }

        private void rbn_Roll_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rollRadialChecked = !rollRadialChecked;
            try
            {
                if (rollRadialChecked)
                {
                    rbn_Roll.IsChecked = true;
                    rollWindowIsOpen = true;
                    rollWindow.Show();

                }
                else if (!rollRadialChecked)
                {
                    rbn_Roll.IsChecked = false;
                    rollWindowIsOpen = false;
                    rollWindow.Hide();
                    if (RollWindow.privateRollWindow.IsEnabled)
                    {
                        RollWindow.privateRollWindow.Hide();
                        rollWindow.privateRollwindowOpen = false;
                        this.Dispatcher.Invoke((() => rollWindow.rbt_PrivateRoll.IsChecked = false));
                    }


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        private void rbn_Coin_Click(object sender, RoutedEventArgs e)
        {
            coinRadialChecked = !coinRadialChecked;
            try
            {
                if (coinRadialChecked)
                {
                    rbn_Coin.IsChecked = true;
                    coinFlipWindowIsOpen = true;
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_FlipIcon.Visibility = Visibility.Visible));
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_HeadsIcon.Visibility = Visibility.Hidden));
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_TailsIcon.Visibility = Visibility.Hidden));
                    coinFlipWindow.Show();

                }
                else if (!coinRadialChecked)
                {
                    rbn_Coin.IsChecked = false;
                    coinFlipWindowIsOpen = false;
                    coinFlipWindow.Hide();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        private void rbn_Coin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            coinRadialChecked = !coinRadialChecked;
            try
            {
                if (coinRadialChecked)
                {
                    rbn_Coin.IsChecked = true;
                    coinFlipWindowIsOpen = true;
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_FlipIcon.Visibility = Visibility.Visible));
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_HeadsIcon.Visibility = Visibility.Hidden));
                    this.Dispatcher.Invoke((() => coinFlipWindow.stk_TailsIcon.Visibility = Visibility.Hidden));
                    coinFlipWindow.Show();

                }
                else if (!coinRadialChecked)
                {
                    rbn_Coin.IsChecked = false;
                    coinFlipWindowIsOpen = false;
                    coinFlipWindow.Hide();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        private void rbn_RPS_Checked(object sender, RoutedEventArgs e)
        {

            rpsRadialChecked = !rpsRadialChecked;
            try
            {

                if (rpsRadialChecked)
                {
                    rbn_RPS.IsChecked = true;
                    rpsWindowIsOpen = true;
                    rpsWindow.Show();

                }
                else if (!rpsRadialChecked)
                {
                    rbn_RPS.IsChecked = false;
                    rpsWindowIsOpen = false;
                    rpsWindow.ResetRPSWindow();
                    rpsWindow.Hide();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        private void rbn_RPS_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rpsRadialChecked = !rpsRadialChecked;
            try

            {
                if (rpsRadialChecked)
                {
                    rbn_RPS.IsChecked = true;
                    rpsWindowIsOpen = true;

                    rpsWindow.Show();

                }
                else if (!rpsRadialChecked)
                {
                    rbn_RPS.IsChecked = false;
                    rpsWindowIsOpen = false;
                    rpsWindow.ResetRPSWindow();
                    rpsWindow.Hide();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        public void chk_SoundEnabled_Checked(object sender, RoutedEventArgs e)
        {
            soundIsChecked = !soundIsChecked;
            if (soundIsChecked)
            {
                playSoundEnabled = true;
                chk_SoundEnabled.IsChecked = true;

            }
            else if (!soundIsChecked)
            {
                playSoundEnabled = false;
                chk_SoundEnabled.IsChecked = false;
            }
        }

        private void btn_SoundProfile_Click(object sender, RoutedEventArgs e)
        {

            soundWindowOpen = !soundWindowOpen;

            if (soundWindowOpen)
            {
                soundProfileWindow.Show();
            }
            else if (!soundWindowOpen)
            {
                soundProfileWindow.Hide();
            }



        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/gRGFDDTBvz",
                UseShellExecute = true
            });
        }

        private void rbn_Greed_Click(object sender, RoutedEventArgs e)
        {

            greedRadialChecked = !greedRadialChecked;
            try
            {
                if (greedRadialChecked)
                {

                    greedWindowIsOpen = true;
                    needGreedWindow.Show();
                    rbn_Greed.IsChecked = true;
                }
                else if (!greedRadialChecked)
                {
                    greedWindowIsOpen = false;
                    rbn_Greed.IsChecked = false;
                    needGreedWindow.Hide();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void rbn_Greed_MouseDown(object sender, MouseButtonEventArgs e)
        {
            greedRadialChecked = !greedRadialChecked;

            try
            {
                if (greedRadialChecked)
                {
                    greedWindowIsOpen = true;
                    needGreedWindow.Show();
                    rbn_Greed.IsChecked = true;

                }
                else if (!greedRadialChecked)
                {
                    greedWindowIsOpen = false;
                    rbn_Greed.IsChecked = false;
                    needGreedWindow.Hide();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void txb_ChatInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (isPrivateMessage)
            {

                    TextBox textBox = sender as TextBox;
                    int indexLength = int.Parse(textBox.Tag.ToString());
                if (e.Key == Key.Left || e.Key == Key.Back)
                {
                    if (textBox != null && textBox.CaretIndex <= indexLength)
                    {
                        e.Handled = true;
                    }
                }
                if (textBox != null && textBox.CaretIndex < indexLength)
                {
                    e.Handled = true;
                }
                if (e.Key == Key.Enter)
                {
                    btn_SendChat_Click(sender, e);
                }
                if (e.Key == Key.Escape)
                {
                    txb_ChatInput.Clear();
                    txb_ChatInput.Foreground = null;
                    isPrivateMessage = false;
                    txb_ChatInput.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                }

            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    btn_SendChat_Click(sender, e);
                }

                if (e.Key == Key.Escape)
                {
                    txb_ChatInput.Clear();
                    txb_ChatInput.Foreground = null;
                    isPrivateMessage = false;
                    txb_ChatInput.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
                }
            }
        }


    }
}
