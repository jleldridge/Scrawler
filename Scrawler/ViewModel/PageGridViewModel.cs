using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils.ViewModel;

namespace Scrawler.ViewModel
{
    public class PageGridViewModel : ViewModelBase
    {
        private ObservableCollection<PageViewModel> _pages;

        public PageGridViewModel(IEnumerable<PageViewModel> pages)
        {
            _pages = new ObservableCollection<PageViewModel>(pages);
        }

        public ObservableCollection<PageViewModel> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                OnPropertyChanged();
            }
        }
    }
}