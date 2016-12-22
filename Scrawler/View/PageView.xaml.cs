using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Scrawler.ViewModel;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.Foundation;
using System.ComponentModel;
using System;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Scrawler.Renderers;

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
            DrawingCanvasElement.Draw += DrawingCanvas_OnDraw;
            DataContextChanged += PageView_DataContextChanged;
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
            //var pageViewModel = e.NewValue as PageViewModel;
            //if (pageViewModel != null)
            //{
            //    InkCanvasElement.InkPresenter.StrokeContainer = pageViewModel.StrokeContainer;
            //}
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
