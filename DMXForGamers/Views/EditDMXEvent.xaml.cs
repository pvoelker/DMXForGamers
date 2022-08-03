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

namespace DMXForGamers.Views
{
    /// <summary>
    /// Interaction logic for EditEvent.xaml
    /// </summary>
    public partial class EditDMXEvent : UserControl
    {
        public EditDMXEvent()
        {
            InitializeComponent();
        }

        private void ShiftTimeButton_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as Models.DMXEvent;

            var mapper = new Mappers.ShiftTimeBlock();

            var dlg = new ShiftTimeBlocksWindow();

            var dlgData = new ViewModels.ShiftTimeBlocks();
            dlgData.Values.AddRange(data.TimeBlocks.OrderBy(x => x.StartTime).Select(x => mapper.ToModel(x)));

            dlg.DataContext = dlgData;

            dlg.ShowDialog();

            if(dlg.IsApplied)
            {
                foreach (var item in dlgData.Values.Where(x => x.NewStartTime.HasValue))
                {
                    var itemToUpdate = data.TimeBlocks.Single(x => x.StartTime == item.StartTime);
                    mapper.UpdateFromModel(item, itemToUpdate);
                }
            }
        }
    }
}
