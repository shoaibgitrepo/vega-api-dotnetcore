using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Core;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Persistence;

namespace vega_api_dotnetcore.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly VegaDbContext context;
        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork, VegaDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        // [Authorize(Policies.RequireAdminRole)]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
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



            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);

            vehicle.LastUpdate = DateTime.Now;
            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicleAsync(vehicle.Id);

            var result = mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            // Domain model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // // Vehicle modelId validation
            // var model = await context.Models.FindAsync(vehicleResource.ModelId);
            // if (model == null)
            // {
            //     ModelState.AddModelError("ModelId", "Invalid modelId.");
            //     return BadRequest(ModelState);
            // }

            // // Vehicle features validation
            // var features = await context.Features.Where(f => vehicleResource.Features.Contains(f.Id)).ToListAsync();
            // if (features.Count < vehicleResource.Features.Count)
            // {
            //     ModelState.AddModelError("Features", "Invalid featureId.");
            //     return BadRequest(ModelState);
            // }

            var vehicle = await repository.GetVehicleAsync(id);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicleAsync(vehicle.Id);

            var result = mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicleAsync(id, includeRelated: false);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GetVehicleAsync(id);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            var vehicleResouce = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResouce);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await repository.GetVehiclesAsync(filter);

            return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}