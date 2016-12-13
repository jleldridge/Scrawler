using StylusAppU.Data.Data;
using StylusAppU.ViewModel;
using System.Collections.Generic;
using Utils.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StylusAppU.DialogViewModels
{
    public class PageOptionsViewModel : ViewModelBase
    {
        private double _height, _width;
        private double _red, _green, _blue;
        private BackgroundType _backgroundType;

        public PageOptionsViewModel(PageViewModel page)
        {
            Width = page.Width;
            Height = page.Height;
            Red = page.BackgroundColor.R;
            Green = page.BackgroundColor.G;
            Blue = page.BackgroundColor.B;
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public List<BackgroundType> BackgroundTypes
        {
            get
            {
                return new List<BackgroundType>()
                {
                    BackgroundType.Solid,
                    BackgroundType.HorizontalLines,
                    BackgroundType.VerticalLines,
                    BackgroundType.Grid,
                    BackgroundType.Image
                };
            }
        }

        public BackgroundType BackgroundType
        {
            get { return _backgroundType; }
            set
            {
                _backgroundType = value;
                OnPropertyChanged();
            }
        }

        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public double Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public double Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged();
                OnPropertyChanged("BackgroundColorSample");
            }
        }

        public SolidColorBrush BackgroundColorSample
        {
            get { return new SolidColorBrush(new Color() { A = 255, R = (byte)Red, G = (byte)Green, B = (byte)Blue }); }
        }
    }
}
