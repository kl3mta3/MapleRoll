using MapleRoll.Connect;
using MapleRoll.Net;
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


namespace MapleRoll.Roll
{
    /// <summary>
    /// Interaction logic for RollWindow.xaml
    /// </summary>
    public partial class RollWindow : Window
    {


        public RollWindow()
        {
            InitializeComponent();
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
    }
}
