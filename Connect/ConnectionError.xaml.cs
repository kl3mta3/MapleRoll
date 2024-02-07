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

namespace MapleRoll.Connect
{
    /// <summary>
    /// Interaction logic for ConnectionError.xaml
    /// </summary>
    public partial class ConnectionError : Window
    {
     
        public ConnectionError()
        {
            InitializeComponent();
        
        }

        private void btn_Okay_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
