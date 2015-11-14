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
            ShowsNavigationUI = false;
        }
        
      

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                boardPage p = new boardPage();
                p.label2.Content = textBox.Text;
                p.label2.HorizontalContentAlignment = HorizontalAlignment.Right;
                this.NavigationService.Navigate(p);
            }

        }

        
    }
}
