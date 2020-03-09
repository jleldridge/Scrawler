using System;
using System.Collections.Generic;
using Scrawler.Data.Data;
using Scrawler.Data.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Utils.Commands;
using Utils.ViewModel;
using System.Threading.Tasks;
using Windows.UI;
using System.ComponentModel;
using Windows.Storage;

namespace Scrawler.ViewModel
{
    public class NotebookViewModel : ViewModelBase
    {
        public static readonly float ZoomMax = 5.0f;
        public static readonly float ZoomMin = 0.1f;

        private Notebook _notebook;
        private int _currentPageNumber;
        private float _zoom = 1f;
        private PenOptionsViewModel _penOptionsViewModel;
        private PageGridViewModel _pageGridViewModel;
        private bool _pageGridVisible;
        private bool _unsavedChanges;
        private StorageFile _file;

        private RelayCommand _prevPageCommand, 
            _nextPageCommand,
            _showPageGridCommand;

        public NotebookViewModel(Notebook notebook, StorageFile file)
        {
            _notebook = notebook;
            _file = file;
            Pages = new ObservableCollection<PageViewModel>();
            foreach (var page in _notebook.Pages)
            {
                var pageVm = new PageViewModel(page);
                pageVm.PropertyChanged += PageVm_PropertyChanged;
                Pages.Add(pageVm);
            }

            PenOptionsViewModel = new PenOptionsViewModel(this);
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

        public Defaults Defaults
        {
            get { return _notebook.Defaults; }
            set
            {
                _notebook.Defaults = value;
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
                if (value > ZoomMax) _zoom = ZoomMax;
                else if (value < ZoomMin) _zoom = ZoomMin;
                else _zoom = value;

                OnPropertyChanged();
            }
        }

        public List<Color> SavedColors
        {
            get { return _notebook.SavedColors; }
            set
            {
                _notebook.SavedColors = value;
                OnPropertyChanged();
            }
        }

        public List<BackgroundBase> SavedPageBackgrounds
        {
            get { return _notebook.SavedPageBackgrounds; }
            set 
            {
                _notebook.SavedPageBackgrounds = value;
                OnPropertyChanged();
            }
        }

        public PenOptionsViewModel PenOptionsViewModel
        {
            get { return _penOptionsViewModel; }
            set
            {
                _penOptionsViewModel = value;
                OnPropertyChanged();
            }
        }

        public PageGridViewModel PageGridViewModel
        {
            get { return _pageGridViewModel; }
            set
            {
                _pageGridViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool PageGridVisible
        {
            get { return _pageGridVisible; }
            set
            {
                _pageGridVisible = value;
                OnPropertyChanged();
            }
        }

        public bool UnsavedChanges
        {
            get { return _unsavedChanges; }
            set
            {
                _unsavedChanges = value;
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

        public RelayCommand ShowPageGridCommand
        {
            get { return _showPageGridCommand ?? (_showPageGridCommand = new RelayCommand(_ => ShowPageGrid())); }
        }

        public void ShowPageGrid()
        {
            PageGridVisible = !PageGridVisible;
            if (PageGridVisible)
            {
                PageGridViewModel = new PageGridViewModel(Pages, CurrentPageNumber - 1);
                PageGridViewModel.PropertyChanged += PageGridViewModel_PropertyChanged;
            }
        }

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var pageVm = sender as PageViewModel;
            if (pageVm == null) return;

            switch (e.PropertyName)
            {
                case "UnsavedChanges":
                    if (pageVm.UnsavedChanges)
                    {
                        UnsavedChanges = true;
                    }
                    break;
            }
        }

        private void PageGridViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedPageIndex":
                    CurrentPageNumber = PageGridViewModel.SelectedPageIndex + 1;
                    break;
            }
        }

        public void CreateNewPage()
        {
            _notebook.AddPage();
            var page = new PageViewModel(_notebook.Pages.Last());
            Pages.Add(page);
            CurrentPageNumber = _notebook.Pages.Count;
            UnsavedChanges = true;
        }

        public async Task SaveNotebook()
        {
            if (_file == null)
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Notebook file", new List<string>() { ".note" }));
                picker.SuggestedFileName = "Notebook";
                _file = await picker.PickSaveFileAsync();
            }
            await NotebookSerializer.SaveNotebook(_notebook, _file);
            UnsavedChanges = false;
            foreach(var page in Pages)
            {
                page.UnsavedChanges = false;
            }
        }
    }
}
