using Scrawler.Renderers;
using Scrawler.ViewModel;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Scrawler.Controls
{
    public class PagePreviewControl : Canvas
    {
        private BackgroundControl _background;
        private Image _ink;

        public PagePreviewControl()
        {
            Background = new SolidColorBrush(Colors.White);
            _background = new BackgroundControl();
            _ink = new Image();

            Children.Add(_background);
            Children.Add(_ink);

            Loaded += PagePreview_Loaded;
        }

        private async void PagePreview_Loaded(object sender, RoutedEventArgs e)
        {
            var pageVm = DataContext as PageViewModel;
            if (pageVm != null)
            {
                _background.Width = pageVm.Width;
                _background.Height = pageVm.Height;
                _background.BackgroundViewModel = pageVm.BackgroundViewModel;
                await DrawInk(pageVm);
            }
        }

        private async Task DrawInk(PageViewModel pageVm)
        {
            var bitmap = InkRenderer.RenderInk(pageVm.StrokeContainer.GetStrokes(), pageVm.Width, pageVm.Height);
            var outputBitmap = new SoftwareBitmap(
                BitmapPixelFormat.Bgra8,
                (int)bitmap.SizeInPixels.Width,
                (int)bitmap.SizeInPixels.Height,
                BitmapAlphaMode.Premultiplied);
            outputBitmap.CopyFromBuffer(bitmap.GetPixelBytes().AsBuffer());

            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(outputBitmap);
            _ink.Source = source;
        }
    }
}
