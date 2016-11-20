using System;
using System.Runtime.Serialization;

namespace StylusAppU.Data.Data
{
    [DataContract]
    public class Page : IEquatable<Page>
    {
        internal Page()
        {
            Guid = Guid.NewGuid();
            InkFileName = Guid.ToString();
            BackgroundFileName = Guid.ToString();
        }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string InkFileName { get; set; }

        [DataMember]
        public string BackgroundFileName { get; set; }

        public bool Equals(Page other)
        {
            return other.Guid.Equals(Guid) 
                && other.InkFileName.Equals(InkFileName) 
                && other.BackgroundFileName.Equals(BackgroundFileName);
        }
    }
}
