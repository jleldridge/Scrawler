using System;
using StylusAppU.Data.Data;
using Utils.ViewModel;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;
using StylusAppU.Data.Serialization;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using StylusAppU.DialogViewModels;

namespace StylusAppU.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        private Page _page;
        private NotebookSerializer _notebookSerializer;
        private BackgroundViewModelBase _backgroundViewModel;

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
            await ImageSerializer.CreateImage(_page, file);
        }
    }
}
