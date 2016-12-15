using StylusAppU.Data.Data;
using StylusAppU.Data.Serialization;
using StylusAppU.ViewModel;
using Utils.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StylusAppU.DialogViewModels
{
    public class PageOptionsViewModel : ViewModelBase
    {
        private double _height, _width;
        //private double _red, _green, _blue;

        public PageOptionsViewModel(PageViewModel page)
        {
            Width = page.Width;
            Height = page.Height;
            BackgroundData = DataContractHelper.Clone(page.BackgroundData);
            //Red = page.BackgroundData.BackgroundColor.R;
            //Green = page.BackgroundData.BackgroundColor.G;
            //Blue = page.BackgroundData.BackgroundColor.B;
        }

        public BackgroundBase BackgroundData { get; private set; }

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
}
