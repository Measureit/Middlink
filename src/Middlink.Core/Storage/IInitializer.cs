using System.Threading.Tasks;

namespace Middlink.Core.Storage
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
