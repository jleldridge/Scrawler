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
        public static async Task SaveImage(CanvasBitmap backgroundBitmap, CanvasBitmap inkBitmap, StorageFile file)
        {
            var device = CanvasDevice.GetSharedDevice();
            var bitmap = new CanvasRenderTarget(device, (float)backgroundBitmap.Bounds.Width, (float)backgroundBitmap.Bounds.Height, 96.0f);

            using (var session = bitmap.CreateDrawingSession())
            {
                session.Units = CanvasUnits.Pixels;
                session.DrawImage(backgroundBitmap);
                session.DrawImage(inkBitmap);
            }

            var outputBitmap = new SoftwareBitmap(
                BitmapPixelFormat.Bgra8,
                (int)bitmap.SizeInPixels.Width,
                (int)bitmap.SizeInPixels.Height,
                BitmapAlphaMode.Premultiplied);
            outputBitmap.CopyFromBuffer(bitmap.GetPixelBytes().AsBuffer());

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetSoftwareBitmap(outputBitmap);
                encoder.IsThumbnailGenerated = true;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception ex)
                {
                    switch (ex.HResult)
                    {
                        //WINCODEC_ERR_UNSUPPORTEDOPERATION
                        // If the encoder does not support writing a thumbnail, then try again
                        // but disable thumbnail generation.
                        case unchecked((int)0x88982F81):
                            encoder.IsThumbnailGenerated = false;
                            break;
                    }
                }
                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }
        }
    }
}
