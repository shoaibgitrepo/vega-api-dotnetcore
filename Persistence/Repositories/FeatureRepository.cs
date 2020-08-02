using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Core.Repositories;

namespace vega_api_dotnetcore.Persistence.Repositories
{
    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        public FeatureRepository(VegaDbContext context)
            : base(context)
        {
        }

        public VegaDbContext vegaDbContext
        {
            get { return (VegaDbContext)context; }
        }
    }
}