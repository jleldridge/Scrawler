using System.Runtime.Serialization;
using Windows.UI;

namespace Scrawler.Data.Data
{
    [DataContract]
    public class GridLineBackground : BackgroundBase
    {
        public GridLineBackground()
        {
            LineColor = new Color() { A = 255, R = 0, G = 0, B = 0 };
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserializedBase(context);

            if (LineColor.A == 0)
            {
                LineColor = new Color() { A = 255, R = 0, G = 0, B = 0 };
            }
        }

        [DataMember]
        public double HorizontalLineThickness { get; set; }

        [DataMember]
        public double VerticalLineThickness { get; set; }

        [DataMember]
        public double HorizontalLineSpacing { get; set; }

        [DataMember]
        public double VerticalLineSpacing { get; set; }

        [DataMember]
        public Color LineColor { get; set; }
    }
}
