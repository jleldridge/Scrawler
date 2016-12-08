using System;
using System.Runtime.Serialization;
using Windows.UI.Input.Inking;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class Page : IEquatable<Page>
    {
        internal Page()
        {
            Guid = Guid.NewGuid();
            InkFileName = Guid.ToString();
            BackgroundFileName = Guid.ToString();
            Width = 800;
            Height = 600;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Width == 0) Width = 800;
            if (Height == 0) Height = 600;
        }

        [DataMember]
        public double Width { get; set; }

        [DataMember]
        public double Height { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string InkFileName { get; set; }

        [DataMember]
        public string BackgroundFileName { get; set; }

        public InkStrokeContainer StrokeContainer { get; set; }

        public bool Equals(Page other)
        {
            return other.Guid.Equals(Guid) 
                && other.InkFileName.Equals(InkFileName) 
                && other.BackgroundFileName.Equals(BackgroundFileName);
        }
    }
}
