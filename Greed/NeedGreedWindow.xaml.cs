using MapleRoll.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapleRoll.Greed
{

    public partial class NeedGreedWindow : Window
    {

        public static bool hasRolled;
        public NeedGreedWindow()
        {
            InitializeComponent();
        }

        public void UpdateWinningRollData(string roll, string winningMessage, int color, string rollType )
        {
            Console.WriteLine("Adding winning data to Roll window");

            Application.Current.Dispatcher.Invoke((() =>
            {

                switch (rollType)
                {
                    case "Need":

                        switch (color)
                        {
                            case 1:
                                lbl_WinningUser_Green.Content = winningMessage;
                                lbl_WinningUser_White.Content = "";

                                lbl_WinningRoll_Greed.Content = "";
                                lbl_WinningRoll_Need.Content = roll;
                            break;

                            case 2:
                                lbl_WinningUser_Green.Content = "";
                                lbl_WinningUser_White.Content = winningMessage;

                                lbl_WinningRoll_Greed.Content = "";
                                lbl_WinningRoll_Need.Content = roll;
                            break;
                        }
                    break;

                    case "Greed":

                        switch (color)
                        {
                            case 1:
                                lbl_WinningUser_Green.Content = winningMessage;
                                lbl_WinningUser_White.Content = "";

                                lbl_WinningRoll_Greed.Content = roll;
                                lbl_WinningRoll_Need.Content = "";
                            break;

                            case 2:
                                lbl_WinningUser_Green.Content = "";
                                lbl_WinningUser_White.Content = winningMessage;

                                lbl_WinningRoll_Greed.Content = roll;
                                lbl_WinningRoll_Need.Content = "";
                            break;
                        }
                    break;
                }
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
                        lbl_UserRoll_Green.Content = "";
                        lbl_UserRoll_White.Content = "";
                    break;
                }
            }));

        }



        private void NeedRoll(object sender, RoutedEventArgs e)
        {
            //send greed roll request to server
            if (!MapleRollConnect.hasNeedGreedRolled)
            {
                MapleRollConnect.hasNeedGreedRolled = true;
                lbl_UserText_Need.Visibility = Visibility.Visible;
                lbl_UserText_Greed.Visibility = Visibility.Hidden;
                btn_VoteDone_White.Visibility = Visibility.Visible;
                btn_VoteDone_Red.Visibility = Visibility.Hidden;
                MapleRollConnect.SendNeedRollToServer();
            }
        }

        private void GreedRoll(object sender, RoutedEventArgs e)
        {
            //send greed roll request to server
            if (!MapleRollConnect.hasNeedGreedRolled)
            {
                    MapleRollConnect.hasNeedGreedRolled=true;
                    lbl_UserText_Need.Visibility = Visibility.Hidden;
                    lbl_UserText_Greed.Visibility = Visibility.Visible;
                btn_VoteDone_White.Visibility = Visibility.Visible;
                btn_VoteDone_Red.Visibility = Visibility.Hidden;
                MapleRollConnect.SendGreedRollToServer();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btn_VoteDone_White_Click(object sender, RoutedEventArgs e)
        {

            if (!MapleRollConnect.userHasVotedToEndNeedGreedRoll && MapleRollConnect.groupMembers.Count > 1)
            {
                MapleRollConnect.userHasVotedToEndNeedGreedRoll = true;
                btn_VoteDone_White.Visibility = Visibility.Hidden;
                btn_VoteDone_Red.Visibility = Visibility.Visible;
                MapleRollConnect.SendVoteToEndNeedGreedRollRequestToServer();
            }


        }

      
    }
}
