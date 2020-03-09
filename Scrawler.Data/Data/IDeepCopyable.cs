using Scrawler.Data.Serialization;

namespace Scrawler.Data.Data
{
    public interface IDeepCopyable<T>
    {
        T GetDeepCopy();
    }
}
