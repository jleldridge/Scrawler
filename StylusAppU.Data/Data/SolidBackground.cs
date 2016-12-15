using System.Runtime.Serialization;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class SolidBackground : BackgroundBase
    {
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserializedBase(context);
        }
    }
}
