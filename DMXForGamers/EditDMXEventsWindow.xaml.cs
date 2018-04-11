using DMXForGamers.Models;
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

namespace DMXForGamers
{
    /// <summary>
    /// Interaction logic for EditEventsWindow.xaml
    /// </summary>
    public partial class EditDMXEventsWindow : Window
    {
        public EditDMXEventsWindow()
        {
            InitializeComponent();            
        }

        public bool IsSave { get; set; } = false;

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            IsSave = true;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            IsSave = false;
            this.Close();
        }
    }
}
