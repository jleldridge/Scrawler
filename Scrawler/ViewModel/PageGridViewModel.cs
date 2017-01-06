using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils.ViewModel;

namespace Scrawler.ViewModel
{
    public class PageGridViewModel : ViewModelBase
    {
        private ObservableCollection<PageGridPageViewModel> _pages;
        private int _selectedPageIndex;

        public PageGridViewModel(IEnumerable<PageViewModel> pages, int pageIndex)
        {
            _pages = new ObservableCollection<PageGridPageViewModel>();
            foreach (var page in pages)
            {
                _pages.Add(new PageGridPageViewModel(page));
            }
            _selectedPageIndex = pageIndex;
        }

        public ObservableCollection<PageGridPageViewModel> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                OnPropertyChanged();
            }
        }

        public int SelectedPageIndex
        {
            get { return _selectedPageIndex; }
            set
            {
                _selectedPageIndex = value;
                OnPropertyChanged();
            }
        }
    }

    public class PageGridPageViewModel : ViewModelBase
    {
        private double _width, _height;

        public PageGridPageViewModel(PageViewModel pageVm)
        {
            PageViewModel = pageVm;
        }

        public PageViewModel PageViewModel { get; private set; }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
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
    }
}