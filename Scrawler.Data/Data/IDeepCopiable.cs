using Scrawler.Data.Serialization;

namespace Scrawler.Data.Data
{
    public interface IDeepCopiable<T>
    {
        T GetDeepCopy();
    }
}
