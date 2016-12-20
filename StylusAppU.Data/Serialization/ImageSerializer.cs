using System;
using Microsoft.Graphics.Canvas;
using StylusAppU.Data.Data;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace StylusAppU.Data.Serialization
{
    public static class ImageSerializer
    {
        public static async Task CreateImage(Page page, StorageFile file)
        {
            var device = CanvasDevice.GetSharedDevice();

            var target = new CanvasRenderTarget(device, (float)page.Width, (float)page.Height, 96.0f);
            using (var session = target.CreateDrawingSession())
            {
                session.Units = CanvasUnits.Pixels;
                session.Clear(page.Background.BackgroundColor);
                session.DrawInk(page.StrokeContainer.GetStrokes());
            }

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var outputBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, (int)target.SizeInPixels.Width, (int)target.SizeInPixels.Height);
                outputBitmap.CopyFromBuffer(target.GetPixelBytes().AsBuffer());
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetSoftwareBitmap(outputBitmap);
                await encoder.FlushAsync();
            }
        }
    }
}
