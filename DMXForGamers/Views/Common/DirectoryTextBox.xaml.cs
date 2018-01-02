using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMXForGamers.Views.Common
{
    /// <summary>
    /// Interaction logic for DirectoryTextBox.xaml
    /// </summary>
    public partial class DirectoryTextBox : UserControl
    {
        public static readonly DependencyProperty DirectoryPathProperty = DependencyProperty.Register(nameof(DirectoryPath), typeof(string), typeof(DirectoryTextBox),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DirectoryPath
        {
            get { return (string)GetValue(DirectoryPathProperty) ?? string.Empty; }
            set { SetValue(DirectoryPathProperty, value); }
        }

        public DirectoryTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectDirectory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory();
        }

        private void SelectDirectory()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(DirectoryPath))
                {
                    dialog.SelectedPath = DirectoryPath;
                }
                dialog.ShowDialog();
                DirectoryPath = dialog.SelectedPath;
            }
        }
    }
}
