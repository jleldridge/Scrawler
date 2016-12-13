using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using StylusAppU.ViewModel;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.Foundation;

namespace StylusAppU.View
{
    public sealed partial class PageView : UserControl
    {
        public PageView()
        {
            this.InitializeComponent();
            InkCanvasElement.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            DataContextChanged += PageView_DataContextChanged;
        }

        private void PageView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs e)
        {
            var pageViewModel = e.NewValue as PageViewModel;
            if (pageViewModel != null)
            {
                InkCanvasElement.InkPresenter.StrokeContainer = pageViewModel.StrokeContainer;
            }
        }

        public void ApplyPenOptions(PenOptionsViewModel options)
        {
            var drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = new Color()
            {
                A = 255,
                R = (byte)options.Red,
                G = (byte)options.Green,
                B = (byte)options.Blue
            };
            drawingAttributes.Size = new Size(options.PenSize, options.PenSize);

            InkCanvasElement.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }
    }
}
