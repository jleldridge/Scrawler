using System.Runtime.Serialization;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class ImageBackground : BackgroundBase
    {
        public ImageBackground()
        {
        }

        [DataMember]
        public string ImageFilePath { get; set; }
    }
}
