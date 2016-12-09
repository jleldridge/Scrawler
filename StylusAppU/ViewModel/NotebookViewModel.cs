using System;
using System.Collections.Generic;
using StylusAppU.Data.Data;
using StylusAppU.Data.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Utils.Commands;
using Utils.ViewModel;
using System.Threading.Tasks;

namespace StylusAppU.ViewModel
{
    public class NotebookViewModel : ViewModelBase
    {
        private Notebook _notebook;
        private int _currentPageNumber;
        private NotebookSerializer _notebookSerializer;
        private float _zoom = 1.0f;

        private RelayCommand _prevPageCommand;
        private RelayCommand _nextPageCommand;

        public NotebookViewModel(NotebookSerializer notebookSerializer)
        {
            _notebookSerializer = notebookSerializer;
            _notebook = notebookSerializer.Notebook;
            Pages = new ObservableCollection<PageViewModel>();
            foreach (var page in _notebook.Pages)
            {
                Pages.Add(new PageViewModel(page, _notebookSerializer));
            }
        }

        public Guid NotebookGuid
        {
            get { return _notebook.Guid; }
        }

        public string Name
        {
            get { return _notebook.Name; }
            set
            {
                _notebook.Name = value;
                OnPropertyChanged();
            }
        }

        public PageViewModel CurrentPage
        {
            // be sure to use field for indexing, since it is 0 based
            // and property is 1 based.
            get { return Pages[_currentPageNumber]; }
        }

        public ObservableCollection<PageViewModel> Pages { get; private set; }

        // expose the property as 1 based for display, but keep the field zero-based for indexing.
        public int CurrentPageNumber
        {
            get { return _currentPageNumber + 1; }
            set
            {
                if (value > Pages.Count)
                {
                    _currentPageNumber = Pages.Count - 1;
                }
                else if (value <= 0)
                {
                    _currentPageNumber = 0;
                }
                else
                {
                    _currentPageNumber = value - 1;
                }
                OnPropertyChanged();
                OnPropertyChanged("CurrentPage");
            }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand PreviousPageCommand
        {
            get
            {
                return _prevPageCommand ?? (_prevPageCommand = new RelayCommand(
                _ =>
                {
                    if (_currentPageNumber > 0)
                    {
                        CurrentPageNumber--;
                    }
                }));
            }
        }

        public RelayCommand NextPageCommand
        {
            get
            {
                return _nextPageCommand ?? (_nextPageCommand = new RelayCommand(
                    _ => 
                    {
                        if (_currentPageNumber < _notebook.Pages.Count - 1)
                        {
                            CurrentPageNumber++;
                        }
                        else if (_currentPageNumber == _notebook.Pages.Count - 1)
                        {
                            CreateNewPage();
                        }
                    }));
            }
        }

        public void CreateNewPage()
        {
            _notebook.AddPage();
            var page = new PageViewModel(_notebook.Pages.Last(), _notebookSerializer);
            Pages.Add(page);
            CurrentPageNumber = _notebook.Pages.Count;
        }

        public async Task SaveNotebookAs()
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Notebook file", new List<string>() {".note"}));
            picker.SuggestedFileName = "Notebook";
            var file = await picker.PickSaveFileAsync();

            _notebookSerializer.InitializeNotebookArchive(file);
        }

        public async Task SaveNotebook()
        {
            if (_notebookSerializer.NotebookArchiveFile == null)
            {
                await SaveNotebookAs();
            }
            await _notebookSerializer.SaveNotebook();
        }
    }
}
