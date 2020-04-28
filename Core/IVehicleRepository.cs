using System.Collections.Generic;
using System.Threading.Tasks;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Core
{
    public interface IVehicleRepository
    {
        void Add(Vehicle vehicle);
        Task<Vehicle> GetVehicleAsync(int id, bool includeRelated = true);
        Task<IEnumerable<Vehicle>> GetVehiclesAsync(VehicleQuery filter, bool includeRelated = true);
        Task<Vehicle> GetVehicleWithMakeAsync(int id);
        void Remove(Vehicle vehicle);
    }
}