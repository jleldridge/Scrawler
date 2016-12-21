using System;
using StylusAppU.Data.Data;
using System.Windows.Input;
using Windows.Storage;
using StylusAppU.Data.Serialization;
using Utils.Commands;
using Utils.ViewModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;
using StylusAppU.DialogViewModels;
using StylusAppU.Dialogs;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;

namespace StylusAppU.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _saveCommand, 
            _loadCommand, 
            _createNotebookCommand, 
            _showNotebookOptionsCommand,
            _zoomOutCommand,
            _zoomInCommand,
            _createPageImageCommand;
        private NotebookViewModel _currentNotebook;
        private bool _isSaving;

        public MainViewModel()
        {
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

        public bool IsSaving
        {
            get { return _isSaving; }
            set
            {
                _isSaving = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get { return _saveCommand ?? (_saveCommand = new RelayCommand(_ => SaveNotebook())); } }

        public ICommand LoadCommand { get { return _loadCommand ?? (_loadCommand = new RelayCommand(_ => LoadNotebook())); } }

        public ICommand CreateNotebookCommand
        {
            get
            {
                return _createNotebookCommand 
                    ?? (_createNotebookCommand = new RelayCommand(_ => CreateNewNotebook()));
            }
        }

        public ICommand ShowPageOptionsCommand
        {
            get { return _showNotebookOptionsCommand ?? (_showNotebookOptionsCommand = new RelayCommand(_ => ShowNotebookOptions())); }
        }

        public ICommand ZoomOutCommand
        {
            get { return _zoomOutCommand ?? (_zoomOutCommand = new RelayCommand(_ => ZoomOut())); }
        }

        public ICommand ZoomInCommand
        {
            get { return _zoomInCommand ?? (_zoomInCommand = new RelayCommand(_ => ZoomIn())); }
        }

        public ICommand CreatePageImageCommand
        {
            get { return _createPageImageCommand ?? (_createPageImageCommand = new RelayCommand(_ => CreatePageImage())); }
        }

        private async Task CreatePageImage()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Bitmap file", new List<string>() { ".bmp" }));
            picker.SuggestedFileName = "PageImage";
            var file = await picker.PickSaveFileAsync();

            await CurrentNotebook.CurrentPage.CreatePageImage(file);
        }

        private void CreateNewNotebook()
        {
            var notebook = new Notebook("NewNotebook");
            notebook.AddPage();
            var notebookSerializer = new NotebookSerializer(notebook);
            CurrentNotebook = new NotebookViewModel(notebookSerializer);
        }

        private async Task SaveNotebook()
        {
            IsSaving = true;
            await CurrentNotebook.SaveNotebook();
            IsSaving = false;
        }

        public async Task LoadNotebook()
        {
            //var notebookGuid = ApplicationData.Current.LocalSettings.Values[NotebookSerializer.CurrentNotebookKey] as string;
            var notebookSerializer = new NotebookSerializer();

            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".note");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                await notebookSerializer.LoadNotebookArchive(file);
                CurrentNotebook = new NotebookViewModel(notebookSerializer);
            }
        }

        public async Task ShowNotebookOptions()
        {
            PageOptionsViewModel options = new PageOptionsViewModel(CurrentNotebook.CurrentPage);
            var dlg = new PageOptionsDialog(options);
            var result = await dlg.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CurrentNotebook.CurrentPage.Width = options.Width;
                CurrentNotebook.CurrentPage.Height = options.Height;
                CurrentNotebook.CurrentPage.BackgroundViewModel = options.BackgroundDataViewModel;

                CurrentNotebook.Defaults = new Defaults()
                {
                    Background = options.BackgroundDataViewModel.BackgroundData,
                    PageWidth = options.Width,
                    PageHeight = options.Height
                };
            }
        }

        public void ZoomOut()
        {
            if (CurrentNotebook != null)
            {
                CurrentNotebook.Zoom -= 0.1f;
            }
        }

        public void ZoomIn()
        {
            if (CurrentNotebook != null)
            {
                CurrentNotebook.Zoom += 0.1f;
            }
        }
    }
}
