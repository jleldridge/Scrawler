using System;
using StylusAppU.Data.Data;
using Utils.ViewModel;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;
using StylusAppU.Data.Serialization;
using System.Threading.Tasks;

namespace StylusAppU.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        private Page _page;
        private BitmapImage _backgroundImage;
        private NotebookSerializer _notebookSerializer;

        public PageViewModel(Page page, NotebookSerializer notebookSerializer)
        {
            _page = page;
            _notebookSerializer = notebookSerializer;
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

        public BitmapImage BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                _backgroundImage = value;
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
    }
}
