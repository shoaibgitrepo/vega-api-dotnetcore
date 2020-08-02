using System.Collections.Generic;
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
    public class FeaturesController
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;
        public FeaturesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
            var features = await unitOfWork.Features.GetEntitiesAsync();

            return mapper.Map<IEnumerable<Feature>, IEnumerable<KeyValuePairResource>>(features);
        }
    }
}