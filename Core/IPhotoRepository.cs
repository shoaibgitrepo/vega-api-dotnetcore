using System.Collections.Generic;
using System.Threading.Tasks;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Core
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}