using System.Threading.Tasks;

namespace Middlink.Storage.MongoDb
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}
