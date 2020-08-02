using System.Collections.Generic;
using System.Threading.Tasks;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Core.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle> GetVehicleAsync(int id, bool includeRelated = true);
        Task<QueryResult<Vehicle>> GetVehiclesAsync(VehicleQuery filter, bool includeRelated = true);
        Task<Vehicle> GetVehicleWithMakeAsync(int id);
    }
}