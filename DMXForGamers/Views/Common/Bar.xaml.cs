using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMXForGamers.Views.Common
{
    public partial class Bar : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(Bar),
            new FrameworkPropertyMetadata((int)20, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValuesChanged)));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(Bar),
            new FrameworkPropertyMetadata((int)30, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValuesChanged)));

        private static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(nameof(BarHeight), typeof(double), typeof(Bar),
            new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.AffectsRender));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        private double BarHeight
        {
            get { return (double)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        public Bar()
        {
            InitializeComponent();
        }

        private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = d as Bar;

            if (This.MaxValue == 0)
                This.BarHeight = 0;
            else
                This.BarHeight = This._Grid.ActualHeight * (This.Value / (double)This.MaxValue);
        }
    }
}
