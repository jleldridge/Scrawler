﻿using System.Runtime.Serialization;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class ImageBackground : BackgroundBase
    {
        public ImageBackground()
        {
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserializedBase(context);
        }


        [DataMember]
        public string ImageFilePath { get; set; }
    }
}
