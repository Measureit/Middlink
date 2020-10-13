using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Middlink.Repositories
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync();
    }
}
