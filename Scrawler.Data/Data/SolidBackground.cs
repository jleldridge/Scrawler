using System.Runtime.Serialization;

namespace Scrawler.Data.Data
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
