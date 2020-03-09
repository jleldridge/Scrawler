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
            copy.ImageFileName = Guid.NewGuid().ToString();
            copy.Image = CanvasBitmap.CreateFromDirect3D11Surface(CanvasDevice.GetSharedDevice(), Image);
            
            return copy;
        }
    }

    public enum ImageScaleSetting
    {
        ScaleImageToPage,
        ScalePageToImage
    }
}
