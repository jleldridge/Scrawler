using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace StylusAppU.BackgroundControls
{
    public class GridLineBackgroundControl : Canvas
    {
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

            control.Children.Clear();

            control.AddHorizontalLines();
            control.AddVerticalLines();
            control.InvalidateArrange();
        }

        private void AddHorizontalLines()
        {
            int numberOfHorizontalLines = (int)(Height / (HorizontalLineThickness + HorizontalLineSpacing));
            var lineColor = new SolidColorBrush(LineColor);
            for (int i = 0; i < numberOfHorizontalLines; i++)
            {
                double y = i * (HorizontalLineThickness + HorizontalLineThickness);
                var line = new Line
                {
                    Stroke = lineColor,
                    StrokeThickness = HorizontalLineThickness,
                    X1 = 0,
                    X2 = Width,
                    Y1 = y,
                    Y2 = y
                };
                Children.Add(line);
            }
        }

        private void AddVerticalLines()
        {
            int numberOfVerticalLines = (int)(Width / (VerticalLineThickness + VerticalLineSpacing));
            var lineColor = new SolidColorBrush(LineColor);
            for (int i = 0; i < numberOfVerticalLines; i++)
            {
                double x = i * (VerticalLineThickness + VerticalLineSpacing);
                var line = new Line
                {
                    Stroke = lineColor,
                    StrokeThickness = VerticalLineThickness,
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = Height
                };
                Children.Add(line);
            }
        }
    }
}
