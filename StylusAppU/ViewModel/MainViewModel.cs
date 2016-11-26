using StylusAppU.Data.Data;
using System.Windows.Input;
using Windows.Storage;
using StylusAppU.Data.Serialization;
using Utils.Commands;
using Utils.ViewModel;
using System.Threading.Tasks;

namespace StylusAppU.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _saveCommand, _loadCommand, _createNotebookCommand;
        private NotebookViewModel _currentNotebook;

        public MainViewModel()
        {
            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey(NotebookSerializer.CurrentNotebookKey))
            //{
            //    var notebookGuid = ApplicationData.Current.LocalSettings.Values[NotebookSerializer.CurrentNotebookKey] as string;
            //    var notebookSerializer = new NotebookSerializer();
            //    notebookSerializer.LoadLocalNotebookFolder(notebookGuid);
            //    CurrentNotebook = new NotebookViewModel(notebookSerializer);
            //}
            //else
            //{
                //CreateNewNotebook();
                //ApplicationData.Current.LocalSettings.Values.Add(NotebookSerializer.CurrentNotebookKey, CurrentNotebook.NotebookGuid.ToString());
            //}
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

        public ICommand SaveCommand { get { return _saveCommand ?? (_saveCommand = new RelayCommand(_ => SaveNotebook())); } }

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

        private async void SaveNotebook()
        {
            await CurrentNotebook.SaveNotebook();
        }

        public async void LoadNotebook(object commandParam)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(NotebookSerializer.CurrentNotebookKey))
            {
                var notebookGuid = ApplicationData.Current.LocalSettings.Values[NotebookSerializer.CurrentNotebookKey] as string;
                var notebookSerializer = new NotebookSerializer();
                notebookSerializer.LoadLocalNotebookFolder(notebookGuid);
                CurrentNotebook = new NotebookViewModel(notebookSerializer);
            }
        }
    }
}
