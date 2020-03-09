using System.Runtime.Serialization;

namespace Scrawler.Data.Data
{
    [DataContract]
    public class Defaults : IDeepCopiable<Defaults>
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

        public Defaults GetDeepCopy()
        {
            var copy = new Defaults();
            copy.Background = Background.GetDeepCopy();
            copy.PageWidth = PageWidth;
            copy.PageHeight = PageHeight;

            return copy;
        }
    }
}
