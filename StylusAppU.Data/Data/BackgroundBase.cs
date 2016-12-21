using System.Runtime.Serialization;
using Windows.UI;

namespace StylusAppU.Data.Data
{
    [DataContract]
    [KnownType(typeof(SolidBackground))]
    [KnownType(typeof(ImageBackground))]
    [KnownType(typeof(GridLineBackground))]
    public abstract class BackgroundBase
    {
        public BackgroundBase()
        {
            BackgroundColor = new Color() { A = 255, R = 255, G = 255, B = 255 };
        }

        protected void OnDeserializedBase(StreamingContext context)
        {
        }

        [DataMember]
        public Color BackgroundColor { get; set; }
    }
}
