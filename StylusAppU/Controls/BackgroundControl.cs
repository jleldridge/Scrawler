using System;
using System.ComponentModel;
using StylusAppU.Data.Data;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using StylusAppU.DialogViewModels;

namespace StylusAppU.Controls
{
    public class BackgroundControl : Canvas
    {
        public BackgroundControl()
        {
            SizeChanged += GridLineBackgroundControl_SizeChanged;
        }

        private void GridLineBackgroundControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawChildren();
        }

        public static readonly DependencyProperty BackgroundViewModelProperty = DependencyProperty.Register(
            "BackgroundViewModel", 
            typeof (BackgroundViewModelBase), 
            typeof (BackgroundControl), 
            new PropertyMetadata(default(BackgroundViewModelBase), BackgroundViewModelChanged));

        public BackgroundViewModelBase BackgroundViewModel
        {
            get { return (BackgroundViewModelBase) GetValue(BackgroundViewModelProperty); }
            set { SetValue(BackgroundViewModelProperty, value); }
        }

        private static void BackgroundViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = ((BackgroundControl)d);
            control.BackgroundViewModel.PropertyChanged += control.BackgroundViewModelOnPropertyChanged;
            control.RedrawChildren();
        }

        private void BackgroundViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RedrawChildren();
        }

        private void RedrawChildren()
        {
            Children.Clear();
            Background = new SolidColorBrush(BackgroundViewModel.BackgroundData.BackgroundColor);
            
            if (BackgroundViewModel.BackgroundData is GridLineBackground)
            {
                var background = BackgroundViewModel.BackgroundData as GridLineBackground;
                DrawGridLineBackground(background);
            }
            InvalidateArrange();
        }

        private void DrawGridLineBackground(GridLineBackground background)
        {
            var lineColor = new SolidColorBrush(background.LineColor);

            if (background.HorizontalLineSpacing > 0)
            {
                double y = background.HorizontalLineSpacing;
                while (y < Height - 1)
                {
                    var line = new Line
                    {
                        Stroke = lineColor,
                        StrokeThickness = background.HorizontalLineThickness,
                        X1 = 0,
                        X2 = Width,
                        Y1 = y - background.HorizontalLineThickness / 2,
                        Y2 = y - background.HorizontalLineThickness / 2
                    };
                    Children.Add(line);

                    y += background.HorizontalLineSpacing;
                }
            }

            if (background.VerticalLineSpacing > 0)
            {
                double x = background.VerticalLineSpacing;
                while (x < Width)
                {
                    var line = new Line
                    {
                        Stroke = lineColor,
                        StrokeThickness = background.VerticalLineThickness,
                        X1 = x - background.VerticalLineThickness / 2,
                        X2 = x - background.VerticalLineThickness / 2,
                        Y1 = 0,
                        Y2 = Height
                    };
                    Children.Add(line);

                    x += background.VerticalLineSpacing;
                }
            }
        }
    }
}
