using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Core;
using vega_api_dotnetcore.Core.Models;
using vega_api_dotnetcore.Persistence;

namespace vega_api_dotnetcore.Controllers
{
    public class MakesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public MakesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await unitOfWork.Makes.GetMakesWithModelsAsync();

            return mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(makes);
        }
    }
}