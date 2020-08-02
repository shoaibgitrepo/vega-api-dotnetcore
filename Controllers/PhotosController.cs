using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Core.Repositories;
using vega_api_dotnetcore.Core.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;
using vega_api_dotnetcore.Core;

namespace vega_api_dotnetcore.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;
        private readonly PhotoSettings photoSettings;
        private readonly IPhotoService photoService;
        private readonly IUnitOfWork unitOfWork;
        public PhotosController(
            IWebHostEnvironment env,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsSnapshot<PhotoSettings> options,
            IPhotoService photoService)
        {
            this.unitOfWork = unitOfWork;
            this.photoService = photoService;
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.env = env;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await unitOfWork.Photos.GetPhotos(vehicleId);

            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await unitOfWork.Vehicles.GetVehicleAsync(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound("Not found");

            if (file == null) return BadRequest("Null file.");
            if (file.Length == 0) return BadRequest("Empty file.");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("Max file szie exceeded.");
            if (!photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(env.WebRootPath, "uploads");

            var photo = await photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}