using System;
using Scrawler.Data.Data;
using Utils.ViewModel;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;
using Scrawler.Data.Serialization;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Scrawler.ViewModel;
using Scrawler.Renderers;
using Windows.Graphics.Imaging;

namespace Scrawler.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        private Page _page;
        private NotebookSerializer _notebookSerializer;
        private BackgroundViewModelBase _backgroundViewModel;
        private bool _unsavedChanges;

        public PageViewModel(Page page, NotebookSerializer notebookSerializer)
        {
            _page = page;
            _notebookSerializer = notebookSerializer;
            if (_page.Background is SolidBackground)
            {
                _backgroundViewModel = new SolidBackgroundViewModel((SolidBackground)_page.Background);
            }
            else if (_page.Background is GridLineBackground)
            {
                _backgroundViewModel = new GridLineBackgroundViewModel((GridLineBackground)_page.Background);
            }
            else if (_page.Background is ImageBackground)
            {
                _backgroundViewModel = new ImageBackgroundViewModel((ImageBackground)_page.Background);
            }
            //LoadBackground();
        }

        public double Width
        {
            get { return _page.Width; }
            set
            {
                _page.Width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return _page.Height; }
            set
            {
                _page.Height = value;
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

        public BackgroundViewModelBase BackgroundViewModel
        {
            get { return _backgroundViewModel; }
            set
            {
                _backgroundViewModel = value;
                _page.Background = _backgroundViewModel.BackgroundData;
                OnPropertyChanged();
            }
        }

        public InkStrokeContainer StrokeContainer
        {
            get { return _page.StrokeContainer; }
            set
            {
                _page.StrokeContainer = value;
                OnPropertyChanged();
            }
        }

        internal async Task CreatePageImage(StorageFile file)
        {
            var backgroundBitmap = BackgroundRenderer.RenderBackground(BackgroundViewModel.BackgroundData, Width, Height);
            var inkBitmap = InkRenderer.RenderInk(StrokeContainer.GetStrokes(), Width, Height);

            await ImageSerializer.SaveImage(backgroundBitmap, inkBitmap, file);
        }
    }
}
