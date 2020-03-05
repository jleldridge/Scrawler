using System;
using Microsoft.Graphics.Canvas;
using Scrawler.ViewModel;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Scrawler.View
{
    public sealed partial class PageOptionsDialog : ContentDialog
    {
        public PageOptionsDialog(PageOptionsViewModel viewModel)
        {
            DataContext = viewModel;
            this.InitializeComponent();
        }

        private void ContentDialog_OkButtonClick(ContentDialog sender, 
            ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, 
            ContentDialogButtonClickEventArgs args)
        {
        }

        private async void OpenImageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var viewModel = DataContext as PageOptionsViewModel;
            if (viewModel == null) return;

            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            // Ensure the stream is disposed once the image is loaded
            using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                var device = CanvasDevice.GetSharedDevice();
                var bitmapImage = await CanvasBitmap.LoadAsync(device, fileStream);
                viewModel.LoadImageForBackground(bitmapImage);
            }
        }
    }
}
