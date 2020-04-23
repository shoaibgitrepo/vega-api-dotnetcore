using AutoMapper;
using vega_api_dotnetcore.Controllers.Resources;
using vega_api_dotnetcore.Models;

namespace vega_api_dotnetcore.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
        }
    }
}