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

namespace MapleRoll
{
    /// <summary>
    /// Interaction logic for SoundProfile.xaml
    /// </summary>
    public partial class SoundProfileWindow : Window
    {
        public SoundProfileWindow()
        {
            InitializeComponent();
        }



        public void ChangeProfile(object sender, RoutedEventArgs e)
        {
            if (rbt_Default.IsChecked==true)
            {
                MapleRollConnect.userSoundProfile = 1;
            }
            else if (rbt_Mario.IsChecked == true)
            {
                MapleRollConnect.userSoundProfile = 2;

            }
            else if (rbt_Bender.IsChecked == true)
            {
                MapleRollConnect.userSoundProfile = 3;
            }
            else if (rbt_FamilyGuy.IsChecked == true)
            {
                MapleRollConnect.userSoundProfile = 4;
            }
            else if (rbt_Zelda.IsChecked == true)
            {
                MapleRollConnect.userSoundProfile = 5;
            }
            else if (rbt_Southpark.IsChecked == true)
            {
                MapleRollConnect.userSoundProfile = 6;
            }
            else  
            {
                MapleRollConnect.userSoundProfile = 1;
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();


        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
