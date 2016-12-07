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
        private InkStrokeContainer _strokeContainer;
        private NotebookSerializer _notebookSerializer;

        public PageViewModel(Page page, NotebookSerializer notebookSerializer)
        {
            _page = page;
            _notebookSerializer = notebookSerializer;
            //LoadBackground();
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
            get { return _strokeContainer; }
            set
            {
                _strokeContainer = value;
                OnPropertyChanged();
            }
        }

        public async Task Initialize()
        {
            if (!string.IsNullOrWhiteSpace(_page.InkFileName))
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(_page.InkFileName);
                if (file != null)
                {
                    StrokeContainer = await _notebookSerializer.LoadPage(_page);
                }
                else
                {
                    StrokeContainer = new InkStrokeContainer();
                }
            }
        }
    }
}
