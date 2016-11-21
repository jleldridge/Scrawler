using StylusAppU.Data.Data;
using Utils.Commands;
using Utils.ViewModel;

namespace StylusAppU.ViewModel
{
    public class NotebookViewModel : ViewModelBase
    {
        private Notebook _notebook;
        private int _currentPageNumber;

        private RelayCommand _prevPageCommand;
        private RelayCommand _nextPageCommand;

        public NotebookViewModel(Notebook notebook)
        {
            _notebook = notebook;
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
            get { return new PageViewModel(_notebook.Pages[_currentPageNumber]); }
        }

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

        public void CreateNewPage()
        {
            _notebook.AddPage();
            CurrentPageNumber = _notebook.Pages.Count;
        }
    }
}
