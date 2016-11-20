using StylusAppU.Data.Data;
using Utils.ViewModel;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Media.Imaging;

namespace StylusAppU.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        private Page _page;
        private BitmapImage _backgroundImage;
        private InkStrokeContainer _strokeContainer;

        public PageViewModel(Page page)
        {
            _page = page;
            LoadStrokes();
            LoadBackground();
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

        public async void LoadStrokes()
        {   
        }

        public async void SaveStrokes()
        {
        }

        public void LoadBackground()
        {
        }
    }
}
