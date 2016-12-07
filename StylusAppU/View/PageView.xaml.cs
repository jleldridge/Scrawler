using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using StylusAppU.ViewModel;

namespace StylusAppU.View
{
    public sealed partial class PageView : UserControl
    {
        public PageView()
        {
            this.InitializeComponent();
            Canvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            DataContextChanged += PageView_DataContextChanged;
        }

        private async void PageView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs e)
        {
            var pageViewModel = e.NewValue as PageViewModel;
            if (pageViewModel != null)
            {
                if (pageViewModel.StrokeContainer == null)
                {
                    await pageViewModel.Initialize();
                }
                Canvas.InkPresenter.StrokeContainer = pageViewModel.StrokeContainer;
            }
        }
    }
}
