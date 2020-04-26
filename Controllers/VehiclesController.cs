using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Models;
using vega_api_dotnetcore.Persistence;

namespace vega_api_dotnetcore.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly VegaDbContext context;
        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleResource vehicleResource)
        {
            // Domain model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vehicle modelId validation
            var model = await context.Models.FindAsync(vehicleResource.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("ModelId", "Invalid modelId.");
                return BadRequest(ModelState);
            }

            // Vehicle features validation
            var features = await context.Features.Where(f => vehicleResource.Features.Contains(f.Id)).ToListAsync();

            if (features.Count < vehicleResource.Features.Count)
            {
                ModelState.AddModelError("Features", "Invalid featureId.");
                return BadRequest(ModelState);
            }

            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);

            vehicle.LastUpdate = DateTime.Now;
            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleResource vehicleResource)
        {
            // Domain model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vehicle modelId validation
            var model = await context.Models.FindAsync(vehicleResource.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("ModelId", "Invalid modelId.");
                return BadRequest(ModelState);
            }

            // Vehicle features validation
            var features = await context.Features.Where(f => vehicleResource.Features.Contains(f.Id)).ToListAsync();
            if (features.Count < vehicleResource.Features.Count)
            {
                ModelState.AddModelError("Features", "Invalid featureId.");
                return BadRequest(ModelState);
            }

            var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            //return Ok(vehicle);
            mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await context.SaveChangesAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }
    }
}