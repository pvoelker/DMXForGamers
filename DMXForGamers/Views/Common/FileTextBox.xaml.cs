using System;
using System.IO;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMXForGamers.Views.Common
{
    [SupportedOSPlatform("windows")]
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

        public static readonly DependencyProperty FileNameOnlyProperty = DependencyProperty.Register(nameof(FileNameOnly), typeof(bool), typeof(FileTextBox),
    new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool FileNameOnly
        {
            get { return (bool)GetValue(FileNameOnlyProperty); }
            set { SetValue(FileNameOnlyProperty, value); }
        }

        public static readonly DependencyProperty ReadDataProperty = DependencyProperty.Register(nameof(ReadData), typeof(bool), typeof(FileTextBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ReadData
        {
            get { return (bool)GetValue(ReadDataProperty); }
            set { SetValue(ReadDataProperty, value); }
        }

        public static readonly DependencyProperty FileSizeByteLimitProperty = DependencyProperty.Register(nameof(FileSizeByteLimit), typeof(long), typeof(FileTextBox),
            new FrameworkPropertyMetadata(1024000L /*1 MB*/, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public long FileSizeByteLimit
        {
            get { return (long)GetValue(FileSizeByteLimitProperty); }
            set { SetValue(FileSizeByteLimitProperty, value); }
        }

        public static readonly DependencyProperty FileDataProperty = DependencyProperty.Register(nameof(FileData), typeof(byte[]), typeof(FileTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public byte[] FileData
        {
            get { return (byte[])GetValue(FileDataProperty); }
            set { SetValue(FileDataProperty, value); }
        }

        public static readonly DependencyProperty FreeEditProperty = DependencyProperty.Register(nameof(FreeEdit), typeof(bool), typeof(FileTextBox),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool FreeEdit
        {
            get { return (bool)GetValue(FreeEditProperty); }
            set { SetValue(FreeEditProperty, value); }
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
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if(ReadData == true)
                    {
                        var fileInfo = new FileInfo(dialog.FileName);

                        if (fileInfo.Length <= FileSizeByteLimit)
                        {
                            try
                            {
                                FileData = File.ReadAllBytes(dialog.FileName);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Unable to read binary data for file: {dialog.FileName}.  See inner exception for details.", ex);
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show($"Unable to load file data.  Please select another equal to or less than {BytesToString(FileSizeByteLimit)} in length.", "File Too Large", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }
                if(FileNameOnly)
                    FilePath = Path.GetFileName(dialog.FileName);
                else
                    FilePath = dialog.FileName;
            }
        }

        private static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
