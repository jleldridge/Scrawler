using Scrawler.ViewModel;
using Windows.UI.Xaml.Controls;

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
    }
}
