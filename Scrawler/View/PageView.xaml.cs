using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Scrawler.ViewModel;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Scrawler.View
{
    public sealed partial class PageView : UserControl
    {
        private InkSynchronizer _inkSynchronizer;

        public PageView()
        {
            this.InitializeComponent();
            InkCanvasElement.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            _inkSynchronizer = InkCanvasElement.InkPresenter.ActivateCustomDrying();
            InkCanvasElement.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;

            // this allows eraser strokes to pass through as unprocessed input.
            InkCanvasElement.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
            InkCanvasElement.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            InkCanvasElement.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            InkCanvasElement.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;

            DrawingCanvasElement.Draw += DrawingCanvas_OnDraw;
            DataContextChanged += PageView_DataContextChanged;
        }

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.IsEraser && args.CurrentPoint.IsInContact)
            {
                var point = args.CurrentPoint.Position;
                var eraseRect = new Rect(new Point(point.X - 3, point.Y - 3), new Point(point.X + 3, point.Y + 3));
                foreach (var stroke in ViewModel.StrokeContainer.GetStrokes())
                {
                    if (RectHelper.Intersect(stroke.BoundingRect, eraseRect) != Rect.Empty)
                    {
                        stroke.Selected = true;
                    }
                }

                ViewModel.StrokeContainer.DeleteSelected();
                DrawingCanvasElement.Invalidate();
            }
        }

        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {
        }

        public PageViewModel ViewModel
        {
            get { return DataContext as PageViewModel; }
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            var strokes = _inkSynchronizer.BeginDry();
            ViewModel.StrokeContainer.AddStrokes(strokes);
            _inkSynchronizer.EndDry();

            DrawingCanvasElement.Invalidate();
        }

        private void DrawingCanvas_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawInk(ViewModel.StrokeContainer.GetStrokes());
        }

        private void PageView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs e)
        {
            DrawingCanvasElement.Invalidate();
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
