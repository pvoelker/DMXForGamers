using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMXForGamers.Views.Common
{
    public partial class DocTextBox : UserControl
    {
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), typeof(DocTextBox),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty) ?? string.Empty; }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(DocTextBox),
            new FrameworkPropertyMetadata((string)string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnFilePathOrEditCommandChange)));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty) ?? string.Empty; }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(nameof(NewCommand), typeof(ICommand), typeof(DocTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ICommand NewCommand
        {
            get { return (ICommand)GetValue(NewCommandProperty); }
            set { SetValue(NewCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(DocTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnFilePathOrEditCommandChange)));

        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty IsEditAllowedProperty = DependencyProperty.Register(nameof(IsEditAllowed), typeof(bool), typeof(DocTextBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsEditAllowed
        {
            get { return (bool)GetValue(IsEditAllowedProperty); }
            private set { SetValue(IsEditAllowedProperty, value); }
        }

        // ----------------------------------------------------------------------------------------

        static private void OnFilePathOrEditCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = d as DocTextBox;

            This.IsEditAllowed = (This.FilePath != null) && (This.FilePath.Length > 0) && (This.EditCommand != null);
        }

        // ----------------------------------------------------------------------------------------

        public DocTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenOrNewFile(false);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenOrNewFile(false);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            OpenOrNewFile(true);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (EditCommand != null)
            {
                if (EditCommand.CanExecute(null) == true)
                {
                    EditCommand.Execute(null);
                }
            }
        }

        // ----------------------------------------------------------------------------------------

        private void OpenOrNewFile(bool newFile)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.CheckFileExists = !newFile;
                dialog.CheckPathExists = newFile;
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
