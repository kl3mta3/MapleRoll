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

namespace MapleRoll
{
    public partial class ContextWindow : Window
    {
        public ListViewItem selectedUser { get; set; }
        public MainWindow mainWindow { get; set; }
        public bool contextUsed = false;
        public bool mouseOver = false;

        public ContextWindow()
        {
            InitializeComponent();
            mainWindow = MapleRollConnect.mainWindow;
            Loaded += Window_Loaded;
        }

        private void btn_SendPM_Click(object sender, RoutedEventArgs e)
        {

                contextUsed = true;
                mainWindow.messageOnClickList(selectedUser);
            
           
        }

        private void btn_VoteToKick_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser.Uid != MapleRollConnect.uId)
            {
                contextUsed = true;
                mainWindow.kickOnClickList(selectedUser);
            }
           
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
                mouseOver = false;
            if (!contextUsed)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_ContextSelectedUser.Content=selectedUser.Name;
        }



        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOver = true;
        }
    }
}
