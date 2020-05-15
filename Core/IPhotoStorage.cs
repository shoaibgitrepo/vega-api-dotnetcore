using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace vega_api_dotnetcore.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}