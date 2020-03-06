using System;
using Scrawler.Data.Data;
using System.Windows.Input;
using Windows.Storage;
using Scrawler.Data.Serialization;
using Utils.Commands;
using Utils.ViewModel;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;
using Scrawler.ViewModel;
using Scrawler.View;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Scrawler.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private List<SplitMenuCommandViewModel> _menuCommands;
        private NotebookViewModel _currentNotebook;
        private bool _isSaving, _menuExpanded;

        public MainViewModel()
        {
            _menuCommands = new List<SplitMenuCommandViewModel>()
            {
                new SplitMenuCommandViewModel("New Notebook", "ms-appx:///Assets/new_notebook.bmp", _ => CreateNewNotebook()),
                new SplitMenuCommandViewModel("Open Notebook", "ms-appx:///Assets/open_notebook.bmp", _ => LoadNotebook()),
                new SplitMenuCommandViewModel("Save Notebook", "ms-appx:///Assets/save_notebook.bmp", _ => SaveNotebook(), true),
                new SplitMenuCommandViewModel("Unsaved Changes", "ms-appx:///Assets/unsaved_changes.bmp", _ => SaveNotebook(), false),
                new SplitMenuCommandViewModel("Page Options", "ms-appx:///Assets/page_options.bmp", _ => ShowNotebookOptions()),
                new SplitMenuCommandViewModel("Create Page Image", "ms-appx:///Assets/create_page_image.bmp", _ => CreatePageImage()),
            };
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

        public bool MenuExpanded
        {
            get { return _menuExpanded; }
            set
            {
                _menuExpanded = value;
                foreach (var item in MenuCommands)
                {
                    item.IsExpanded = value;
                }
                OnPropertyChanged();
            }
        }

        public List<SplitMenuCommandViewModel> MenuCommands
        {
            get { return _menuCommands; }
        }

        private async Task CreatePageImage()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Bitmap file", new List<string>() { ".bmp" }));
            picker.SuggestedFileName = "PageImage";
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await CurrentNotebook.CurrentPage.CreatePageImage(file);
            }
        }

        private void CreateNewNotebook()
        {
            var notebook = new Notebook("NewNotebook");
            notebook.AddPage();
            CurrentNotebook = new NotebookViewModel(notebook, null);
            CurrentNotebook.PropertyChanged += CurrentNotebook_PropertyChanged;
        }

        private void CurrentNotebook_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UnsavedChanges":
                    if (CurrentNotebook.UnsavedChanges)
                    {
                        _menuCommands.First(m => m.Label == "Save Notebook").Visible = false;
                        _menuCommands.First(m => m.Label == "Unsaved Changes").Visible = true;
                    }
                    else
                    {
                        _menuCommands.First(m => m.Label == "Save Notebook").Visible = true;
                        _menuCommands.First(m => m.Label == "Unsaved Changes").Visible = false;
                    }
                    break;
            }
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
                var notebook = await NotebookSerializer.LoadNotebookArchive(file);
                CurrentNotebook = new NotebookViewModel(notebook, file);
                CurrentNotebook.PropertyChanged += CurrentNotebook_PropertyChanged;
            }
        }

        public async Task ShowNotebookOptions()
        {
            PageOptionsViewModel options = new PageOptionsViewModel(CurrentNotebook.CurrentPage, CurrentNotebook);
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
