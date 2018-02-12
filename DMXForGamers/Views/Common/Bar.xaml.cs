using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DMXForGamers.Views.Common
{
    public partial class Bar : UserControl
    {
        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register(nameof(BarColor), typeof(Brush), typeof(Bar),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Blue), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(Bar),
            new FrameworkPropertyMetadata((int)20, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValuesChanged)));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(Bar),
            new FrameworkPropertyMetadata((int)30, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValuesChanged)));

        private static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(nameof(BarHeight), typeof(double), typeof(Bar),
            new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush BarColor
        {
            get { return (Brush)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }

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

            CalcBarHeight(This);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalcBarHeight(this);
        }

        private static void CalcBarHeight(Bar This)
        {
            if (This.MaxValue == 0)
                This.BarHeight = 0;
            else
                This.BarHeight = This._Grid.ActualHeight * (This.Value / (double)This.MaxValue);
        }
    }
}
