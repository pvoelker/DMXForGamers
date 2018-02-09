using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMXForGamers.Views.Common
{
    public partial class FileTextBox : UserControl
    {
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), typeof(FileTextBox),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty) ?? string.Empty; }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(FileTextBox),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty) ?? string.Empty; }
            set { SetValue(FilePathProperty, value); }
        }

        public FileTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }

        private void SelectFile()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.CheckFileExists = false;
                dialog.ValidateNames = false;
                if (string.IsNullOrWhiteSpace(Filter) == true)
                    dialog.Filter = "All files (*.*)|*.*";
                else
                    dialog.Filter = Filter;
                if (!string.IsNullOrEmpty(FilePath))
                {
                    dialog.FileName = FilePath;
                }
                dialog.ShowDialog();
                FilePath = dialog.FileName;
            }
        }
    }
}
