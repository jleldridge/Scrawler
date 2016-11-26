using System;
using StylusAppU.Data.Data;
using StylusAppU.Data.Serialization;
using System.Collections.ObjectModel;
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

        private RelayCommand _prevPageCommand;
        private RelayCommand _nextPageCommand;

        public NotebookViewModel(Notebook notebook)
        {
            _notebook = notebook;
            Pages = new ObservableCollection<PageViewModel>();
            foreach (var page in _notebook.Pages)
            {
                Pages.Add(new PageViewModel(page, _notebookSerializer));
            }

            _notebookSerializer = new NotebookSerializer(notebook);
            _notebookSerializer.InitializeLocalNotebookFolder();
        }

        public NotebookViewModel(NotebookSerializer notebookSerializer)
        {
            _notebookSerializer = notebookSerializer;
            _notebook = notebookSerializer.Notebook;
            Pages = new ObservableCollection<PageViewModel>();
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
                _currentPageNumber = value - 1;
                OnPropertyChanged();
                OnPropertyChanged("CurrentPage");
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

        public async Task LoadPages()
        {
            foreach (var page in _notebook.Pages)
            {
                Pages.Add(new PageViewModel(page, _notebookSerializer));
            }

            foreach (var page in Pages)
            {
                await page.LoadStrokes();
            }
        }

        public void CreateNewPage()
        {
            _notebook.AddPage();
            Pages.Add(new PageViewModel(_notebook.Pages.Last(), _notebookSerializer));
            CurrentPageNumber = _notebook.Pages.Count;
        }

        public async Task SaveNotebook()
        {
            await _notebookSerializer.SaveNotebook();
            for (int i = 0; i < Pages.Count; i++)
            {
                var strokeContainer = Pages[i].StrokeContainer;
                if (strokeContainer.GetStrokes().Any())
                {
                    var page = _notebook.Pages[i];
                    await _notebookSerializer.SavePage(page, strokeContainer);
                }
            }
        }
    }
}
