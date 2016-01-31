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

namespace TestTabControlApp
{
    /// <summary>
    /// Interaction logic for MyTabItem.xaml
    /// </summary>
    public partial class ClosableHeader : UserControl
    {
        public String MyHeader
        { 
            get { return txtHeader.Text; } 
            set { txtHeader.Text = value; } 
        }

        public ClosableHeader()
        {
            InitializeComponent();
        }

        private void CloseButton_click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
