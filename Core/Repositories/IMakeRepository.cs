using System.Collections.Generic;
using System.Threading.Tasks;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Core.Repositories
{
    public interface IMakeRepository : IRepository<Make>
    {
        Task<IEnumerable<Make>> GetMakesWithModelsAsync();
    }
}