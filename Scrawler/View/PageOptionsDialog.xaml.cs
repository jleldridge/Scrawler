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

        private void OpenImageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var viewModel = DataContext as PageOptionsViewModel;
            if (viewModel != null)
            {
                viewModel.LoadImageForBackground();
            }
        }
    }
}
