using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Scrawler.Renderers;
using Scrawler.Data.Data;

namespace Scrawler.Controls
{
    public class SavedBackgroundThumbnail : Canvas
    {
        private Image _backgroundImage;

        public SavedBackgroundThumbnail()
        {
            _backgroundImage = new Image();
            Children.Add(_backgroundImage);
        }

        public static readonly DependencyProperty BackgroundDataProperty = DependencyProperty.Register(
            "BackgroundData",
            typeof(BackgroundBase),
            typeof(SavedBackgroundThumbnail),
            new PropertyMetadata(default(BackgroundBase), BackgroundDataChanged));

        public BackgroundBase BackgroundData
        {
            get { return (BackgroundBase)GetValue(BackgroundDataProperty); }
            set { SetValue(BackgroundDataProperty, value); }
        }

        private static async void BackgroundDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SavedBackgroundThumbnail)d;
            await control.RedrawChildren();
        }

        private async Task RedrawChildren()
        {
            var backgroundImage = BackgroundRenderer.RenderBackground(BackgroundData, Width, Height);

            var outputBitmap = new SoftwareBitmap(
                BitmapPixelFormat.Bgra8,
                (int)backgroundImage.SizeInPixels.Width,
                (int)backgroundImage.SizeInPixels.Height,
                BitmapAlphaMode.Premultiplied);
            outputBitmap.CopyFromBuffer(backgroundImage.GetPixelBytes().AsBuffer());

            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(outputBitmap);
            _backgroundImage.Source = source;
        }
    }
}
