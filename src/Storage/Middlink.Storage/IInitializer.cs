using System.Threading.Tasks;

namespace Middlink.Storage
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
