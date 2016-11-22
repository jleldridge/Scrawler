using StylusAppU.Data.Data;
using System.Windows.Input;
using Utils.Commands;
using Utils.ViewModel;

namespace StylusAppU.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _saveCommand, _loadCommand, _createNotebookCommand;
        private NotebookViewModel _currentNotebook;

        public MainViewModel()
        {
            CreateNewNotebook();
        }

        public NotebookViewModel CurrentNotebook
        {
            get { return _currentNotebook; }
            set
            {
                _currentNotebook = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveNotebook)); } }

        public ICommand LoadCommand { get { return _loadCommand ?? (_loadCommand = new RelayCommand(LoadNotebook)); } }

        public ICommand CreateNotebookCommand
        {
            get
            {
                return _createNotebookCommand 
                    ?? (_createNotebookCommand = new RelayCommand(_ => CreateNewNotebook()));
            }
        }

        private void CreateNewNotebook()
        {
            var notebook = new Notebook("NewNotebook");
            notebook.AddPage();
            CurrentNotebook = new NotebookViewModel(notebook);
        }

        private async void SaveNotebook(object commandArg)
        {
        }

        public async void LoadNotebook(object commandParam)
        {
        }
    }
}
