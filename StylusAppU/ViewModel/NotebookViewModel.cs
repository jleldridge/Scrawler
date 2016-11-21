using StylusAppU.Data.Data;
using Utils.ViewModel;

namespace StylusAppU.ViewModel
{
    public class NotebookViewModel : ViewModelBase
    {
        private Notebook _notebook;
        private int _currentPageNumber;

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
    }
}
