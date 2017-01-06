using Scrawler.Renderers;
using Scrawler.ViewModel;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Scrawler.Controls
{
    public class PagePreviewControl : Canvas
    {
        private BackgroundControl _background;
        private InkCanvas _ink;

        public PagePreviewControl()
        {
            Background = new SolidColorBrush(Colors.White);
            _background = new BackgroundControl();
            _ink = new InkCanvas();
            _ink.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.None;

            Children.Add(_background);
            Children.Add(_ink);

            Loaded += PagePreview_Loaded;
        }

        private void PagePreview_Loaded(object sender, RoutedEventArgs e)
        {
            var pageVm = DataContext as PageViewModel;
            if (pageVm != null)
            {
                _background.Width = pageVm.Width;
                _background.Height = pageVm.Height;
                _background.BackgroundViewModel = pageVm.BackgroundViewModel;
                DrawInk(pageVm);
            }
        }

        private void DrawInk(PageViewModel pageVm)
        {
            _ink.Height = pageVm.Height;
            _ink.Width = pageVm.Width;
            _ink.InkPresenter.StrokeContainer = pageVm.StrokeContainer;
        }
    }
}
