using MapleRoll.Connect;
using MapleRoll.Net;
using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;




namespace MapleRoll.Roll
{
    /// <summary>
    /// Interaction logic for RollWindow.xaml
    /// </summary>
    /// 
    public partial class RollWindow : Window
    {
        public static PrivateRollWindow privateRollWindow { get; set; }
        public bool privateRollwindowOpen = false;

        //added to try to always stay top.

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        //end added to try to always stay on top.




        public RollWindow()
        {
            InitializeComponent();
            privateRollWindow = new PrivateRollWindow();
        }

        private void btn_Roll_Click(object sender, RoutedEventArgs e)
        {
            if (!MapleRollConnect.hasRolled)
            {
                MapleRollConnect.hasRolled = true;
                MapleRollConnect.SendRollRequestToServer();
            }
        }


        public void UpdateWinningRollData(string roll, string winningMessage, int color)
        {
            Console.WriteLine("Adding winning data to Roll window");
            
            Application.Current.Dispatcher.Invoke((() =>
            {

                switch (color)
                {
                    case 1:
                        lbl_WinningRoll_White.Content = roll;
                        lbl_WinningUser_White.Content = winningMessage;

                        lbl_WinningRoll_Green.Content = "";
                        lbl_WinningUser_Green.Content = "";
                        break;
                    case 2:
                        lbl_WinningRoll_Green.Content = roll;
                        lbl_WinningUser_Green.Content = winningMessage;

                        lbl_WinningRoll_White.Content = "";
                        lbl_WinningUser_White.Content = "";
                        break;
                }
                //lbl_WinningRoll_White.Content= roll;
               
                //lbl_WinningUser_White.Content = winningMessage;
              

            }));

         
        }

        public void UpdateUserRollData(string roll, int color)
        {

            Console.WriteLine("Adding User data to Roll window");

            Application.Current.Dispatcher.Invoke((() =>
            {
                switch (color)
                {
                    case 1:
                        lbl_UserRoll_White.Content = roll;
                        lbl_UserRoll_Green.Content = "";
                        lbl_UserRoll_Red.Content = "";
                        break;
                    case 2:
                        lbl_UserRoll_Green.Content = roll;
                        lbl_UserRoll_White.Content = "";
                        lbl_UserRoll_Red.Content = "";
                        break;
                    case 3:
                        lbl_UserRoll_Red.Content = roll;
                        lbl_UserRoll_Green.Content ="";
                        lbl_UserRoll_White.Content = "";
                        break;
                }
                
               // lbl_UserRoll.Foreground =color;
            }));

        }

        private void RollWindow1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void rbt_PrivateRoll_Click(object sender, RoutedEventArgs e)
        {
            privateRollwindowOpen = !privateRollwindowOpen;

            if (privateRollwindowOpen)
            {
                privateRollWindow.Show();
                rbt_PrivateRoll.IsChecked = true;

            }
            else if (!privateRollwindowOpen)
            {
                privateRollWindow.Hide();
                rbt_PrivateRoll.IsChecked = false;
            }
        }

        private void RollWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (rbt_PrivateRoll.IsChecked==true)
            {

                rbt_PrivateRoll.IsChecked = false;
            }
        }

        private void btn_VoteDone_Click(object sender, RoutedEventArgs e)
        {
            if(!MapleRollConnect.userHasVotedToEndRoll && MapleRollConnect.groupMembers.Count>1)
            {
                MapleRollConnect.userHasVotedToEndRoll = true;
                btn_VoteDone_White.Visibility = Visibility.Hidden;
                btn_VoteDone_Red.Visibility = Visibility.Visible;
                MapleRollConnect.SendVoteToEndRollRequestToServer();
            }
        }

        private void RollWindow1_LostFocus(object sender, RoutedEventArgs e)
        {
            IntPtr Handle =
           new WindowInteropHelper(this).Handle;
            SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
    }
}
