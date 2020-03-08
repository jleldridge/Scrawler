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

        [DataMember]
        public ImageScaleSetting ImageScaleSetting { get; set; }

        [DataMember]
        public float PageScale { get; set; }

        public CanvasBitmap Image { get; set; }
    }

    public enum ImageScaleSetting
    {
        ScaleImageToPage,
        ScalePageToImage
    }
}
