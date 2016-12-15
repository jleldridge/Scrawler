using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Utils.Serialization
{
    public static class DataContractHelper
    {
        public static T Clone<T>(T source)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
