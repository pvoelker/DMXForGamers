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
    public partial class EditEventsWindow : Window
    {
        public EditEventsWindow()
        {
            InitializeComponent();            
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(DataContext != null)
            {
                var data = DataContext as Models.EventDefinitions;

                data.AddEvent = new RelayCommand(x =>
                {
                    var events = DataContext as Models.EventDefinitions;

                    events.Events.Add(new EventDefinition());
                });
            }
        }
    }
}
