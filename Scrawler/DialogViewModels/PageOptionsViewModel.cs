using Scrawler.Data.Data;
using Scrawler.Data.Serialization;
using Scrawler.ViewModel;
using System.Collections.Generic;
using Utils.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Scrawler.DialogViewModels
{
    public class PageOptionsViewModel : ViewModelBase
    {
        private double _height, _width;
        private BackgroundType _selectedType;
        private BackgroundViewModelBase _backgroundDataViewModel;

        public PageOptionsViewModel(PageViewModel page)
        {
            Width = page.Width;
            Height = page.Height;
            var backgroundData = DataContractHelper.Clone(page.BackgroundViewModel.BackgroundData);
            
            if (backgroundData is SolidBackground)
            {
                _backgroundDataViewModel = new SolidBackgroundViewModel(backgroundData as SolidBackground);
                _selectedType = BackgroundType.Solid;
            }
            else if (backgroundData is GridLineBackground)
            {
                _backgroundDataViewModel = new GridLineBackgroundViewModel(backgroundData as GridLineBackground);
                _selectedType = BackgroundType.Grid;
            }
            else if (backgroundData is ImageBackground)
            {
                _backgroundDataViewModel = new ImageBackgroundViewModel(backgroundData as ImageBackground);
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
                if (value != _selectedType)
                {    
                    _selectedType = value;
                    OnPropertyChanged();

                    switch (_selectedType)
                    {
                        case BackgroundType.Solid:
                            BackgroundDataViewModel = new SolidBackgroundViewModel(new SolidBackground());
                            break;
                        case BackgroundType.Grid:
                            BackgroundDataViewModel = new GridLineBackgroundViewModel(new GridLineBackground());
                            break;
                        case BackgroundType.Image:
                            BackgroundDataViewModel = new ImageBackgroundViewModel(new ImageBackground());
                            break;
                    }
                }
            }
        }

        public BackgroundViewModelBase BackgroundDataViewModel
        {
            get { return _backgroundDataViewModel; }
            set
            {
                _backgroundDataViewModel = value;
                OnPropertyChanged();
            }
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
    }

    public enum BackgroundType
    {
        Solid,
        Grid,
        Image
    }
}
