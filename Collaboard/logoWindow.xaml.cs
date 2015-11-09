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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collaboard
{
    /// <summary>
    /// Interaction logic for mainBoard.xaml
    /// </summary>
    public partial class mainBoard : Page
    {
        public mainBoard()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            boardPage p = new boardPage();
            this.NavigationService.Navigate(p);
        }
    }
}
