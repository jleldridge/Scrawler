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

        public override BackgroundBase GetDeepCopy()
        {
            var copy = new SolidBackground();
            copy.BackgroundColor = BackgroundColor;
            return copy;
        }
    }
}
