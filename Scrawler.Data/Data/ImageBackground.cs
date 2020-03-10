using Microsoft.Graphics.Canvas;
using System;
using System.Runtime.Serialization;

namespace Scrawler.Data.Data
{
    [DataContract]
    public class ImageBackground : BackgroundBase
    {
        public ImageBackground()
        {
            ImageFileName = Guid.NewGuid().ToString();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserializedBase(context);
        }


        [DataMember]
        public string ImageFileName { get; set; }

        public CanvasBitmap Image { get; set; }

        public override BackgroundBase GetDeepCopy()
        {
            var copy = new ImageBackground();
            copy.BackgroundColor = BackgroundColor;
            // for ImageBackgrounds the image itself should be
            // immutable, so we can reuse the file and in-memory
            // bitmap to avoid memory or file-size bloat.
            copy.ImageFileName = ImageFileName;
            copy.Image = Image;
            
            return copy;
        }
    }

    public enum ImageScaleSetting
    {
        ScaleImageToPage,
        ScalePageToImage
    }
}
