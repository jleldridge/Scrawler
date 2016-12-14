using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace StylusAppU.BackgroundControls
{
    public class GridLineBackgroundControl : Canvas
    {
        public GridLineBackgroundControl()
        {
            SizeChanged += GridLineBackgroundControl_SizeChanged;
        }

        private void GridLineBackgroundControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawChildren();
        }

        public static readonly DependencyProperty HorizontalLineThicknessProperty = DependencyProperty.Register(
            "HorizontalLineThickness", typeof (double), typeof (GridLineBackgroundControl), new PropertyMetadata(default(double), RedrawChildren));

        public double HorizontalLineThickness
        {
            get { return (double) GetValue(HorizontalLineThicknessProperty); }
            set { SetValue(HorizontalLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty VerticalLineThicknessProperty = DependencyProperty.Register(
            "VerticalLineThickness", typeof (double), typeof (GridLineBackgroundControl), new PropertyMetadata(default(double), RedrawChildren));

        public double VerticalLineThickness
        {
            get { return (double) GetValue(VerticalLineThicknessProperty); }
            set { SetValue(VerticalLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty HorizontalLineSpacingProperty = DependencyProperty.Register(
            "HorizontalLineSpacing", typeof (double), typeof (GridLineBackgroundControl), new PropertyMetadata(default(double), RedrawChildren));

        public double HorizontalLineSpacing
        {
            get { return (double) GetValue(HorizontalLineSpacingProperty); }
            set { SetValue(HorizontalLineSpacingProperty, value); }
        }

        public static readonly DependencyProperty VerticalLineSpacingProperty = DependencyProperty.Register(
            "VerticalLineSpacing", typeof (double), typeof (GridLineBackgroundControl), new PropertyMetadata(default(double), RedrawChildren));

        public double VerticalLineSpacing
        {
            get { return (double) GetValue(VerticalLineSpacingProperty); }
            set { SetValue(VerticalLineSpacingProperty, value); }
        }

        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register(
            "LineColor", typeof (Color), typeof (GridLineBackgroundControl), new PropertyMetadata(default(Color), RedrawChildren));

        public Color LineColor
        {
            get { return (Color) GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        private static void RedrawChildren(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (GridLineBackgroundControl)d;
            control.RedrawChildren();
        }

        private void RedrawChildren()
        {
            Children.Clear();

            AddHorizontalLines();
            AddVerticalLines();
            InvalidateArrange();
        }

        private void AddHorizontalLines()
        {
            var lineColor = new SolidColorBrush(LineColor);

            double y = HorizontalLineSpacing;
            while (y < Height - 1)
            {
                var line = new Line
                {
                    Stroke = lineColor,
                    StrokeThickness = HorizontalLineThickness,
                    X1 = 0,
                    X2 = Width,
                    Y1 = y - HorizontalLineThickness / 2,
                    Y2 = y - HorizontalLineThickness / 2
                };
                Children.Add(line);

                y += HorizontalLineSpacing;
            }
        }

        private void AddVerticalLines()
        {
            var lineColor = new SolidColorBrush(LineColor);

            double x = VerticalLineSpacing;
            while (x < Width)
            {
                var line = new Line
                {
                    Stroke = lineColor,
                    StrokeThickness = VerticalLineThickness,
                    X1 = x - VerticalLineThickness / 2,
                    X2 = x - VerticalLineThickness / 2,
                    Y1 = 0,
                    Y2 = Height
                };
                Children.Add(line);

                x += VerticalLineSpacing;
            }
        }
    }
}
