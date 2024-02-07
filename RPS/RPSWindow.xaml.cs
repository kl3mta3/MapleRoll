using MapleRoll.Connect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MapleRoll.RPS
{
    /// <summary>
    /// Interaction logic for RPSWindow.xaml
    /// </summary>
    /// 

    public partial class RPSWindow : Window
    {
        public Button rockButton = new Button();
        public Button paperButton = new Button();
        public Button scissorsButton = new Button();

        public bool IsRock = false;
        public bool IsPaper= false;
        public bool IsScissors=false;
        public bool sentChoice = false;
        public bool hasPicked = false;

        public Rectangle paperSelected= new Rectangle();
        public Rectangle rockSelected = new Rectangle();
        public Rectangle scissorsSelected = new Rectangle();

        public Rectangle paperUnSelected = new Rectangle();
        public Rectangle rockUnSelected = new Rectangle();
        public Rectangle scissorsUnSelected = new Rectangle();

        public string rPSThrow = "";

        public RPSWindow()
        {
            InitializeComponent();

            rockButton = btn_Rock;
            paperButton = btn_Paper;
            scissorsButton = btn_Scissors;

            paperUnSelected = ret_Paper_UnSelected;
            rockUnSelected = ret_Rock_UnSelected;
            scissorsUnSelected = ret_Scissors_UnSelected;

            paperSelected = ret_Paper_Selected;
            rockSelected = ret_Rock_Selected;
            scissorsSelected = ret_Scissors_Selected;

        }

        public void ChoosePaper()
        {
            if(!hasPicked)
            {
                paperButton.Visibility = Visibility.Hidden;
                rockButton.Visibility = Visibility.Hidden;
                scissorsButton.Visibility = Visibility.Hidden;

                paperUnSelected.Visibility = Visibility.Hidden;
                rockUnSelected.Visibility = Visibility.Hidden;
                scissorsUnSelected.Visibility = Visibility.Hidden;

                paperSelected.Visibility = Visibility.Visible;
                rockSelected.Visibility = Visibility.Hidden;
                scissorsSelected.Visibility = Visibility.Hidden;

                IsPaper=true;
                hasPicked = true;
                rPSThrow = "Paper";

            }
            //BrodcastThrow();
        }

        public void ChooseRock()
        {
            if (!hasPicked)
            {
                paperButton.Visibility = Visibility.Hidden;
                rockButton.Visibility = Visibility.Hidden;
                scissorsButton.Visibility = Visibility.Hidden;

                paperUnSelected.Visibility = Visibility.Hidden;
                rockUnSelected.Visibility = Visibility.Hidden;
                scissorsUnSelected.Visibility = Visibility.Hidden;

                paperSelected.Visibility = Visibility.Hidden;
                rockSelected.Visibility = Visibility.Visible;
                scissorsSelected.Visibility = Visibility.Hidden;

                IsRock=true;
                hasPicked = true;
                rPSThrow = "Rock";
            }
            //BrodcastThrow();
        }

        public void ChooseScissors()
        {
            if (!hasPicked)
            {
                paperButton.Visibility = Visibility.Hidden;
                rockButton.Visibility = Visibility.Hidden;
                scissorsButton.Visibility = Visibility.Hidden;

                paperUnSelected.Visibility = Visibility.Hidden;
                rockUnSelected.Visibility = Visibility.Hidden;
                scissorsUnSelected.Visibility = Visibility.Hidden;

                paperSelected.Visibility = Visibility.Hidden;
                rockSelected.Visibility = Visibility.Hidden;
                scissorsSelected.Visibility = Visibility.Visible;

                hasPicked = true;
                IsScissors=true;
                rPSThrow = "Scissors";
            }
            //BrodcastThrow();
        }

        public void ResetRPSWindow()
        {
            paperButton.Visibility = Visibility.Visible;
            rockButton.Visibility = Visibility.Visible;
            scissorsButton.Visibility = Visibility.Visible;

            paperUnSelected.Visibility = Visibility.Hidden;
            rockUnSelected.Visibility = Visibility.Hidden;
            scissorsUnSelected.Visibility = Visibility.Hidden;

            paperSelected.Visibility = Visibility.Hidden;
            rockSelected.Visibility = Visibility.Hidden;
            scissorsSelected.Visibility = Visibility.Hidden;

            hasPicked = false;
            IsPaper = false;
            IsRock = false;
            IsScissors = false;
            sentChoice = false;
            rPSThrow = "";
        }



        public void BrodcastThrow()
        {
            if (hasPicked &&!sentChoice)
            {
                string msg = $"{MapleRollConnect.userName} throws {rPSThrow}.";

                MapleRollConnect._server.SendRPSToServerGroup(MapleRollConnect.groupId, msg);
                sentChoice = true;
            }
        }

        private void btn_FlipReset_Click(object sender, RoutedEventArgs e)
        {
            if (hasPicked)
            {
                ResetRPSWindow();
            }
        }

        private void btn_FlipCoin_Click(object sender, RoutedEventArgs e)
        {
            if (hasPicked)
            {
                BrodcastThrow();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((() => MapleRollConnect.mainWindow.rbn_RPS.IsChecked = false));
            MapleRollConnect.mainWindow.rpsWindowIsOpen = false;
            MapleRollConnect.mainWindow.rpsRadialChecked = false;
            ResetRPSWindow();
            this.Hide();
        }

        private void btn_Scissors_Click(object sender, RoutedEventArgs e)
        {
            ChooseScissors();
        }

        private void btn_Paper_Click(object sender, RoutedEventArgs e)
        {
            ChoosePaper();
        }

        private void btn_Rock_Click(object sender, RoutedEventArgs e)
        {
            ChooseRock();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
