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
        public bool ScaleImageToPage { get; set; }

        public CanvasBitmap Image { get; set; }
    }
}
