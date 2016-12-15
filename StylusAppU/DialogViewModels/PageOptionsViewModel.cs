using StylusAppU.Data.Data;
using StylusAppU.Data.Serialization;
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
        private BackgroundType _selectedType;

        public PageOptionsViewModel(PageViewModel page)
        {
            Width = page.Width;
            Height = page.Height;
            BackgroundData = DataContractHelper.Clone(page.BackgroundData);
            
            if (BackgroundData is SolidBackground)
            {
                _selectedType = BackgroundType.Solid;
            }
            else if (BackgroundData is GridLineBackground)
            {
                _selectedType = BackgroundType.Grid;
            }
            else if (BackgroundData is ImageBackground)
            {
                _selectedType = BackgroundType.Image;
            }
        }

        public List<BackgroundType> BackgroundTypes
        {
            get
            {
                return new List<BackgroundType>()
                {
                    BackgroundType.Solid,
                    BackgroundType.Grid,
                    BackgroundType.Image
                };
            }
        }

        public BackgroundType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged();

                switch (_selectedType)
                {
                    case BackgroundType.Solid:
                        BackgroundData = new SolidBackground();
                        break;
                    case BackgroundType.Grid:
                        BackgroundData = new GridLineBackground();
                        break;
                    case BackgroundType.Image:
                        BackgroundData = new ImageBackground();
                        break;
                }
                OnPropertyChanged("Red");
                OnPropertyChanged("Green");
                OnPropertyChanged("Blue");
                OnPropertyChanged("BackgroundColorSample");
            }
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

    public enum BackgroundType
    {
        Solid,
        Grid,
        Image
    }
}
