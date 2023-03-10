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

namespace Ixian_DLT_GUI
{
    public partial class Confirm : Window
    {
        public Confirm()
        {
            InitializeComponent();
        }
        public string ResponseText
        {
            get { return ResponseTextBox.Content.ToString(); }
            set { ResponseTextBox.Content = value; }
        }
        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
        private void NOButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
