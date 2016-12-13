using System.Collections.Generic;
using System.Windows.Input;
using Utils.Commands;
using Utils.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StylusAppU.ViewModel
{
    public class PenOptionsViewModel : ViewModelBase
    {
        private double _red, _green, _blue;
        private NotebookViewModel _notebookViewModel;
        private ICommand _saveColorCommand, _setColorCommand;

        public PenOptionsViewModel(NotebookViewModel notebook)
        {
            _notebookViewModel = notebook;
        }

        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged();
                OnPropertyChanged("ColorSample");
            }
        }

        public double Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged();
                OnPropertyChanged("ColorSample");
            }
        }

        public double Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged();
                OnPropertyChanged("ColorSample");
            }
        }

        public SolidColorBrush ColorSample
        {
            get { return new SolidColorBrush(new Color() { A = 255, R = (byte)Red, G = (byte)Green, B = (byte)Blue }); }
        }

        public List<Color> SavedColors
        {
            get { return _notebookViewModel.SavedColors; }
            set
            {
                _notebookViewModel.SavedColors = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveColorCommand
        {
            get { return _saveColorCommand ?? (_saveColorCommand = new RelayCommand(_ => SaveColor())); }
        }

        public ICommand SetColorCommand
        {
            get { return _setColorCommand ?? (_setColorCommand = new RelayCommand(SetColor)); }
        }

        public void SetColor(object param)
        {
            var color = param as Color?;
            if (color.HasValue)
            {
                Red = color.Value.R;
                Green = color.Value.G;
                Blue = color.Value.B;
            }
        }

        public void SaveColor()
        {
            SavedColors.Add(new Color()
            {
                A = 255,
                R = (byte)Red,
                G = (byte)Green,
                B = (byte)Blue
            });
            SavedColors = new List<Color>(SavedColors);
        }
    }
}
