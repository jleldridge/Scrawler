﻿using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using StylusAppU.Data.Data;
using Utils.ViewModel;

namespace StylusAppU.DialogViewModels
{
    public abstract class BackgroundViewModelBase : ViewModelBase
    {
        public BackgroundViewModelBase(BackgroundBase backgroundData)
        {
            BackgroundData = backgroundData;
        }

        public BackgroundBase BackgroundData { get; private set; }

        public double Red
        {
            get { return BackgroundData.BackgroundColor.R; }
            set
            {
                BackgroundData.BackgroundColor = new Color()
                {
                    A = BackgroundData.BackgroundColor.A,
                    R = (byte)value,
                    G = BackgroundData.BackgroundColor.G,
                    B = BackgroundData.BackgroundColor.B
                };
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public double Green
        {
            get { return BackgroundData.BackgroundColor.G; }
            set
            {
                BackgroundData.BackgroundColor = new Color()
                {
                    A = BackgroundData.BackgroundColor.A,
                    R = BackgroundData.BackgroundColor.R,
                    G = (byte)value,
                    B = BackgroundData.BackgroundColor.B
                };
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public double Blue
        {
            get { return BackgroundData.BackgroundColor.B; }
            set
            {
                BackgroundData.BackgroundColor = new Color()
                {
                    A = BackgroundData.BackgroundColor.A,
                    R = BackgroundData.BackgroundColor.R,
                    G = BackgroundData.BackgroundColor.G,
                    B = (byte)value
                };
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public SolidColorBrush BackgroundColorSample
        {
            get { return new SolidColorBrush(new Color() { A = 255, R = (byte)Red, G = (byte)Green, B = (byte)Blue }); }
        }
    }

    public class BackgroundViewModelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SolidBackgroundTemplate { get; set; }
        public DataTemplate GridLineBackgroundTemplate { get; set; }
        public DataTemplate ImageBackgroundTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is SolidBackgroundViewModel) return SolidBackgroundTemplate;
            else if (item is GridLineBackgroundViewModel) return GridLineBackgroundTemplate;
            else return ImageBackgroundTemplate;
        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is SolidBackgroundViewModel) return SolidBackgroundTemplate;
            else if (item is GridLineBackgroundViewModel) return GridLineBackgroundTemplate;
            else return ImageBackgroundTemplate;
        }
    }

    public class SolidBackgroundViewModel : BackgroundViewModelBase
    {
        public SolidBackgroundViewModel(SolidBackground backgroundData) : base(backgroundData)
        {
        }

        public SolidBackground SolidBackground
        {
            get { return BackgroundData as SolidBackground; }
        }
    }

    public class GridLineBackgroundViewModel : BackgroundViewModelBase
    {
        public GridLineBackgroundViewModel(GridLineBackground backgroundData) : base(backgroundData)
        {
        }

        public GridLineBackground GridLineBackground
        {
            get { return BackgroundData as GridLineBackground; }
        }

        public double HorizontalLineSpacing
        {
            get { return GridLineBackground.HorizontalLineSpacing; }
            set
            {
                GridLineBackground.HorizontalLineSpacing = value;
                OnPropertyChanged();
            }
        }

        public double VerticalLineSpacing
        {
            get { return GridLineBackground.VerticalLineSpacing; }
            set
            {
                GridLineBackground.VerticalLineSpacing = value;
                OnPropertyChanged();
            }
        }

        public double VerticalLineThickness
        {
            get { return GridLineBackground.VerticalLineThickness; }
            set
            {
                GridLineBackground.VerticalLineThickness = value;
                OnPropertyChanged();
            }
        }

        public double HorizontalLineThickness
        {
            get { return GridLineBackground.HorizontalLineThickness; }
            set
            {
                GridLineBackground.HorizontalLineThickness = value;
                OnPropertyChanged();
            }
        }
    }

    public class ImageBackgroundViewModel : BackgroundViewModelBase
    {
        public ImageBackgroundViewModel(BackgroundBase backgroundData) : base(backgroundData)
        {
        }

        public ImageBackground ImageBackground
        {
            get { return BackgroundData as ImageBackground; }
        }
    }
}
