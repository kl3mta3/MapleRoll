using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for PrivateRollWindow.xaml
    /// </summary>
    public partial class PrivateRollWindow : Window
    {
        
        public PrivateRollWindow()
        {
            InitializeComponent();
        }

        private void PrivateRollWindow1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void PrivateRoll()

        {
            Random rand = new Random();
            int roll = rand.Next(1, 100);
            lbl_PrivateRoll.Content = roll.ToString();
        }

        private void btn_PrivateRoll_Click(object sender, RoutedEventArgs e)
        {
            PrivateRoll();
            PlayWowSound();
        }

        private void PlayWowSound()
        {

            SoundPlayer player = new SoundPlayer(Properties.Resources.wow_c_101soundboards);
            player.Load();
            player.Play();

        }
    }
}
