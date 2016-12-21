using System;
using Utils.Commands;
using Utils.ViewModel;

namespace Scrawler.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        private RelayCommand _relayCommand;
        private string _label;

        public CommandViewModel(string label, Action<object> execute)
        {
            _label = label;
            _relayCommand = new RelayCommand(execute);
        }

        public RelayCommand RelayCommand
        {
            get { return _relayCommand; }
            set
            {
                _relayCommand = value;
                OnPropertyChanged();
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }
    }
}
