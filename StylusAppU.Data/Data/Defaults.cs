using System.Runtime.Serialization;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class Defaults
    {
        public Defaults()
        {
            Background = new SolidBackground();
            PageWidth = 800;
            PageHeight = 600;
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Background == null)
            {
                Background = new SolidBackground();
            }
            if (PageWidth == 0)
            {
                PageWidth = 800;
            }
            if (PageHeight == 0)
            {
                PageHeight = 600;
            }
        }

        [DataMember]
        public BackgroundBase Background { get; set; }

        [DataMember]
        public double PageWidth { get; set; }

        [DataMember]
        public double PageHeight { get; set; }
    }
}
