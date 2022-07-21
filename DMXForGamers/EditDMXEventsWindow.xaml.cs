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
            var data = DataContext as Models.DMXDefinitions;           

            var errors = data.Validate();

            if (errors.Count() > 0)
            {
                var msg = new StringBuilder();
                msg.AppendLine("The DMX events contain the following errors:");
                msg.AppendLine(String.Empty);
                foreach (var error in errors)
                {
                    msg.AppendLine("   • " + error);
                }

                MessageBox.Show(msg.ToString(), "Unable to Save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IsSave = true;
                this.Close();
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            IsSave = false;
            this.Close();
        }
    }
}
