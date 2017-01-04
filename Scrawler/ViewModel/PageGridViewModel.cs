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

    public class PageGridPageViewModel : ViewModelBase
    {
        private double _scaledWidth, _scaledHeight;

        public PageGridPageViewModel(PageViewModel pageVm)
        {
            PageViewModel = pageVm;
        }

        public PageViewModel PageViewModel { get; private set; }

        public double ScaledWidth
        {
            get { return _scaledWidth; }
            set
            {
                _scaledWidth = value;
                OnPropertyChanged();
            }
        }

        public double ScaledHeight
        {
            get { return _scaledHeight; }
            set
            {
                _scaledHeight = value;
                OnPropertyChanged();
            }
        }
    }
}