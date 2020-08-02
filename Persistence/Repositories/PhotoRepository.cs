using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Core.Repositories;
using vega_api_dotnetcore.Core.Models;

namespace vega_api_dotnetcore.Persistence.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(VegaDbContext context)
            : base(context)
        {
        }

        public VegaDbContext vegaDbContext
        {
            get { return (VegaDbContext)context; }
        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await vegaDbContext.Photos
                 .Where(p => p.VehicleId == vehicleId)
                 .ToListAsync();
        }
    }
}