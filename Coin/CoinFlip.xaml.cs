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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapleRoll.Coin
{
    /// <summary>
    /// Interaction logic for CoinFlip.xaml
    /// </summary>
    public partial class CoinFlipWindow : Window
    {
        private StackPanel flipIcon = new StackPanel();
        private StackPanel headsIcon = new StackPanel();
        private StackPanel tailsIcon = new StackPanel();

        private bool isHeads = false;
        private bool isTails = false;
        private bool flipIconEnabled = true;


        public CoinFlipWindow()
        {
            InitializeComponent();
            flipIcon = stk_FlipIcon;
            headsIcon = stk_HeadsIcon;
            tailsIcon = stk_TailsIcon;
        }

        private void FlipCoin()
        {
            isHeads = false; 
            isTails=false;
            string flip = "null";
            Random random = new Random();

            int n = random.Next(100);

            if(n<=50)
            {
                isHeads = true;
                isTails = false;
                if (flipIconEnabled)
                {

                    flipIconEnabled = false;
                    flipIcon.Visibility = Visibility.Hidden;
                }

                stk_HeadsIcon.Visibility = Visibility.Visible;
                stk_TailsIcon.Visibility=Visibility.Hidden;
                flip = "Heads";
                string msg = $"{MapleRollConnect.userName} fliped a coin, it's {flip}";
                Console.WriteLine($"Coin Flipped MSG: {msg}");
                BrodcastFlipToGroup(msg);

            }
            else if(n>50)
            {

                isHeads = false;
                isTails = true;
                if (flipIconEnabled)
                {

                    flipIconEnabled = false;
                    flipIcon.Visibility = Visibility.Hidden;
                }
                stk_HeadsIcon.Visibility = Visibility.Hidden;
                stk_TailsIcon.Visibility = Visibility.Visible;
                flip = "Tails";
                string msg = $"{MapleRollConnect.userName} fliped a coin, it's {flip}";
                Console.WriteLine($"Coin Flipped MSG: {msg}");
                BrodcastFlipToGroup(msg);
            }


        }
        
        private void BrodcastFlipToGroup(string msg)
        {
            Console.WriteLine($"Brodcasting CoinFLip to Group");
            MapleRollConnect._server.SendCoinFlipToServerGroup(MapleRollConnect.groupId,msg);


        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((() => MapleRollConnect.mainWindow.rbn_Coin.IsChecked=false));
            MapleRollConnect.mainWindow.coinFlipWindowIsOpen = false;
            MapleRollConnect.mainWindow.coinRadialChecked = false;
            this.Hide();
            
        }

        private void btn_FlipCoin_Click(object sender, RoutedEventArgs e)
        {
            FlipCoin();
        }

        private void btn_Flip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
