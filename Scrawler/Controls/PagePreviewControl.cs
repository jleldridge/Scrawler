using Microsoft.Graphics.Canvas.UI.Xaml;
using Scrawler.ViewModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Scrawler.Controls
{
    public class PagePreviewControl : Canvas
    {
        private BackgroundControl _background;
        private CanvasControl _ink;

        public PagePreviewControl()
        {
            Background = new SolidColorBrush(Colors.White);
            _background = new BackgroundControl();
            _ink = new CanvasControl();

            Children.Add(_background);
            Children.Add(_ink);

            Loaded += PagePreview_Loaded;
            _ink.Draw += DrawInk;
        }

        private void DrawInk(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var pageVm = DataContext as PageViewModel;
            if (pageVm != null)
            {
                args.DrawingSession.DrawInk(pageVm.StrokeContainer.GetStrokes());
            }
        }

        private void PagePreview_Loaded(object sender, RoutedEventArgs e)
        {
            var pageVm = DataContext as PageViewModel;
            if (pageVm != null)
            {
                _background.Width = pageVm.Width;
                _background.Height = pageVm.Height;
                _background.BackgroundViewModel = pageVm.BackgroundViewModel;
                _ink.Width = pageVm.Width;
                _ink.Height = pageVm.Height;
                _ink.Invalidate();
            }
        }
    }
}
