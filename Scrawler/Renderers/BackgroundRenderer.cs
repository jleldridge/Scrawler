using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Scrawler.Data.Data;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace Scrawler.Renderers
{
    public static class BackgroundRenderer
    {
        public static CanvasBitmap RenderBackground(BackgroundBase background, double width, double height)
        {
            var device = CanvasDevice.GetSharedDevice();
            var bitmap = new CanvasRenderTarget(device, (float)width, (float)height, 96.0f);
            using (var session = bitmap.CreateDrawingSession())
            {
                session.Units = CanvasUnits.Pixels;
                session.Clear(background.BackgroundColor);

                if (background is GridLineBackground)
                {
                    var glBackground = background as GridLineBackground;
                    DrawGridLineBackground(glBackground, bitmap, session);
                }

                if (background is ImageBackground)
                {
                    var imageBackground = background as ImageBackground;
                    DrawImageBackground(imageBackground, bitmap, session);
                }
            }

            return bitmap;
        }

        private static void DrawGridLineBackground(GridLineBackground background, CanvasBitmap bitmap, CanvasDrawingSession session)
        {
            var lineColor = new CanvasSolidColorBrush(bitmap, background.LineColor);

            if (background.HorizontalLineSpacing > 0)
            {
                double y = background.HorizontalLineSpacing;
                while (y < bitmap.Bounds.Height - 1)
                {
                    session.FillRectangle(
                        new Rect(
                            0,
                            y - background.HorizontalLineThickness / 2,
                            bitmap.Bounds.Width,
                            background.HorizontalLineThickness),
                        lineColor);

                    y += background.HorizontalLineSpacing;
                }
            }

            if (background.VerticalLineSpacing > 0)
            {
                double x = background.VerticalLineSpacing;
                while (x < bitmap.Bounds.Width)
                {
                    session.FillRectangle(
                        new Rect(
                            x - background.VerticalLineThickness / 2,
                            0,
                            background.VerticalLineThickness,
                            bitmap.Bounds.Height),
                        lineColor);

                    x += background.VerticalLineSpacing;
                }
            }
        }

        private static void DrawImageBackground(ImageBackground background, CanvasBitmap bitmap, CanvasDrawingSession session)
        {
            if (background.Image != null)
            {
                session.DrawImage(background.Image, bitmap.Bounds);
            }
        }
    }
}
