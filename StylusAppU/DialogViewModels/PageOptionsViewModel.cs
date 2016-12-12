using StylusAppU.ViewModel;
using Utils.ViewModel;
using Windows.UI;

namespace StylusAppU.DialogViewModels
{
    public class PageOptionsViewModel : ViewModelBase
    {
        private double _height, _width;
        private double _red, _green, _blue;

        public PageOptionsViewModel(PageViewModel page)
        {
            Width = page.Width;
            Height = page.Height;
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

        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged();
            }
        }

        public double Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged();
            }
        }

        public double Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged();
            }
        }
    }
}
