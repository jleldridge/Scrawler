using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Scrawler.ViewModel;
using Scrawler.Renderers;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Scrawler.Controls
{
    public class BackgroundControl : Canvas
    {
        private Image _backgroundImage;

        public BackgroundControl()
        {
            _backgroundImage = new Image();
            Children.Add(_backgroundImage);
            SizeChanged += GridLineBackgroundControl_SizeChanged;
        }

        private void GridLineBackgroundControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawChildren();
        }

        public static readonly DependencyProperty BackgroundViewModelProperty = DependencyProperty.Register(
            "BackgroundViewModel", 
            typeof (BackgroundViewModelBase), 
            typeof (BackgroundControl), 
            new PropertyMetadata(default(BackgroundViewModelBase), BackgroundViewModelChanged));

        public BackgroundViewModelBase BackgroundViewModel
        {
            get { return (BackgroundViewModelBase) GetValue(BackgroundViewModelProperty); }
            set { SetValue(BackgroundViewModelProperty, value); }
        }

        private static async void BackgroundViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = ((BackgroundControl)d);
            control.BackgroundViewModel.PropertyChanged += control.BackgroundViewModelOnPropertyChanged;
            await control.RedrawChildren();
        }

        private async void BackgroundViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await RedrawChildren();
        }

        private async Task RedrawChildren()
        {
            var backgroundImage = BackgroundRenderer.RenderBackground(BackgroundViewModel.BackgroundData, Width, Height);

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
