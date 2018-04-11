using System;
using DMXForGamers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFSpark;

namespace DMXForGamers.Views
{
    /// <summary>
    /// Interaction logic for ManualEvents.xaml
    /// </summary>
    public partial class ManualEvents : UserControl
    {
        public ManualEvents()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                var control = sender as Button;
                var data = control.DataContext as EventDefinition;

                if(data.EventOn != null)
                {
                    data.EventOn.Execute(data.EventID);
                }
            }
            else if(sender is ToggleSwitch)
            {
                var control = sender as ToggleSwitch;
                var data = control.DataContext as EventDefinition;

                if(control.IsChecked == false)
                {
                    if (data.EventOff != null)
                    {
                        data.EventOff.Execute(data.EventID);
                    }
                }
                else
                {
                    if (data.EventOn != null)
                    {
                        data.EventOn.Execute(data.EventID);
                    }
                }
            }
            else
            {
                throw new Exception("Unknown control type encountered in button click event");
            }
        }
    }
}
