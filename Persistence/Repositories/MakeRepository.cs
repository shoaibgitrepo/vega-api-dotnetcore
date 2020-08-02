using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Core.Repositories;

namespace vega_api_dotnetcore.Persistence.Repositories
{
    public class MakeRepository : Repository<Make>, IMakeRepository
    {
        public MakeRepository(VegaDbContext context)
            : base(context)
        {
        }

        public VegaDbContext vegaDbContext
        {
            get { return (VegaDbContext)context; }
        }

        public async Task<IEnumerable<Make>> GetMakesWithModelsAsync()
        {
            return await vegaDbContext.Makes
                .Include(m => m.Models)
                .ToListAsync();
        }
    }
}