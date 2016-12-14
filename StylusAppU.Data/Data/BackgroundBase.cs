using System.Runtime.Serialization;
using Windows.UI;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public abstract class BackgroundBase
    {
        public BackgroundBase()
        {
            BackgroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
        }

        [OnDeserialized]
        protected virtual void OnDeserialized(StreamingContext context)
        {
            if (BackgroundColor.A == 0)
            {
                BackgroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
            }
        }

        [DataMember]
        public Color BackgroundColor { get; set; }
    }
}
