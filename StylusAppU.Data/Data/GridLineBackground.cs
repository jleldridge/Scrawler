using System.Runtime.Serialization;
using Windows.UI;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class GridLineBackground : BackgroundBase
    {
        public GridLineBackground()
        {
            LineColor = new Color() { A = 255, R = 0, G = 0, B = 0 };
        }

        [OnDeserialized]
        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

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
