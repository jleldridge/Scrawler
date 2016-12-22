using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace Scrawler.Renderers
{
    public static class InkRenderer
    {
        public static CanvasBitmap RenderInk(IEnumerable<InkStroke> strokes, double width, double height)
        {
            var device = CanvasDevice.GetSharedDevice();
            var bitmap = new CanvasRenderTarget(device, (float)width, (float)height, 96.0f);
            using (var session = bitmap.CreateDrawingSession())
            {
                session.Units = CanvasUnits.Pixels;
                session.DrawInk(strokes);
            }

            return bitmap;
        }
    }
}
