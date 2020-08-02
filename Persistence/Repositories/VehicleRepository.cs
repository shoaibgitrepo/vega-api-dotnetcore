using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Core.Repositories;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Extensions;

namespace vega_api_dotnetcore.Persistence.Repositories
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(VegaDbContext context)
            : base(context)
        {
        }

        public VegaDbContext vegaDbContext
        {
            get { return (VegaDbContext)context; }
        }

        public async Task<QueryResult<Vehicle>> GetVehiclesAsync(VehicleQuery queryObj, bool includeRelated = true)
        {
            // if (!includeRelated)
            //     return await context.Vehicles.ToListAsync();

            var result = new QueryResult<Vehicle>();

            var query = vegaDbContext.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .AsQueryable();

            query = query.ApplyFiltering(queryObj);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                ["id"] = v => v.Id
            };
            query = query.ApplySorting(queryObj, columnsMap);

            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(queryObj);

            result.Items = await query.ToListAsync();
            return result;
        }

        public async Task<Vehicle> GetVehicleAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await vegaDbContext.Vehicles.FindAsync(id);

            return await vegaDbContext.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> GetVehicleWithMakeAsync(int id)
        {
            return await vegaDbContext.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
        }
    }
}