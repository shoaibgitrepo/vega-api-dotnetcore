using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Core;

namespace vega_api_dotnetcore.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public VehiclesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        // [Authorize(Policies.RequireAdminRole)]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
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



            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);

            vehicle.LastUpdate = DateTime.Now;
            await unitOfWork.Vehicles.AddAsync(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await unitOfWork.Vehicles.GetVehicleAsync(vehicle.Id);

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

            var vehicle = await unitOfWork.Vehicles.GetVehicleAsync(id);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            vehicle = await unitOfWork.Vehicles.GetVehicleAsync(vehicle.Id);

            var result = mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await unitOfWork.Vehicles.GetVehicleAsync(id, includeRelated: false);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            unitOfWork.Vehicles.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await unitOfWork.Vehicles.GetVehicleAsync(id);

            if (vehicle == null)
                return NotFound("Invalid vehicleId.");

            var vehicleResouce = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResouce);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await unitOfWork.Vehicles.GetVehiclesAsync(filter);

            return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}