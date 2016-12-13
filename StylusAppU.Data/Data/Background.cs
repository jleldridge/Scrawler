using System.Runtime.Serialization;
using Windows.UI;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class Background
    {
        public Background()
        {
            BackgroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (BackgroundColor.A == 0)
            {
                BackgroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            }

            if (LineColor.A == 0)
            {
                LineColor = new Color() { A = 255, R = 0, G = 0, B = 0 };
            }
        }

        [DataMember]
        public BackgroundType BackgroundType { get; set; }

        [DataMember]
        public double HorizontalLineHeight { get; set; }

        [DataMember]
        public double VerticalLineWidth { get; set; }

        [DataMember]
        public double HorizontalLineSpacing { get; set; }

        [DataMember]
        public double VerticalLineSpacing { get; set; }

        [DataMember]
        public Color LineColor { get; set; }

        [DataMember]
        public Color BackgroundColor { get; set; }
    }

    public enum BackgroundType
    {
        Solid = 0,
        HorizontalLines,
        VerticalLines,
        Grid,
        Image
    }
}
