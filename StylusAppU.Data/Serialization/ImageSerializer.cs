using Microsoft.Graphics.Canvas;
using StylusAppU.Data.Data;

namespace StylusAppU.Data.Serialization
{
    public static class ImageSerializer
    {
        public static void CreateImage(Page page)
        {
            var device = CanvasDevice.GetSharedDevice();

            using (var target = new CanvasRenderTarget(device, (float)page.Width, (float)page.Height, 96.0f))
            {
                using (var session = target.CreateDrawingSession())
                {
                    session.Units = CanvasUnits.Pixels;
                    session.DrawInk(page.StrokeContainer.GetStrokes());
                }
            }
        }
    }
}
